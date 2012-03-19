namespace NETMF.OpenSource.XBee.Api
{
    public interface IResponseFilter
    {
        bool Accept(XBeeResponse response);
    }
}