using System.Collections;

namespace Gadgeteer.Modules.GHIElectronics.Util
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
            if (messageLevel > LoggingLevel)
                return;

            Microsoft.SPOT.Debug.Print(LevelNames[messageLevel] + "\t" + message);
        }
    }
}