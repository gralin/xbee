using System.Collections;

namespace NETMF.OpenSource.XBee.Util
{
    /// <summary>
    ///   TODO: Update Comments
    ///     
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        ///  TODO: Update Comments
        ///     
        /// </summary>
        Off,

        /// <summary>
        ///  TODO: Update Comments
        ///     
        /// </summary>
        Fatal,

        /// <summary>
        ///  TODO: Update Comments
        ///     
        /// </summary>
        Error,

        /// <summary>
        ///  TODO: Update Comments
        ///     
        /// </summary>
        Warn,

        /// <summary>
        ///  TODO: Update Comments
        ///     
        /// </summary>
        Info,

        /// <summary>
        ///  TODO: Update Comments
        ///     
        /// </summary>
        Debug,

        /// <summary>
        ///  TODO: Update Comments
        ///     
        /// </summary>
        LowDebug,

        /// <summary>
        ///  TODO: Update Comments
        ///     
        /// </summary>
        All
    }

    public static class Logger
    {
        public static LogLevel LoggingLevel { get; set; }

        private static readonly Hashtable LevelNames;
        private static LogWriteDelegate _logWrite;

        static Logger()
        {
            LevelNames = new Hashtable
            {
                {LogLevel.Fatal, "Fatal"},
                {LogLevel.Error, "Error"},
                {LogLevel.Warn, "Warn"},
                {LogLevel.Info, "Info"},
                {LogLevel.Debug, "Debug"},
                {LogLevel.LowDebug, "LowDebug"},
            };
        }

        public static void Initialize(LogWriteDelegate logWrite, LogLevel logLevel)
        {
            _logWrite = logWrite;
            LoggingLevel = logLevel;
        }

        public static bool IsActive(LogLevel level)
        {
            return level <= LoggingLevel;
        }

        public static void Fatal(string message)
        {
            Log(message, LogLevel.Fatal);
        }

        public static void Error(string message)
        {
            Log(message, LogLevel.Error);
        }

        public static void Warn(string message)
        {
            Log(message, LogLevel.Warn);
        }

        public static void Info(string message)
        {
            Log(message, LogLevel.Info);
        }

        public static void Debug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public static void LowDebug(string message)
        {
            Log(message, LogLevel.LowDebug);
        }

        private static void Log(string message, LogLevel messageLevel)
        {
            if (IsActive(messageLevel) && _logWrite != null)
                _logWrite.Invoke(LevelNames[messageLevel] + "\t" + message);
        }
    }

    public delegate void LogWriteDelegate(string message);
}