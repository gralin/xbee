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

        public static bool operator ==(NodeInfo n1, object n2)
        {
            if (ReferenceEquals(null, n1) && ReferenceEquals(null, n2))
                return true;

            return !ReferenceEquals(null, n1) && n1.Equals(n2);
        }

        public static bool operator !=(NodeInfo n1, object n2)
        {
            return !(n1 == n2);
        }

        public static bool operator ==(NodeInfo n1, NodeInfo n2)
        {
            if (ReferenceEquals(null, n1) && ReferenceEquals(null, n2))
                return true;

            return !ReferenceEquals(null, n1) && n1.Equals(n2);
        }

        public static bool operator !=(NodeInfo n1, NodeInfo n2)
        {
            return !(n1 == n2);
        }

        public bool Equals(NodeInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.NetworkAddress, NetworkAddress) 
                && Equals(other.SerialNumber, SerialNumber);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (NodeInfo) 
                && Equals((NodeInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (NetworkAddress.GetHashCode() * 397) ^ SerialNumber.GetHashCode();
            }
        }
    }
}