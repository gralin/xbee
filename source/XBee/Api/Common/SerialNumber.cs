﻿using NETMF.OpenSource.XBee.Api.At;
using NETMF.OpenSource.XBee.Util;

namespace NETMF.OpenSource.XBee.Api.Common
{
    public static class SerialNumber
    {
        public static XBeeAddress64 Read(XBee xbee)
        {
            var sh = xbee.Send2(AtCmd.SerialNumberHigh).GetResponse();
            var sl = xbee.Send2(AtCmd.SerialNumberLow).GetResponse();
            return Parse(sl, sh);
        }

        public static XBeeAddress64 Read(XBee sender, XBeeAddress remoteXbee)
        {
            var sh = sender.Send2(AtCmd.SerialNumberHigh).To(remoteXbee).GetResponse();
            var sl = sender.Send2(AtCmd.SerialNumberLow).To(remoteXbee).GetResponse();
            return Parse((AtResponse)sl, (AtResponse)sh);
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