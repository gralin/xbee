namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    public class TxRequestBase : XBeeRequest, IWpanPacket
    {
        public override ApiId ApiId
        {
            get { throw new System.NotImplementedException(); }
        }

        public override int[] GetFrameData()
        {
            throw new System.NotImplementedException();
        }
    }
}