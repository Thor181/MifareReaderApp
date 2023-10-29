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

        private string _configPath = $"{Constants.Constants.MainFolderPath}\\Config\\Config.json";

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

        private int _baudRate = 9600;
        public int BaudRate
        {
            get
            {
                return _baudRate;
            }
            set
            {
                _baudRate = value;
                Save();
            }
        }

        [JsonIgnore]
        public string LogsPath => $"{Constants.Constants.MainFolderPath}\\Logs\\";

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
            if (!Constants.Constants.OnDeserialization)
            {
                var serialized = JsonSerializer.Serialize(Instance);
                File.WriteAllText(_configPath, serialized);
            }
        }

        public void Load()
        {
            try
            {
                Constants.Constants.OnDeserialization = true;
                var json = File.ReadAllText(_configPath);
                var deserialized = JsonSerializer.Deserialize<AppConfig>(json);
                _instance = deserialized;
            }
            catch (Exception e)
            {
                Logger.Instance.LogError("При загрузке конфига возникла ошибка", e);
                throw;
            }
            finally
            {
                Constants.Constants.OnDeserialization = false;
            }
        }

    }
}
