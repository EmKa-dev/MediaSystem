using System;
using System.Linq;
using TcpServerBaseLibrary.Interface;

namespace MediaSystem.MediaServer
{
    public class ConsoleLogger : ILogger
    {

        ConsoleColor _OriginalColor = Console.ForegroundColor;

        ConsoleColor _DebugColor = ConsoleColor.Yellow;

        ConsoleColor _ErrorColor = ConsoleColor.Red;

        LogLevel[] _Filters;

        /// <summary>
        /// Initiates a new ConsoleLogger, displays all loglevels if no filter provided.
        /// </summary>
        /// <param name="logLevels">Which loglevels to display</param>
        public ConsoleLogger(LogLevel[] logLevels = null)
        {

            if (logLevels == null)
            {
                _Filters = new LogLevel[] { LogLevel.Debug, LogLevel.Error, LogLevel.Info };
            }
            else
            {
                _Filters = logLevels;

            }
        }

        public void Debug(string message)
        {
            Console.ForegroundColor = _DebugColor;
            WriteToConsole(message, LogLevel.Debug);
        }

        public void Error(string message)
        {
            Console.ForegroundColor = _ErrorColor;
            WriteToConsole(message, LogLevel.Error);
        }

        public void Info(string message)
        {
            Console.ForegroundColor = _OriginalColor;
            WriteToConsole(message, LogLevel.Info);
        }


        private void WriteToConsole(string msg, LogLevel level)
        {

            if (_Filters.Contains(level))
            {
                Console.WriteLine(msg);
            }

            //Reset to original color in case we had changed it
            Console.ForegroundColor = _OriginalColor;
        }
    }

    public enum LogLevel
    {
        Debug,
        Error,
        Info
    }
}
