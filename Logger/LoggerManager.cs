using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Manages logging of messages with different log levels (INFO, WARNING, ERROR).
/// Logs messages to a specified log file with timestamp.
/// </summary>
namespace Logger
{
    public class LoggerManager
    {
        private readonly string _logFilePath;
        private readonly string _logFileName = @"\\Output_log.txt";

        public LoggerManager(string logFilePath)
        {
            _logFilePath = logFilePath;
            CreateLogFile();
        }

        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        public void LogError(string message)
        {
            Log("ERROR", message);
        }

        /// <summary>
        /// Writes a log message with a specified log level.
        /// </summary>
        /// <param name="logLevel">The log level (e.g., INFO, WARNING, ERROR).</param>
        /// <param name="message">The message to log.</param>
        private void Log(string logLevel, string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}";

            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath + _logFileName, true))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot write file log: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates the log file if it does not already exist.
        /// </summary>
        private void CreateLogFile()
        {
            try
            {
                if (!File.Exists(_logFilePath + _logFileName))
                {
                    File.CreateText(_logFilePath + _logFileName);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"An error has occurred: {e.Message}");
            }
        }
    }
}
