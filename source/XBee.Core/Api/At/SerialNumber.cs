using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public static class SerialNumber
    {
        public static XBeeAddress64 Read(XBee xbee)
        {
            var sh = xbee.Send(new AtCommand(AtCmd.SH));
            var sl = xbee.Send(new AtCommand(AtCmd.SL));
            return Parse(sl, sh);
        }

        public static XBeeAddress64 Read(XBee sender, XBeeAddress16 remoteXbee)
        {
            var sh = sender.Send(new RemoteAtCommand(remoteXbee, AtCmd.SH));
            var sl = sender.Send(new RemoteAtCommand(remoteXbee, AtCmd.SL));
            return Parse(sl, sh);
        }

        private static XBeeAddress64 Parse(AtCommandResponse sl, AtCommandResponse sh)
        {
            if (!sh.IsOk || !sl.IsOk)
                throw new XBeeException("Failed to read serial number");

            var data = new OutputStream();
            data.Write(sh.Value);
            data.Write(sl.Value);

            return new XBeeAddress64(data.ToArray());
        }
    }
}