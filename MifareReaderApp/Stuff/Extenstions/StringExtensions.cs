using MifareReaderApp.Stuff.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff.Extenstions
{
    public static class StringExtensions
    {
        public static (string PropName, string PropValue) GetLocalizedPropInfo<T>(this string value, Expression<Func<T, string>> propertySelector)
        {
            var propName = ((MemberExpression)propertySelector.Body).Member.Name;
            return (LocalizedValues.First(x => x == propName), value);
        }
    }
}
