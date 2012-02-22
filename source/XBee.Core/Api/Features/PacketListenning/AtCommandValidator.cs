using Gadgeteer.Modules.GHIElectronics.Api.At;

namespace Gadgeteer.Modules.GHIElectronics.Api.Features.PacketListenning
{
    public class AtCommandValidator : TypeValidator
    {
        private readonly AtCmd _atCmd;

        public AtCommandValidator(AtCmd expectedAtCmd)
            : base(typeof(AtResponse))
        {
            _atCmd = expectedAtCmd;
        }

        public new bool Validate(XBeeResponse packet)
        {
            if (!base.Validate(packet))
                return false;

            return ((AtResponse)packet).Command == _atCmd;
        }
    }
}