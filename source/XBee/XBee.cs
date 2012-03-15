using System;
using Gadgeteer.Interfaces;

namespace Gadgeteer.Modules.GHIElectronics
{
    /// <summary>
    /// A XBee module for Microsoft .NET Gadgeteer
    /// </summary>
    public class XBee : Module
    {
        private Api.XBee _api;
        private Serial _serialLine;
        private readonly Socket _socket;
        private readonly DigitalOutput _resetPin;

        /// <summary>
        /// Gets the <see cref="GHIElectronics.Api.XBee"/> of the connected XBee module.
        /// </summary>
        public Api.XBee Api
        {
            get
            {
                if (_api == null)
                    Configure();

                return _api;
            }
        }

        /// <summary
        /// Gets the <see cref="Serial"/> device associated with this instance.
        /// </summary>
        public Serial SerialLine
        {
            get
            {
                if (_serialLine == null)
                    Configure();

                return _serialLine;
            }
        }

        /// <summary>
        /// Creates an instance of Gadgeteer XBee module driver.
        /// </summary>
        /// <param name="socketNumber">The socket that this module is plugged in to.</param>
        /// <remarks>
        /// The function <see cref="Configure"/> can be called to configure the <see cref="SerialLine"/> before it is used.
        /// If it is not called before first use, then the following defaults will be used and cannot be changed afterwards:
        /// <list type="bullet">
        ///  <item>Baud Rate - 9600</item>
        ///  <item>Parity - <see cref="Serial.SerialParity">SerialParity.None</see></item>
        ///  <item>Stop Bits - <see cref="Serial.SerialStopBits">SerialStopBits.One</see></item>
        ///  <item>Data Bits - 8</item>
        /// </list>
        /// </remarks>
        public XBee(int socketNumber)
        {
            // This finds the Socket instance from the user-specified socket number.  
            // This will generate user-friendly error messages if the socket is invalid.
            // If there is more than one socket on this module, then instead of "null" for the last parameter, 
            // put text that identifies the socket to the user (e.g. "S" if there is a socket type S)
            _socket = Socket.GetSocket(socketNumber, true, this, null);
            
            _socket.EnsureTypeIsSupported(new[] { 'K', 'U' }, this);

            _resetPin = new DigitalOutput(_socket, Socket.Pin.Three, false, this);
        }

        /// <summary>
        /// Configures this serial line.
        /// </summary>
        /// <remarks>
        /// This should be called at most once.
        /// </remarks>
        /// <param name="baudRate">The baud rate.</param>
        /// <param name="parity">A value from the <see cref="Serial.SerialParity"/> enumeration that specifies the parity.</param>
        /// <param name="stopBits">A value from the <see cref="Serial.SerialStopBits"/> enumeration that specifies the number of stop bits.</param>
        /// <param name="dataBits">The number of data bits.</param>
        public void Configure(int baudRate = 9600, Serial.SerialParity parity = Serial.SerialParity.None, Serial.SerialStopBits stopBits = Serial.SerialStopBits.One, int dataBits = 8)
        {
            if (SerialLine != null)
                throw new Exception("XBee.Configure can only be called once");

            // TODO: check if HW flow control should be used
            _serialLine = new Serial(_socket, baudRate, parity, stopBits, dataBits, Serial.HardwareFlowControl.NotRequired, this);

            _api = new Api.XBee(new XBeeConnection(_serialLine));
            _api.Open();
        }

        /// <summary>
        /// Perform module hardware reset.
        /// </summary>
        public void Reset()
        {
            _resetPin.Write(true);
            _resetPin.Write(false);
        }
    }
}
