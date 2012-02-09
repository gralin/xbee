namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// AT Command Queue
    /// API ID: 0x9
    /// </summary>
    public class AtCommandQueue : AtCommand
    {
        public AtCommandQueue(string command) 
            : this(command, null, DEFAULT_FRAME_ID)
        {
        }

        public AtCommandQueue(string command, int[] value, int frameId) 
            : base(command, value, frameId)
        {
        }

        public override ApiId ApiId
        {
            get { return ApiId.AT_COMMAND_QUEUE; }
        }
    }
}