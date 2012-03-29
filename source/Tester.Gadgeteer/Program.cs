using Microsoft.SPOT;
using NETMF.OpenSource.XBee.Api;

namespace Gadgeteer.Tester
{
    public partial class Program
    {
        void ProgramStarted()
        {
            xbee.Configure();
            xbee.Api.StatusChanged += (x, s) => OnStatusChanged(s);
            Debug.Print(xbee.Api.Config.ToString());
            lED7R.TurnLightOn(7, true);
        }

        private void OnStatusChanged(ModemStatus status)
        {
            if (status == ModemStatus.Associated)
                DiscoverNodes();
        }

        private void DiscoverNodes()
        {
            var nodeCounter = 0;

            xbee.Api.DiscoverNodes(nodeInfo =>
            {
                nodeCounter++;
                PrintNode(nodeCounter, nodeInfo);
            });
        }

        private void PrintNode(int nodeNumber, NodeInfo nodeInfo)
        {
            Debug.Print("#" + nodeNumber + " - " + nodeInfo);
            lED7R.TurnLightOn(nodeNumber);
        }
    }
}
