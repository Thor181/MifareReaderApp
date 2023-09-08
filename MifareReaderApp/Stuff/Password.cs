using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MifareReaderApp.Stuff
{
    public class Password
    {
        private const string S = "ppa";
        private const string A = "red";
        private const string L = "rea";
        private const string T = "far";

        private const string FileName = "slt.at";

        public static string Hash(string password)
        {
            password += S + A + L + T;

            var bytes = Encoding.UTF8.GetBytes(password);

            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(bytes);

            return Encoding.UTF8.GetString(hash);
        }

        public static void WriteToFile(string password)
        {
            File.WriteAllText(FileName, password);
        }

        public static string ReadFromFile()
        {
            return File.ReadAllText(FileName);
        }

        public static PasswordCheckResult Check(string password)
        {
            var result = new PasswordCheckResult();

            if (string.IsNullOrEmpty(password))
            {
                result.IsSuccess = false;
                result.Message = "Пароль не может быть пустым";
                return result;
            }

            var filePassword = ReadFromFile();
            if (string.IsNullOrEmpty(filePassword))
            {
                result.IsSuccess = false;
                result.Message = "В файле не обнаружен пароль";
                return result;
            }

            var hash = Hash(password);

            if (hash == password)
                return result;

            result.IsSuccess = false;
            result.Message = "Пароль не совпадает";

            return result;
        }

    }
}
