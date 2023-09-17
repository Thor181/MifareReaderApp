using MifareReaderApp.Models.Stuff;
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

        private string _portName = "COM1";
        public string PortName
        {
            get
            {
                return _portName;
            }
            set
            {
                _portName = value;
                Save();
            }
        }

        public string LogsPath { get; set; } = "Logs\\";

        private DbConnectionString _dbConnectionString = new();
        public DbConnectionString DbConnectionString
        {
            get
            {
                return _dbConnectionString;
            }
            set
            {
                _dbConnectionString = value;
                Save();
            }
        }

        [JsonIgnore]
        public string ConnectionString { get => DbConnectionString.ToString(); }

        [JsonIgnore]
        public bool AdminMode { get; set; }

        [JsonConstructor]
        public AppConfig()
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
