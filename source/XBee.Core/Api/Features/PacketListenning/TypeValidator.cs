using System;

namespace Gadgeteer.Modules.GHIElectronics.Api.Features.PacketListenning
{
    public class TypeValidator : IPacketValidator
    {
        private readonly Type _expectedType;

        public TypeValidator(Type expectedType)
        {
            if (expectedType == null)
                throw new ArgumentException("expectedType needs to be specified");

            _expectedType = expectedType;
        }

        public bool Validate(XBeeResponse packet)
        {
            return _expectedType.IsInstanceOfType(packet);
        }
    }
}