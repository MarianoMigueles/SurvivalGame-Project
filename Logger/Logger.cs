using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.Log
{
    public class Logger
    {
        private readonly string _logFilePath;

        public Logger(string logFilePath)
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

        private void Log(string logLevel, string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}";

            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath + ".txt", true))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot write file log: {ex.Message}");
            }
        }

        private void CreateLogFile()
        {
            try
            {
                if (!File.Exists(_logFilePath))
                {
                    File.Create(_logFilePath + ".txt").Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"An error has occurred: {e.Message}");
            }
        }
    }
}
