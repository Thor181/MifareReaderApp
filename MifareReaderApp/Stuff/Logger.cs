using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    public class Logger
    {
        private static Logger _instance;
        public static Logger Instance => _instance ??= new Logger();

        private string _logsPath = AppConfig.Instance.LogsPath;
        private string _extension = ".log";

        private string _errorLevel = "Error";

        private Logger()
        {
            CheckDirectoryExist();
        }

        public void LogError(string message, Exception e)
        {
            var formattedMessage = $"[{DateTime.Now}] ({_errorLevel}) | {message} | {e.Message}";
            AppendLines(formattedMessage);
        }

        private void AppendLines(params string[] formattedMessage)
        {
            var task = Task.Run(async () =>
            {
                var fileName = GetFileName();
                await File.AppendAllLinesAsync(fileName, formattedMessage);
            });

            task.Wait();
        }

        private string GetFileName()
        {
            var path = Path.Combine(_logsPath, DateTime.Today.ToShortDateString().ToString());
            var fileName = $"{path}{_extension}";

            return fileName;
        }

        private void CheckDirectoryExist()
        {
            var fileName = GetFileName();
            var directory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory!);
        }
    }
}
