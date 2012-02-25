using System;
using System.Collections;
using System.Threading;
using Gadgeteer.Modules.GHIElectronics.Api;
using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Api.Wpan;
using Gadgeteer.Modules.GHIElectronics.Util;
using Microsoft.SPOT;

namespace NETMF.Tester
{
    public static class WpanTest
    {
        public static void Run(XBee xbee1, XBee xbee2)
        {
            Debug.Print("XBee 1: " + (xbee1.Send(AtCmd.CoordinatorEnable).Value[0] > 0 ? "coordinator" : "end device"));
            Debug.Print("XBee 2: " + (xbee2.Send(AtCmd.CoordinatorEnable).Value[0] > 0 ? "coordinator" : "end device"));
            
            Debug.Print("Performing energy scan...");
            var result = xbee1.Send(AtCmd.EnergyScan, new byte[] { 3 }).Value;
            for (var i = 0; i < result.Length; i++)
                Debug.Print("Channel " + (i + 0x0B) + ": " + result[i] + "-dBi");

            Debug.Print("Active channel: " + xbee1.Send(AtCmd.OperatingChannel).Value[0]);

            // disovering nodes

            Debug.Print("Discovering nodes...");
            var foundNodes = DiscoverNodes(xbee1);

            Debug.Print("Found: " + foundNodes.Length + " nodes");

            for (var i = 0; i < foundNodes.Length; i++)
                Debug.Print("#" + (i + 1) + " - " + foundNodes[i]);

            // setting address 1 to xbee1 and address 2 to xbee2 if not available

            Debug.Print("Setting addresses...");

            var xbee1Address = GetAddress(xbee1);
            var xbee2Address = GetAddress(xbee2);

            xbee1Address.Address = 1;
            SetAddress(xbee1, xbee1Address);

            xbee2Address.Address = 2;
            SetAddress(xbee2, xbee2Address);

            Debug.Print("XBee 1 new address: " + xbee1Address);
            Debug.Print("XBee 2 new address: " + xbee2Address);

            // performing unicast message sending

            var xbee1Serial = xbee1.Config.SerialNumber;
            var xbee2Serial = xbee2.Config.SerialNumber;

            xbee1.AddPacketListener(new IncomingDataListener(xbee1Address));
            xbee2.AddPacketListener(new IncomingDataListener(xbee2Address));

            const string message1 = "serial unicast";
            Debug.Print(xbee1Address + " -> " + xbee2Serial + " (" + message1 + ")");
            xbee1.Send(xbee2Serial, message1);

            Thread.Sleep(1000);

            const string message2 = "address unicast";
            Debug.Print(xbee2Address + " -> " + xbee1Address + " (" + message2 + ")");
            xbee2.Send(xbee1Address, message2);

            Thread.Sleep(1000);

            // performing broadcast message sending

            const string message3 = "serial broadcast";
            Debug.Print(xbee1Address + " -> " + XBeeAddress64.Broadcast + " (" + message3 + ")");
            xbee1.SendAsync(XBeeAddress64.Broadcast, message3);

            Thread.Sleep(1000);

            const string message4 = "address broadcast";
            Debug.Print(xbee1Address + " -> "+ XBeeAddress16.Broadcast + " (" + message4 + ")");
            xbee1.SendAsync(XBeeAddress16.Broadcast, message4);
        }

        private static void SetAddress(XBee xbee, XBeeAddress16 address)
        {
            xbee.Send(AtCmd.NetworkAddress, (address as XBeeAddress).Address);
        }

        private static XBeeAddress16 GetAddress(XBee xbee)
        {
            return new XBeeAddress16(xbee.Send(AtCmd.NetworkAddress).Value);
        }

        private static WpanNodeDiscover[] DiscoverNodes(XBee xbee)
        {
            var asyncResult = xbee.BeginSend(xbee.CreateRequest(AtCmd.NodeDiscover), new NodeDiscoveryListener());

            var nodes = xbee.EndReceive(asyncResult, 5000);
            var result = new ArrayList();

            foreach (var node in nodes)
            {
                var foundNode = WpanNodeDiscover.Parse(node);
                if (foundNode != null)
                    result.Add(foundNode);
            }

            return (WpanNodeDiscover[])result.ToArray(typeof(WpanNodeDiscover));
        }

        class IncomingDataListener : IPacketListener
        {
            private readonly XBeeAddress16 _address;

            public IncomingDataListener(XBeeAddress16 receiverAddress)
            {
                _address = receiverAddress;
            }

            public bool Finished
            {
                get { return false; }
            }

            public void ProcessPacket(XBeeResponse packet)
            {
                if (!(packet is RxResponse)) 
                    return;
                
                var rxResponse = (RxResponse) packet;
                var message = Arrays.ToString(rxResponse.Payload);
                Debug.Print(_address + " <- " + rxResponse + " (" + message + ")");
            }

            public XBeeResponse[] GetPackets(int timeout)
            {
                throw new NotSupportedException();
            }
        }
    }
}