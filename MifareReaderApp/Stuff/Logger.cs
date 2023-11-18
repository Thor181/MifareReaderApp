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

        public void LogError(string message, Exception e = null)
        {
            var formattedMessage = $"[{DateTime.Now}] ({_errorLevel}) | {message}";

            if (e != null)
                formattedMessage += $" | {e.Message} | {e.InnerException}";

            AppendLines(formattedMessage);
        }

        private void AppendLines(params string[] formattedMessage)
        {
            Task.Run(async () =>
            {
                await Retry.Do(() =>
                 {
                     var fileName = GetFileName();
                     File.AppendAllLines(fileName, formattedMessage);
                     return true;
                 }, null, TimeSpan.FromSeconds(5), 10, "");
            });
            //var task = Task.Run(async () =>
            //{

            //});

            //task.Wait();
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
