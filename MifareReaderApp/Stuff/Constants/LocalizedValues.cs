using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public const string Id1 = "ID1";
        public const string Id2 = "ID2";
        public const string BeforeDate = "Дата";
        public const string BeforeTime = "Время";
        public const string Place = "Местонахождение";
        public const string Staff = "Сотрудник";

        public const string DbServer = "Сервер";

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
