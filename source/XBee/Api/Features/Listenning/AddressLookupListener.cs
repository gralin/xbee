using System;
using System.Collections;
using NETMF.OpenSource.XBee.Api.Zigbee;

namespace NETMF.OpenSource.XBee.Api
{
    public class AddressLookupListener : IPacketListener
    {
        private readonly Hashtable _addressLookup;

        public XBeeRequest CurrentRequest { get; set; }

        public bool Finished
        {
            get { return false; }
        }

        public AddressLookupListener(Hashtable addressLookup)
        {
            _addressLookup = addressLookup;
        }

        public XBeeResponse[] GetPackets(int timeout)
        {
            throw new NotSupportedException();
        }

        public void ProcessPacket(XBeeResponse packet)
        {
            if (packet is RemoteAtResponse)
            {
                var atResponse = packet as RemoteAtResponse;
                _addressLookup[atResponse.RemoteSerial] = atResponse.RemoteAddress;
            }
            else if (packet is RxResponse)
            {
                var zigbeeResponse = packet as RxResponse;
                _addressLookup[zigbeeResponse.SourceSerial] = zigbeeResponse.SourceAddress;
            }
            else if (packet is TxStatusResponse && CurrentRequest is TxRequest)
            {
                var request = CurrentRequest as TxRequest;
                var response = packet as TxStatusResponse;

                _addressLookup[request.DestinationSerial] = 
                    response.DeliveryStatus == TxStatusResponse.DeliveryResult.Success
                        ? response.DestinationAddress
                        : XBeeAddress16.Unknown;
            }
            else if (packet is NodeIdentificationResponse)
            {
                var identPacket = packet as NodeIdentificationResponse;
                _addressLookup[identPacket.SenderSerial] = identPacket.SenderAddress;
                _addressLookup[identPacket.RemoteSerial] = identPacket.RemoteAddress;
            }
        }
    }
}