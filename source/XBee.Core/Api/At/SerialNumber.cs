using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public static class SerialNumber
    {
        public static XBeeAddress64 Read(XBee xbee)
        {
            var sh = xbee.Send(AtCmd.SerialNumberHigh);
            var sl = xbee.Send(AtCmd.SerialNumberLow);
            return Parse(sl, sh);
        }

        public static XBeeAddress64 Read(XBee sender, XBeeAddress remoteXbee)
        {
            var sh = sender.Send(AtCmd.SerialNumberHigh, remoteXbee);
            var sl = sender.Send(AtCmd.SerialNumberLow, remoteXbee);
            return Parse(sl, sh);
        }

        private static XBeeAddress64 Parse(AtResponse sl, AtResponse sh)
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