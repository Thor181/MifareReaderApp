using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LocalizedNameAttribute : Attribute
    {
        public string LocalizedName { get; set; }

        public LocalizedNameAttribute(string name)
        {
            LocalizedName = name;
        }
    }
}
