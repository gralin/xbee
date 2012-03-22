namespace NETMF.OpenSource.XBee.Api
{
    public class NodeInfo
    {
        public XBeeAddress16 NetworkAddress { get; set; }
        public XBeeAddress64 SerialNumber { get; set; }
        public string NodeIdentifier { get; set; }

        public NodeInfo()
            : this(XBeeAddress64.Broadcast, XBeeAddress16.Unknown)
        {
        }

        public NodeInfo(XBeeAddress64 serialNumber, XBeeAddress16 networkAddress)
        {
            NetworkAddress = networkAddress;
            SerialNumber = serialNumber;
            NodeIdentifier = string.Empty;
        }

        public override string ToString()
        {
            return "S/N=" + SerialNumber
                   + ", address=" + NetworkAddress
                   + ", id='" + NodeIdentifier + "'";
        }
    }
}