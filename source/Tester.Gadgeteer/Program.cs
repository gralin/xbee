using Microsoft.SPOT;

namespace Gadgeteer.Tester
{
    public partial class Program
    {
        private Timer _timer;

        void ProgramStarted()
        {
            InitializeXBee();

            _timer = new Timer(10000, Timer.BehaviorType.RunOnce);
            _timer.Tick += DiscoverNodes;
            _timer.Start();
        }

        private void InitializeXBee()
        {
            xbee.Configure();
            Debug.Print(xbee.Api.Config.ToString());
        }

        private void DiscoverNodes(Timer timer)
        {
            var nodeCounter = 0;

            xbee.Api.DiscoverNodes(nodeInfo =>
            {
                nodeCounter++;
                Debug.Print("#" + nodeCounter + " - " + nodeInfo);
                lED7R.TurnLightOn(nodeCounter);
            });
        }
    }
}
