using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using MifareReaderApp.Stuff.Constants;
using NPOI.POIFS.FileSystem;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    public class MetadataInfo<T> where T : class
    {
        public static PropertyInfo[]? GetProperties(T instance = null)
        {
            PropertyInfo[]? props = null;

            if (instance == null)
                props = typeof(T).GetProperties();
            else
                props = instance.GetType().GetProperties();

            return props;
        }

        public static string GetPropertyLocalizedName(Type type, string propertyName)
        {
            var props = type.GetProperties();
            var localizedname = GetLocalizedNameInternal(props, propertyName);

            return localizedname;
        }

        public static string GetPropertyLocalizedName(T instance, string propertyName)
        {
            if (instance == null)
                return propertyName;

            var props = instance.GetType().GetProperties();
            var localizedName = GetLocalizedNameInternal(props, propertyName);

            return localizedName;
        }

        public static string GetPropertyLocalizedName(string propertyName)
        {
            var props = typeof(T).GetProperties();
            var localizedName = GetLocalizedNameInternal(props, propertyName);

            return localizedName;
        }

        public static string GetPropertyLocalizedName(Expression<Func<T, string>> func)
        {
            var memberName = ((MemberExpression)func.Body).Member.Name;
            return GetPropertyLocalizedName(memberName);
        }

        private static string GetLocalizedNameInternal(PropertyInfo[] propertyInfos, string propertyName)
        {
            var property = propertyInfos.SingleOrDefault(x => propertyName == x.Name);

            if (property == null)
                return propertyName;

            var attr = property.GetCustomAttributes<LocalizedNameAttribute>().FirstOrDefault();

            if (attr == null)
                return propertyName;

            var localizedName = attr.LocalizedName;

            return localizedName;
        }

        public static List<string> GetPropertiesValues(T instance)
        {
            var type = instance.GetType();
            var props = GetProperties(instance).Where(x => !PropertyIsVirtual(type, x.Name) || PropertyIsDateTime(x.PropertyType)).ToList();
            if (props == null)
                return null;

            var values = new List<string>();

            for (int i = 0; i < props.Count; i++)
                values.Add(props[i].GetValue(instance)?.ToString() ?? string.Empty);

            return values;
        }

        public static bool PropertyIsVirtual(Type type, string propertyName)
        {
            var isVirtual = type.GetProperty(propertyName).GetGetMethod().IsVirtual;

            return isVirtual;
        }

        public static bool PropertyIsDateTime(Type type)
        {
            var propertyType = type.IsGenericType ? type.GenericTypeArguments.First() : type;
            var isDateTime = propertyType == typeof(DateTime);

            return isDateTime;
        }

        public static bool IsVisibleByOverride(Type type, string propertyName)
        {
            var attribute = type.GetProperty(propertyName)?.GetCustomAttribute<OverrideVisibleAttribute>();

            if (attribute == null)
                return false;

            return attribute.Visible;
        }

        
    }
}
