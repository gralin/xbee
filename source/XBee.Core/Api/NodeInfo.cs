namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class NodeInfo
    {
        public XBeeAddress16 NetworkAddress { get; set; }
        public XBeeAddress64 SerialNumber { get; set; }
        public string NodeIdentifier { get; set; }
    }
}