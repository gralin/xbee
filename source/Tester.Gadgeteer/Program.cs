using Microsoft.SPOT;
using NETMF.OpenSource.XBee.Api;
using Gadgeteer.Modules.GHIElectronics;

namespace Gadgeteer.Tester
{
    public partial class Program
    {
        void ProgramStarted()
        {
            coordinator.Configure();

            router.Configure();            
            router.Api.StatusChanged += (x, s) => OnStatusChanged(s);

            endDevice.Configure();

            Debug.Print(coordinator.Api.Config.ToString());
            Debug.Print(router.Api.Config.ToString());
            Debug.Print(endDevice.Api.Config.ToString());

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

            router.Api.DiscoverNodes(nodeInfo =>
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
