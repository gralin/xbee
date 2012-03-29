using System.Collections;
using System.Text;
using Microsoft.SPOT;
using NETMF.OpenSource.XBee.Api;
using NETMF.OpenSource.XBee.Api.Common;
using XBee = NETMF.OpenSource.XBee.Api.XBee;

namespace Gadgeteer.Tester
{
    public partial class Program
    {
        private const string Ping = "PING";
        private const string Pong = "PONG";
        private ArrayList _nodes;

        void ProgramStarted()
        {
            lED7R.TurnLightOn(7, true);

            coordinator.Configure();
            coordinator.Api.DataReceived += OnDataReceived;

            endDevice.Configure();
            endDevice.Api.DataReceived += OnDataReceived;

            router.Configure();
            router.Api.StatusChanged += (x, s) => OnStatusChanged(s);
            router.Api.DataReceived += OnDataReceived;

            Debug.Print(coordinator.Api.Config.ToString());
            Debug.Print(router.Api.Config.ToString());
            Debug.Print(endDevice.Api.Config.ToString());
        }

        private void OnStatusChanged(ModemStatus status)
        {
            if (status == ModemStatus.Associated)
                DiscoverNodes();
        }

        private void DiscoverNodes()
        {
            _nodes = new ArrayList();

            router.Api.DiscoverNodes(node =>
            {
                var nodeInfo = node.NodeInfo;

                if (_nodes.Contains(nodeInfo))
                    return;

                _nodes.Add(nodeInfo);

                PrintNode(_nodes.Count, node);

                // depending on XBee settings discovery may return local node info
                if (nodeInfo.SerialNumber != router.Api.Config.SerialNumber)
                    router.Api.Send(Ping).To(nodeInfo).NoResponse();
            });
        }

        private void PrintNode(int nodeNumber, NodeDiscover nodeInfo)
        {
            Debug.Print("#" + nodeNumber + " - " + nodeInfo);
            lED7R.TurnLightOn(nodeNumber);
        }

        private static void OnDataReceived(XBee receiver, byte[] data, XBeeAddress sender)
        {
            var dataStr = new string(Encoding.UTF8.GetChars(data));

            switch (dataStr)
            {
                case Ping:
                    receiver.Send(Pong).To(sender).NoResponse();
                    break;

                case Pong:
                    Debug.Print("Received Pong from " + sender);
                    break;
            }
        }
    }
}
