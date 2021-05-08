using Shared.Helpers;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Services {

    public class FileLoggingService : ILoggingService {
        private readonly SemaphoreSlim SlowStuffSemaphore = new SemaphoreSlim(1, 1);
        private string _logDirectory { get; }
        private string _logFile => GenericHelpers.Combine(_logDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt");

        public FileLoggingService() {
            _logDirectory = GenericHelpers.Combine(AppContext.BaseDirectory, "file_logs");
        }

        public void Write(string type, string message) {
            WriteToLog($"{DateTime.UtcNow.ToString("hh:mm:ss")} [{type}]\n{message}");
        }

        public void WriteError(string message) {
            WriteToLog($"{DateTime.UtcNow.ToString("hh:mm:ss")} [ERROR]\n{message}");
        }

        public void WriteException(Exception ex) {
            var sb = new StringBuilder();
            sb.AppendLine($"{DateTime.UtcNow.ToString("hh:mm:ss")} [EXCEPTION]");

            while (ex != null) {
                sb.AppendLine(ex.GetType().FullName);
                sb.AppendLine("Message : " + ex.Message);
                sb.AppendLine("StackTrace : " + ex.StackTrace);

                ex = ex.InnerException;
            }

            WriteToLog(sb.ToString());
        }

        public void WriteException(string message, Exception ex) {
            var sb = new StringBuilder();
            sb.AppendLine($"{DateTime.UtcNow.ToString("hh:mm:ss")} [EXCEPTION]");
            sb.AppendLine(message);
            while (ex != null) {
                sb.AppendLine(ex.GetType().FullName);
                sb.AppendLine("Message : " + ex.Message);
                sb.AppendLine("StackTrace : " + ex.StackTrace);

                ex = ex.InnerException;
            }

            WriteToLog(sb.ToString());
        }

        public void WriteInfo(string message) {
            WriteToLog($"{DateTime.UtcNow.ToString("hh:mm:ss")} [INFO]\n{message}");
        }

        public void WriteWarning(string message) {
            WriteToLog($"{DateTime.UtcNow.ToString("hh:mm:ss")} [WARNING]\n{message}");
        }

        private void WriteToLog(string msg) {
            SlowStuffSemaphore.WaitAsync();
            try {
                Console.WriteLine(msg);
                if (!Directory.Exists(_logDirectory))     // Create the log directory if it doesn't exist
                    Directory.CreateDirectory(_logDirectory);
                if (!File.Exists(_logFile))               // Create today's log file if it doesn't exist
                    File.Create(_logFile).Dispose();

                File.AppendAllText(_logFile, msg + "\n");     // Write the log text to a file
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            finally {
                SlowStuffSemaphore.Release();
            }
        }
    }
}
