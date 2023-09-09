using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    public class AppConfig
    {
        private static AppConfig? _instance;

        public static AppConfig Instance
        {
            get => _instance ??= new AppConfig();
        }

        public bool PasswordSetted { get => Password.ReadFromFile().IsSuccess; }
        public string PortName { get; set; } = "COM2";

        private AppConfig()
        {
            
        }
        
    }
}
