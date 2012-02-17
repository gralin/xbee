using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public static class SerialNumber
    {
        public static XBeeAddress64 Read(XBee xbee)
        {
            var data = new OutputStream();
            var sh = xbee.Send(new AtCommand(AtCmd.SH));
            var sl = xbee.Send(new AtCommand(AtCmd.SL));

            if (!sh.IsOk || !sl.IsOk)
                throw new XBeeException("Failed to read serial number");

            data.Write(sh.Value);
            data.Write(sl.Value);

            return new XBeeAddress64(data.ToArray());
        }
    }
}