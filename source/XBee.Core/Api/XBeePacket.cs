using System;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Packages a frame data array into an XBee packet.
    /// </summary>
    public class XBeePacket
    {
        public enum SpecialByte
        {
            START_BYTE = 0x7e, // ~
            ESCAPE = 0x7d, // }
            XON = 0x11,
            XOFF = 0x13
        }

        private readonly int[] _packet;

        /// <summary>
        /// Performs the necessary activities to construct an XBee packet from the frame data.
        /// This includes: computing the checksum, escaping the necessary bytes, adding the start byte and length bytes.
        /// The format of a packet is as follows:
        /// start byte - msb length byte - lsb length byte - frame data - checksum byte
        /// </summary>
        /// <param name="frameData"></param>
        public XBeePacket(int[] frameData)
        {
            // checksum is always computed on pre-escaped packet
            var checksum = new Checksum();

            foreach (var b in frameData)
                checksum.AddByte(b);

            checksum.Compute();

            // packet size is frame data + start byte + 2 length bytes + checksum byte
            _packet = new int[frameData.Length + 4];
            _packet[0] = (int) SpecialByte.START_BYTE;

            // Packet length does not include escape bytes or start, length and checksum bytes
            var length = frameData.Length;

            // msb length (will be zero until maybe someday when > 255 bytes packets are supported)
            _packet[1] = UshortUtils.Msb(length);
            // lsb length
            _packet[2] = UshortUtils.Lsb(length);

            for (var i = 0; i < frameData.Length; i++)
            {
                if (frameData[i] > 255)
                    throw new Exception("Packet values must not be greater than one byte (255): " + frameData[i]);

                _packet[3 + i] = frameData[i];
            }

            // set last byte as checksum
            // note: if checksum is not correct, XBee won't send out packet or return error.  ask me how I know.

            _packet[_packet.Length - 1] = checksum.GetChecksum();

            var preEscapeLength = _packet.Length;

            // TODO save escaping for the serial out method. this is an unnecessary operation
            _packet = EscapePacket(_packet);

            var escapeLength = _packet.Length;

            var packetStr = "Packet: ";
            for (var i = 0; i < escapeLength; i++)
            {
                packetStr += ByteUtils.ToBase16(_packet[i]);

                if (i < escapeLength - 1)
                    packetStr += " ";
            }

            Logger.LowDebug(packetStr);
            Logger.LowDebug("pre-escape packet size is " + preEscapeLength + ", post-escape packet size is " + escapeLength);
        }

        /// <summary>
        /// Escape all bytes in packet after start byte, and including checksum
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private static int[] EscapePacket(int[] packet)
        {
            var escapeBytes = 0;

            // escape packet.  start at one so we don't escape the start byte 
            for (var i = 1; i < packet.Length; i++)
            {
                if (!IsSpecialByte(packet[i])) 
                    continue;
                
                Logger.LowDebug("escapeFrameData: packet byte requires escaping byte " + ByteUtils.ToBase16(packet[i]));
                escapeBytes++;
            }

            if (escapeBytes == 0)
			    return packet;

            Logger.LowDebug("packet requires escaping");
			
            var escapePacket = new int[packet.Length + escapeBytes];
			
            var pos = 1;
			
            escapePacket[0] = (int) SpecialByte.START_BYTE;
				
            for (var i = 1; i < packet.Length; i++) 
            {
                if (IsSpecialByte(packet[i])) 
                {
                    escapePacket[pos] = (int) SpecialByte.ESCAPE;
                    escapePacket[++pos] = 0x20 ^ packet[i];
                    Logger.LowDebug("escapeFrameData: xor'd byte is 0x" + ByteUtils.ToBase16(escapePacket[pos]));
                } 
                else 
                {
                    escapePacket[pos] = packet[i];
                }
				
                pos++;
            }
			
            return escapePacket;
        }

        public static int[] UnEscapePacket(int[] packet)
        {
		    var escapeBytes = 0;

            foreach (var b in packet)
                if (b == (int)SpecialByte.ESCAPE)
                    escapeBytes++;
		
		    if (escapeBytes == 0)
			    return packet;
		
		    var unEscapedPacket = new int[packet.Length - escapeBytes];
		
		    var pos = 0;
		
		    for (var i = 0; i < packet.Length; i++) 
            {
			    if (packet[i] == (int) SpecialByte.ESCAPE) 
                {
				    // discard escape byte and un-escape following byte
				    unEscapedPacket[pos] = 0x20 ^ packet[++i];
			    } 
                else 
                {
				    unEscapedPacket[pos] = packet[i];
			    }
			
			    pos++;
		    }
		
		    return unEscapedPacket;
        }

        public static bool IsSpecialByte(int b)
        {
            switch ((SpecialByte)b)
            {
                case SpecialByte.START_BYTE:
                case SpecialByte.ESCAPE:
                case SpecialByte.XON:
                case SpecialByte.XOFF:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the packet is valid
        /// </summary>
        /// <param name="packet"></param>
        /// <returns> true if the packet is valid</returns>
        public static bool Verify(int[] packet)
        {
            var valid = true;

            try
            {
                if (packet[0] != (int)SpecialByte.START_BYTE)
                    return false;
 
                // first need to unescape packet
                var unEscaped = UnEscapePacket(packet);

                var len = UshortUtils.ToUshort(unEscaped[1], unEscaped[2]);

                // stated packet length does not include start byte, length bytes, or checksum and is calculated before escaping

                var frameData = new int[len];

                var checksum = new Checksum();

                for (var i = 3; i < unEscaped.Length - 1; i++)
                {
                    frameData[i - 3] = unEscaped[i];
                    checksum.AddByte(frameData[i - 3]);
                }

                // add checksum byte to verify -- the last byte
                checksum.AddByte(unEscaped[unEscaped.Length - 1]);

                valid &= checksum.Verify();
            }
            catch (Exception e)
            {
                throw new Exception("Packet verification failed with error: ", e);
            }

            return valid;
        }

        public int[] ToByteArray()
        {
            return _packet;
        }
    }
}