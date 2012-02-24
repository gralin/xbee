namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    /// <summary>
    /// AT Command Queue
    /// API ID: 0x9
    /// </summary>
    public class AtCommandQueue : AtCommand
    {
        public AtCommandQueue(AtCmd command)
            : this(command, null, PacketIdGenerator.DefaultId)
        {
        }

        public AtCommandQueue(AtCmd command, int[] value, int frameId) 
            : base(command, value, frameId)
        {
        }

        public override ApiId ApiId
        {
            get { return ApiId.AT_COMMAND_QUEUE; }
        }
    }
}