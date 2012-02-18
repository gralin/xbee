using System;
using System.Collections;
using System.IO;
using System.Threading;
using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Api.Wpan;
using Gadgeteer.Modules.GHIElectronics.Api.Zigbee;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Reads a packet from the input stream, verifies checksum and creates an XBeeResponse object
    /// </summary>
    /// <remarks>
    /// Escaped bytes increase packet length but packet stated length only indicates un-escaped bytes.
    /// Stated length includes all bytes after Length bytes, not including the checksum
    /// </remarks>
    public class PacketParser : IPacketParser
    {
        private static Hashtable _responseHandler;
        private readonly Checksum _checksum;
        private XBeeResponse _response;
        private int _escapedBytes;

        public const int DefaultParseTimeout = 5000;
        public int ParseTimeout { get; set; }
        public DateTime ParseStartTime { get; private set; }
        public TimeSpan ParseElapsedTime { get { return DateTime.Now.Subtract(ParseStartTime); } }
        public int ParseTimeLeft { get { return (int) (ParseTimeout - ParseElapsedTime.Ticks/TimeSpan.TicksPerMillisecond); } }

        protected bool CurrentBufferEmpty
        {
            get
            {
                return _currentBuffer == null || _currentBuffer.Position == _currentBuffer.Length;
            }
        }

        private readonly Queue _buffers;
        private readonly ManualResetEvent _buffersAvailable;
        private Stream _currentBuffer;

        private Thread _parsingThread;
        private bool _finished;

        private readonly ArrayList _packetListeners;

        public PacketParser()
        {
            ParseTimeout = DefaultParseTimeout;
            Length = 0;

            _checksum = new Checksum();
            _buffers = new Queue();
            _buffersAvailable = new ManualResetEvent(false);

            _packetListeners = new ArrayList();
        }

        public void Start()
        {
            Stop();

            _parsingThread = new Thread(ParsePackets);
            _parsingThread.Start();
        }

        public void Stop()
        {
            if (_parsingThread == null)
                return;

            _finished = true;
            _buffersAvailable.Set();

            if (!_parsingThread.Join(ParseTimeout))
            {
                Logger.Error("Failed to stop parsing thread!");
                _parsingThread.Abort();
            }

            _parsingThread = null;
        }

        public void AddToParse(byte[] data)
        {
            Logger.LowDebug("Received " + data.Length + " bytes");
             
            lock (_buffers)
            {
                _buffers.Enqueue(data);
                _buffersAvailable.Set();    
            }
        }

        public void AddPacketListener(IPacketListener listener)
        {
            lock (_packetListeners)
            {
                _packetListeners.Add(listener);
            }
        }

        public void RemovePacketListener(IPacketListener listener)
        {
            lock (_packetListeners)
            {
                _packetListeners.Remove(listener);
            }
        }

        private void ParsePackets()
        {
            while (!_finished)
            {
                var b = TakeFromBuffer();

                if (_finished)
                    return;

                if (!XBeePacket.IsSpecialByte(b) || b != (int) XBeePacket.SpecialByte.START_BYTE) 
                    continue;

                try
                {
                    var packet = ParsePacket();

                    if (Logger.IsActive(LogLevel.Debug))
                        Logger.Debug("Received " + packet.GetType().Name + ": " + packet);

                    lock (_packetListeners)
                    {
                        foreach (var obj in _packetListeners)
                        {
                            var packetListener = (IPacketListener) obj;
                            packetListener.ProcessPacket(packet);
                        }
                    }
                }
                catch (XBeeTimeoutException)
                {
                    Logger.LowDebug("Incomplete packet received");
                }
                catch (XBeeParseException)
                {
                    Logger.Warn("Errors occured while parsing received packet");
                }
                catch (Exception e)
                {
                    Logger.Error("Unexpected exception occured while parsing packet. " + e.Message);
                }
            }
        }

        private XBeeResponse ParsePacket()
        {
            try
            {
                ParseStartTime = DateTime.Now;
                BytesRead = 0;
                _checksum.Clear();
                
                // length of api structure, starting here (not including start byte or length bytes, or checksum)
                // length doesn't account for escaped bytes
                Length = UshortUtils.ToUshort(Read("Length MSB"), Read("Length LSB"));

                Logger.LowDebug("packet length is " + ByteUtils.ToBase16(Length));

                // total packet length = stated length + 1 start byte + 1 checksum byte + 2 length bytes

                ApiId = (ApiId)Read("API ID");

                Logger.LowDebug("Handling ApiId: " + ApiId);

                // TODO parse I/O data page 12. 82 API Identifier Byte for 64 bit address A/D data (83 is for 16bit A/D data)
                // TODO XBeeResponse should implement an abstract parse method

                _response = GetResponse(ApiId);

                if (_response == null)
                {
                    Logger.Warn("Did not find a response handler for ApiId [" + ByteUtils.ToBase16((int)ApiId));
                    _response = new GenericResponse();
                }

                _response.Parse(this);
                _response.Checksum = Read("Checksum");

                if (RemainingBytes > 0)
                    throw new XBeeParseException("There are remaining bytes after parsing the packet");

                _response.Finish();
            }
            catch (Exception e)
            {
                Logger.Error("Failed to parse packet due to exception. " + e.Message);
                _response = new ErrorResponse { ErrorMsg = e.Message, Exception = e };
            }
            finally
            {
                if (_response != null)
                {
                    _response.Length = Length;
                    _response.ApiId = ApiId;
                }
            }

            return _response;
        }

        private static void SetupResponseHandlers()
        {
            _responseHandler = new Hashtable(13)
            {
                {ApiId.AT_RESPONSE, typeof (AtResponse)},
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
            var constructor = responseClass.GetConstructor(new Type[0]);

            if (constructor == null)
                return null;

            return constructor.Invoke(null) as XBeeResponse;
        }

        private int TakeFromBuffer(int timeout = 0)
        {
            if (CurrentBufferEmpty)
                GetNextBuffer(timeout);

            return _currentBuffer.ReadByte();
        }

        private void GetNextBuffer(int timeout = 0)
        {
            if (_currentBuffer != null)
            {
                _currentBuffer.Dispose();
                _currentBuffer = null;
            }

            if (timeout > 0)
            {
                if (!_buffersAvailable.WaitOne(timeout, false))
                    throw new XBeeTimeoutException();
            }
            else
            {
                _buffersAvailable.WaitOne();
            }

            // this can happen if Stop() was called
            if (_buffers.Count == 0) 
                return;
            
            lock (_buffers)
            {
                var newBuffer = (byte[])_buffers.Dequeue();
                _currentBuffer = new MemoryStream(newBuffer);

                if (_buffers.Count == 0)
                    _buffersAvailable.Reset();
            }
        }

        #region IPacketParser Members

        public ApiId ApiId { get; protected set; }

        public ushort Length { get; protected set; }

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
                return Length - FrameDataBytesRead + 1;
            }
        }

        /// <summary>
        /// This method reads bytes from the underlying input stream and performs the following tasks:
        /// 1. Keeps track of how many bytes we've read
        /// 2. Un-escapes bytes if necessary and verifies the checksum.
        /// </summary>
        /// <returns></returns>
        public int Read()
        {
            if (RemainingBytes == 0)
                throw new XBeeParseException("Packet has read all of its bytes");

            var b = TakeFromBuffer(ParseTimeLeft);

            if (XBeePacket.IsSpecialByte(b))
            {
                Logger.LowDebug("Read special byte that needs to be unescaped");

                if (b == (int)XBeePacket.SpecialByte.ESCAPE)
                {
                    Logger.LowDebug("found escape byte");
                    // read next byte
                    b = TakeFromBuffer(ParseTimeLeft);

                    Logger.LowDebug("next byte is " + ByteUtils.FormatByte(b));
                    b = 0x20 ^ b;
                    Logger.LowDebug("unescaped (xor) byte is " + ByteUtils.FormatByte(b));

                    _escapedBytes++;
                }
                else
                {
                    // TODO some responses such as AT Response for node discover do not escape the bytes?? shouldn't occur if AP mode is 2?
                    // while reading remote at response Found unescaped special byte base10=19,base16=0x13,base2=00010011 at position 5 
                    Logger.LowDebug("Found unescaped special byte " + ByteUtils.FormatByte(b) + " at position " + BytesRead);
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

                //Logger.LowDebug("Read byte " + ByteUtils.FormatByte(b)
                //    + " at position " + BytesRead
                //    + ", packet length is " + Length.Get16BitValue()
                //    + ", #escapeBytes is " + _escapedBytes
                //    + ", remaining bytes is " + RemainingBytes);

                // escape bytes are not included in the stated packet length
                if (FrameDataBytesRead >= (Length + 1))
                {
                    // this is checksum and final byte of packet

                    Logger.LowDebug("Checksum byte is " + b);

                    if (!_checksum.Verify())
                        throw new XBeeParseException("Checksum is incorrect. Expected 0xff, but got 0x" 
                            + ByteUtils.ToBase16(_checksum.GetChecksum()));
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
            Logger.LowDebug("Read " + context + " byte, val is " + b);
            return b;
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

            Logger.LowDebug("There should be " + valueLength + " remaining bytes");

            for (var i = 0; i < valueLength; i++)
                value[i] = Read("Remaining byte " + i);

            return value;
        }

        public XBeeAddress16 ParseAddress16()
        {
            return new XBeeAddress16(Read("Address 16 MSB"), Read("Address 16 LSB"));
        }

        public XBeeAddress64 ParseAddress64()
        {
            var addr = new XBeeAddress64();

            for (var i = 0; i < 8; i++)
                addr[i] = Read("64-bit Address byte " + i);

            return addr;
        }

        #endregion
    }
}