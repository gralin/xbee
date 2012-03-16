using System;
using System.Threading;
using Gadgeteer.Interfaces;

namespace Gadgeteer.Modules.GHIElectronics
{
    /// <summary>
    /// A XBee module for Microsoft .NET Gadgeteer
    /// </summary>
    public class XBee : Module
    {
        // It seems that the module is ready to work after aprox. 100 ms after the reset
        private const int StartupTime = 100;

        private Api.XBee _api;
        private Serial _serialLine;
        private readonly Socket _socket;
        private readonly DigitalOutput _resetPin;
        private readonly DigitalOutput _sleepPin;
        private readonly Timer _resetTimer;
        private readonly ManualResetEvent _ready;

        private static class ResetState
        {
            public const bool NotRunning = false;
            public const bool Running = true;
        }

        private static class SleepState
        {
            public const bool Awaken = false;
            public const bool Sleeping = true;
        }

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

            _resetPin = new DigitalOutput(_socket, Socket.Pin.Three, ResetState.NotRunning, this);
            _sleepPin = new DigitalOutput(_socket, Socket.Pin.Eight, SleepState.Awaken, this);

            _resetTimer = new Timer(StartupTime, Timer.BehaviorType.RunOnce);
            _resetTimer.Tick += t => _ready.Set();

            _ready = new ManualResetEvent(false);
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

            Reset();

            if (!_ready.WaitOne(2 * StartupTime, false))
                throw new Exception("Module ready flag was not set in expected time!");
            
            _api.Open();
        }

        /// <summary>
        /// Perform module hardware reset.
        /// </summary>
        public void Reset()
        {
            // reset pulse must be at least 200 ns
            Disable();
            Enable();
        }

        /// <summary>
        /// Disables the module (power off).
        /// </summary>
        public void Disable()
        {
            if (_resetPin.Read() == ResetState.NotRunning) 
                return;

            _ready.Reset();
            _resetPin.Write(ResetState.NotRunning);
        }

        /// <summary>
        /// Enabled the module (power on).
        /// </summary>
        public void Enable()
        {
            if (_resetPin.Read() == ResetState.Running)
                return;

            _resetPin.Write(ResetState.Running);
            _resetTimer.Start();
        }

        /// <summary>
        /// Sets the sleep control pin to active state (sleep request).
        /// </summary>
        public void Sleep()
        {
            _sleepPin.Write(SleepState.Sleeping);
        }

        /// <summary>
        /// Sets the sleep control pin to inactive state (no sleep request)
        /// </summary>
        public void Awake()
        {
            _sleepPin.Write(SleepState.Awaken);
        }
    }
}
