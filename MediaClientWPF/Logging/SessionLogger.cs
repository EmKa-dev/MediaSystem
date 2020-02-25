using System;

namespace MediaSystem.DesktopClientWPF
{
    public static class SessionLogger
    {
        public static event  Action<string> LoggerEvent;

        public static void LogEvent(string args)
        {
            LoggerEvent?.Invoke(args);
        }
    }
}
