using System;
using System.Threading;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    internal class AsyncSendResult : IAsyncResult
    {
        public XBeeRequest Request { get; protected set; }
        public IPacketListener ResponseListener { get; protected set; }

        public AsyncSendResult(XBeeRequest request, IPacketListener responseListener)
        {
            Request = request;
            ResponseListener = responseListener;
        }

        public bool IsCompleted
        {
            get { return ResponseListener.Finished; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { throw new NotSupportedException(); }
        }

        public object AsyncState
        {
            get { throw new NotSupportedException(); }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotSupportedException(); }
        }
    }
}