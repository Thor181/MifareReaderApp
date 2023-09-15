using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Models.Stuff
{
    public class DbConnectionString
    {
        public string Server { get; set; } = "localhost";
        public int Port { get; set; } = 1433;
        public string Database { get; set; } = "MfRADb";
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool TrustedConnection { get; set; } = true;
        public bool Encrypt { get; set; } = false;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Server={Server};");
            sb.Append($"Database={Database};");

            if (TrustedConnection == false)
            {
                sb.Append($"User Id={User};");
                sb.Append($"Password={Password}");
            }
            else
                sb.Append($"Trusted_Connection={TrustedConnection};");

            sb.Append($"Encrypt={Encrypt}");

            return sb.ToString();

        }
    }
}
