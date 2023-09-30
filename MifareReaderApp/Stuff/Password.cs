using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using MifareReaderApp.Stuff.Results;

namespace MifareReaderApp.Stuff
{
    public class Password
    {
        private const string S = "ppa";
        private const string A = "red";
        private const string L = "rea";
        private const string T = "far";

        private static string FileName = $"{Constants.Constants.MainFolderPath}\\slt.at";

        public static PasswordCheckResult HashAndWriteToFile(string password)
        {
            var result = new PasswordCheckResult();

            var validationResult = ValidatePassword(password);

            if (!validationResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Message = string.Join("\n", validationResult.Errors);
                return result;
            }

            try
            {
                var hash = Hash(password);

                WriteToFile(hash);
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = $"Во время установки пароля возникла ошибка\n{e.Message}";
            }

            return result;
        }

        public static string Hash(string password)
        {
            password += S + A + L + T;

            var bytes = Encoding.UTF8.GetBytes(password);

            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(bytes);

            return Convert.ToHexString(hash);
        }

        public static void WriteToFile(string hashedPassword)
        {
            File.WriteAllText(FileName, hashedPassword);
        }

        public static FileReadResult ReadFromFile()
        {
            var result = new FileReadResult();
            
            if (!File.Exists(FileName))
            {
                result.IsSuccess = false;
                result.Message = "Файл с паролем не найден";
                return result;
            }

            var content = File.ReadAllText(FileName);

            if (string.IsNullOrEmpty(content))
            {
                result.IsSuccess = false;
                result.Message = "Файл с паролем пуст";
                return result;
            }

            result.Message = content;

            return result;
        }

        public static PasswordCheckResult Check(string password)
        {
            var result = new PasswordCheckResult();

            var validationResult = ValidatePassword(password);

            if (!validationResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Message = string.Join("\n", validationResult.Errors);
                return result;
            }

            var readResult = ReadFromFile();

            if (!readResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Message = readResult.Message;
                return result;
            }

            var hashPassword = Hash(password);

            if (hashPassword == readResult.Message)
                return result;

            result.IsSuccess = false;
            result.Message = "Пароль не совпадает";

            return result;
        }

        private static PasswordValidationResult ValidatePassword(string password)
        {
            var passwordValidationResult = new PasswordValidationResult();

            if (string.IsNullOrEmpty(password))
            {
                passwordValidationResult.IsSuccess = false;
                passwordValidationResult.Errors.Add("Пароль не может быть пустым");
            }

            return passwordValidationResult;
        }

    }
}
