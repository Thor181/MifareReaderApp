using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    public class AppConfig
    {
        private static AppConfig _instance;
        public static AppConfig Instance => _instance ??= new AppConfig();

        [JsonIgnore]
        public bool PasswordSetted => Password.ReadFromFile().IsSuccess;

        [JsonIgnore]
        public bool DatabaseInitialized { get; set; } = false;

        private string _configPath = "Config\\Config.json";

        public string PortName { get; set; } = "COM1";
        public string LogsPath { get; set; } = "Logs\\";
        public string ConnectionString { get; set; } = "Server=localhost;Database=MfRADb;Trusted_Connection=True;Encrypt=false";

        [JsonIgnore]
        public bool AdminMode { get; set; }

        [JsonConstructor]
        public  AppConfig()
        {
        }

        public void Initialize()
        {
            CheckDirectoryExist();
            CheckFileExist();

            Load();
        }

        private void CheckDirectoryExist()
        {
            var fileName = _configPath;
            var directory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory!);
        }

        private void CheckFileExist()
        {
            var isExists = File.Exists(_configPath);
            if (!isExists)
                Save();
        }

        public void Save()
        {
            var serialized = JsonSerializer.Serialize(Instance);
            File.WriteAllText(_configPath, serialized);
        }

        public void Load()
        {
            var json = File.ReadAllText(_configPath);
            var deserialized = JsonSerializer.Deserialize<AppConfig>(json);
            _instance = deserialized;
        }

    }
}
