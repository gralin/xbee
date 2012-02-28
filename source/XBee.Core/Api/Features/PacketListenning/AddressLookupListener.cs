using System;
using System.Collections;
using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Api.Zigbee;

namespace Gadgeteer.Modules.GHIElectronics.Api
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
            else if (packet is ZNetRxResponse)
            {
                var zigbeeResponse = packet as ZNetRxResponse;
                _addressLookup[zigbeeResponse.SourceSerial] = zigbeeResponse.SourceAddress;
            }
            else if (packet is ZNetTxStatusResponse && CurrentRequest is ZNetTxRequest)
            {
                var request = CurrentRequest as ZNetTxRequest;
                var response = packet as ZNetTxStatusResponse;
                _addressLookup[request.DestinationSerial] = response.DestinationAddress;
            }
        }
    }
}