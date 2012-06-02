using System.Collections;

namespace NETMF.OpenSource.XBee.Util
{
    public enum LogLevel
    {
        Off,
        Fatal,
        Error,
        Warn,
        Info,
        Debug,
        LowDebug,
        All
    }

    public static class Logger
    {
        public static LogLevel LoggingLevel { get; set; }

        private static readonly Hashtable LevelNames;
        private static readonly Hashtable LogWritters;

        static Logger()
        {
            LoggingLevel = LogLevel.Info;

            LevelNames = new Hashtable
            {
                {LogLevel.Fatal, "Fatal"},
                {LogLevel.Error, "Error"},
                {LogLevel.Warn, "Warn"},
                {LogLevel.Info, "Info"},
                {LogLevel.Debug, "Debug"},
                {LogLevel.LowDebug, "LowDebug"},
            };

            LogWritters = new Hashtable
            {
                {LogLevel.Fatal, null},
                {LogLevel.Error, null},
                {LogLevel.Warn, null},
                {LogLevel.Info, null},
                {LogLevel.Debug, null},
                {LogLevel.LowDebug, null},
            };
        }

        public static void Initialize(LogWriteDelegate logWritter, params LogLevel[] logLevel)
        {
            if (logLevel.Length == 0 || logLevel[0] == LogLevel.All)
            {
                foreach (var level in LogWritters.Keys)
                    LogWritters[level] = logWritter;
            }
            else
            {
                foreach (var level in logLevel)
                    LogWritters[level] = logWritter;
            }
        }

        public static bool IsActive(LogLevel level)
        {
            return LessOrEqual(level, LoggingLevel);
        }

        public static bool LessOrEqual(LogLevel level, LogLevel from)
        {
            return level <= from;
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
            var logWritter = LogWritters[messageLevel] as LogWriteDelegate;

            if (IsActive(messageLevel) && logWritter != null)
                logWritter.Invoke(LevelNames[messageLevel] + "\t" + message);
        }
    }

    public delegate void LogWriteDelegate(string message);
}