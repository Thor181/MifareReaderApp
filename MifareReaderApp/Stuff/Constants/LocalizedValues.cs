﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff.Constants
{
    public static class LocalizedValues
    {
        public const string Name = "Имя";
        public const string Name2 = "Отчество";
        public const string Surname = "Фамилия";
        public const string Card = "Номер карты";
        public static string Id1 => AppConfig.Instance.ID1Name;
        public static string Id2 => AppConfig.Instance.ID2Name;    
        public const string BeforeDate = "Дата";
        public const string BeforeTime = "Время";
        public const string Place = "Местонахождение";
        public const string Staff = "Сотрудник";

        public const string DbServer = "Сервер";
        public const string Database = "База";
        public const string User = "Пользователь";
        public const string Password = "Пароль";
        public const string TrustedConnection = "Trusted connection";
        public const string Encrypt = "Шифрование";

        public static IEnumerator<(string FieldName, string FieldValue)> GetEnumerator()
        {
            var fields = typeof(LocalizedValues).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach (var field in fields )
            {
                yield return (field.Name, field.GetValue(null) as string);
            }
        }

        public static string First(Predicate<string> predicate)
        {
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (predicate(current.FieldName))
                    return current.FieldValue;
            }

            throw new InvalidOperationException("Не обнаружено локализованное значение");
        }
    }
}
