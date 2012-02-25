namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    /// <summary>
    /// AT Command Queue
    /// API ID: 0x9
    /// </summary>
    public class AtCommandQueue : AtCommand
    {
        public AtCommandQueue(AtCmd command, byte[] value = null) 
            : base(command, value)
        {
        }

        public override ApiId ApiId
        {
            get { return ApiId.AT_COMMAND_QUEUE; }
        }
    }
}