using System;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    /// <summary>
    /// Series 1 XBee.
    /// Class for 16 and 64 bit address Transmit packets.
    /// </summary>
    public class TxRequest : XBeeRequest, IWpanPacket
    {
        public enum Options
        {
		    Unicast = 0,
		    DisableAck = 1,
		    Broadcast = 4
        }

        /// <summary>
        /// Maximum payload size as specified in the series 1 XBee manual.
        /// This is provided for reference only and is not used for validation
        /// </summary>
        public static int MaxPayloadSize = 100;

        private int[] _payload;

        public int[] Payload
        {
            get { return _payload; }
            set
            {
                if (value != null && value.Length > MaxPayloadSize)
                    throw new ArgumentOutOfRangeException("Max payload length is " + MaxPayloadSize);

                _payload = value;
            }
        }

        public Options Option { get; set; }

        public XBeeAddress Destination { get; set; }

        public override ApiId ApiId
        {
            get 
            { 
                return (Destination is XBeeAddress16) 
                    ? ApiId.TX_REQUEST_16 
                    : ApiId.TX_REQUEST_64; 
            }
        }

        public TxRequest(XBeeAddress destination, int[] payload, Options option = Options.Unicast, int frameId = PacketIdGenerator.DefaultId)
        {
            Destination = destination;
            Payload = payload;
            Option = option;
            FrameId = frameId;
        }

        public override int[] GetFrameData()
        {
            var output = new OutputStream();

            output.Write((byte)ApiId);
            output.Write(FrameId);
            output.Write(Destination.Address);
            output.Write((byte)Option);

            if (Payload != null)
                output.Write(Payload);

            return output.ToArray();
        }

        public override string ToString()
        {
            return base.ToString() 
                + ",destination=" + Destination
                + ",option=" + Option 
                + ",payload=int[" + Payload.Length + "]";
        }
    }
}