using System;
using System.Collections;
using System.IO;
using Gadgeteer.Modules.GHIElectronics.Api.Wpan;
using Gadgeteer.Modules.GHIElectronics.Api.Zigbee;
using Gadgeteer.Modules.GHIElectronics.Util;
using Microsoft.SPOT;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Reads a packet from the input stream, verifies checksum and creates an XBeeResponse object
    /// </summary>
    /// <remarks>
    /// Escaped bytes increase packet length but packet stated length only indicates un-escaped bytes.
    /// Stated length includes all bytes after Length bytes, not including the checksum
    /// </remarks>
    public class PacketParser : IIntInputStream, IPacketParser
    {
        private static Hashtable _responseHandler;
        private readonly IIntInputStream _input;
        private readonly Checksum _checksum;
        private XBeeResponse _response;
        private IIntArray _rawBytes;
        private bool _done;
        private int _escapedBytes;

        private static void SetupResponseHandlers()
        {
            _responseHandler = new Hashtable(13)
            {
                {ApiId.AT_RESPONSE, typeof (AtCommandResponse)},
                {ApiId.MODEM_STATUS_RESPONSE, typeof (ModemStatusResponse)},
                {ApiId.REMOTE_AT_RESPONSE, typeof (RemoteAtResponse)},
                {ApiId.RX_16_IO_RESPONSE, typeof (RxResponseIoSample)},
                {ApiId.RX_64_IO_RESPONSE, typeof (RxResponseIoSample)},
                {ApiId.RX_16_RESPONSE, typeof (RxResponse16)},
                {ApiId.RX_64_RESPONSE, typeof (RxResponse64)},
                {ApiId.TX_STATUS_RESPONSE, typeof (TxStatusResponse)},
                {ApiId.ZNET_EXPLICIT_RX_RESPONSE, typeof (ZNetExplicitRxResponse)},
                {ApiId.ZNET_IO_NODE_IDENTIFIER_RESPONSE, typeof (ZNetNodeIdentificationResponse)},
                {ApiId.ZNET_IO_SAMPLE_RESPONSE, typeof (ZNetRxIoSampleResponse)},
                {ApiId.ZNET_RX_RESPONSE, typeof (ZNetRxResponse)},
                {ApiId.ZNET_TX_STATUS_RESPONSE, typeof (ZNetTxStatusResponse)}
            };
        }

        private static XBeeResponse GetResponse(ApiId apiId)
        {
            if (_responseHandler == null)
                SetupResponseHandlers();

            if (_responseHandler == null || !_responseHandler.Contains(apiId))
                return null;

            var responseClass = (Type)_responseHandler[apiId];
            var constructor = responseClass.GetConstructor(null);

            if (constructor == null)
                return null;

            return constructor.Invoke(null) as XBeeResponse;
        }

        protected PacketParser()
        {
            _checksum = new Checksum();
        }

        public PacketParser(Stream input) 
            : this()
        {
            _input = new IntArrayInputStream(input);
        }

        public PacketParser(IIntInputStream input) 
            : this()
        {
            _input = input;
        }

        /// <summary>
        /// This method is guaranteed (unless I screwed up) to return an instance of XBeeResponse and should never throw an exception
        /// If an exception occurs, it will be packaged and returned as an ErrorResponse. 
        /// </summary>
        /// <returns></returns>
        public XBeeResponse ParsePacket()
        {
            try
            {
                // length of api structure, starting here (not including start byte or length bytes, or checksum)
                // length doesn't account for escaped bytes
                var length = new XBeePacketLength(Read("Length MSB"), Read("Length LSB"));

                Debug.Print("packet length is " + ByteUtils.ToBase16(length.GetLength()));

                // total packet length = stated length + 1 start byte + 1 checksum byte + 2 length bytes

                ApiId = (ApiId) Read("API ID");

                Debug.Print("Handling ApiId: " + ApiId);

                // TODO parse I/O data page 12. 82 API Identifier Byte for 64 bit address A/D data (83 is for 16bit A/D data)
                // TODO XBeeResponse should implement an abstract parse method

                _response = GetResponse(ApiId);

                if (_response == null)
                {
                    Debug.Print("Did not find a response handler for ApiId [" + ByteUtils.ToBase16((int)ApiId));
                    _response = new GenericResponse();
                }

                _response.Parse(this);
                _response.Checksum = Read("Checksum");

                if (!_done)
                    throw new XBeeParseException("There are remaining bytes according to stated packet length " 
                        + "but we have read all the bytes we thought were required for this packet (if that makes sense)");

                _response.Finish();
            }
            catch (Exception e)
            {
                // added bytes read for troubleshooting
                Debug.Print("Failed due to exception. Returning ErrorResponse. Bytes read: " + ByteUtils.ToBase16(_rawBytes.GetIntArray()));
                _response = new ErrorResponse { ErrorMsg = e.Message, Exception = e };
            }
            finally
            {
                if (_response != null)
                {
                    _response.Length = Length;
                    _response.ApiId = ApiId;
                    // preserve original byte array for transfer over networks
                    _response.RawPacketBytes = _rawBytes.GetIntArray();
                }
            }

            return _response;
        }

        #region IIntInputStream Members

        /// <summary>
        /// This method reads bytes from the underlying input stream and performs the following tasks:
        /// 1. Keeps track of how many bytes we've read
        /// 2. Un-escapes bytes if necessary and verifies the checksum.
        /// </summary>
        /// <returns></returns>
        public int Read()
        {
            if (_done)
                throw new XBeeParseException("Packet has read all of its bytes");

            var b = _input.Read();

            if (b == -1)
                throw new XBeeParseException("Read -1 from input stream while reading packet!");

            if (XBeePacket.IsSpecialByte(b))
            {
                Debug.Print("Read special byte that needs to be unescaped");

                if (b == (int)XBeePacket.SpecialByte.ESCAPE)
                {
                    Debug.Print("found escape byte");
                    // read next byte
                    b = _input.Read();

                    Debug.Print("next byte is " + ByteUtils.FormatByte(b));
                    b = 0x20 ^ b;
                    Debug.Print("unescaped (xor) byte is " + ByteUtils.FormatByte(b));

                    _escapedBytes++;
                }
                else
                {
                    // TODO some responses such as AT Response for node discover do not escape the bytes?? shouldn't occur if AP mode is 2?
                    // while reading remote at response Found unescaped special byte base10=19,base16=0x13,base2=00010011 at position 5 
                    Debug.Print("Found unescaped special byte " + ByteUtils.FormatByte(b) + " at position " + BytesRead);
                }
            }

            BytesRead++;

            // do this only after reading length bytes
            if (BytesRead > 2)
            {
                // when verifying checksum you must add the checksum that we are verifying
                // checksum should only include unescaped bytes!!!!
                // when computing checksum, do not include start byte, length, or checksum; when verifying, include checksum
                _checksum.AddByte(b);

                Debug.Print("Read byte " + ByteUtils.FormatByte(b) 
                    + " at position " + BytesRead 
                    + ", packet length is " + Length.Get16BitValue() 
                    + ", #escapeBytes is " + _escapedBytes 
                    + ", remaining bytes is " + RemainingBytes);

                // escape bytes are not included in the stated packet length
                if (FrameDataBytesRead >= (Length.Get16BitValue() + 1))
                {
                    // this is checksum and final byte of packet
                    _done = true;

                    Debug.Print("Checksum byte is " + b);

                    if (!_checksum.Verify())
                        throw new XBeeParseException("Checksum is incorrect.  Expected 0xff, but got " + _checksum.GetChecksum());
                }
            }

            return b;
        }

        /// <summary>
        /// Same as read() but logs the context of the byte being read.  useful for debugging
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public int Read(string context)
        {
            var b = Read();
            Debug.Print("Read " + context + " byte, val is " + b);
            return b;
        }

        public void Dispose()
        {
            _input.Dispose();
        }

        #endregion

        #region IPacketParser Members

        public ApiId ApiId { get; protected set; }
        public XBeePacketLength Length { get; protected set; }

        /// <summary>
        /// Returns number of bytes remaining, relative to the stated packet length (not including checksum).
        /// </summary>
        public int FrameDataBytesRead
        {
            get
            {
                // subtract out the 2 length bytes
                return BytesRead - 2;
            }
        }

        /// <summary>
        /// Does not include any escape bytes
        /// </summary>
        public int BytesRead { get; protected set; }

        /// <summary>
        /// Number of bytes remaining to be read, including the checksum
        /// </summary>
        public int RemainingBytes
        {
            get
            {
                // add one for checksum byte (not included) in packet length
                return Length.Get16BitValue() - FrameDataBytesRead + 1;
            }
        }

        /// <summary>
        /// Reads all remaining bytes except for checksum
        /// </summary>
        /// <returns></returns>
        public int[] ReadRemainingBytes()
        {
            // minus one since we don't read the checksum
            var valueLength = RemainingBytes - 1;
            var value = new int[valueLength];

            Debug.Print("There are " + valueLength + " remaining bytes");

            for (var i = 0; i < valueLength; i++)
                value[i] = Read("Remaining bytes " + i);

            return value;
        }

        public XBeeAddress16 ParseAddress16()
        {
            return new XBeeAddress16(new[]
            {
                Read("Address 16 MSB"),
                Read("Address 16 LSB")
            });
        }

        public XBeeAddress64 ParseAddress64()
        {
            var addr = new XBeeAddress64();

            for (var i = 0; i < 8; i++)
                addr.GetAddress()[i] = Read("64-bit Address byte " + i);

            return addr;
        }

        #endregion
    }
}