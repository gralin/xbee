namespace NETMF.OpenSource.XBee.Api
{
    /// <summary>
    ///  TODO: Update comments
    ///     
    /// </summary>
    /// <remarks>
    ///     
    /// </remarks>
    public class DataDelegateRequest : DataRequest
    {
        /// <summary>
        ///  TODO: Update Comments
        ///     
        /// </summary>
        private XBee.PayloadDelegate _bytesDelegate;

        /// <summary>
        ///   TODO: Update Comments
        ///     
        /// </summary>
        /// <value>
        ///     <para>
        ///         
        ///     </para>
        /// </value>
        /// <remarks>
        ///     
        /// </remarks>
        public override byte[] Payload
        {
            get { return _bytesDelegate.Invoke(); }
        }

        /// <summary>
        ///   TODO: Update Comments
        ///     
        /// </summary>
        /// <param name="xbee" type="NETMF.OpenSource.XBee.Api.XBee">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        public DataDelegateRequest(XBee xbee) 
            : base(xbee)
        {
        }

        /// <summary>
        ///   TODO: Update Comments
        ///     
        /// </summary>
        /// <param name="payloadDelegate" type="NETMF.OpenSource.XBee.Api.XBee.PayloadDelegate">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        internal void Init(XBee.PayloadDelegate payloadDelegate)
        {
            Init();
            _bytesDelegate = payloadDelegate;
        }
    }
}