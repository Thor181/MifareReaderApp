using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OverrideVisibleAttribute : Attribute
    {
        public bool Visible { get; }

        public OverrideVisibleAttribute(bool visible)
        {
            Visible = visible;
        }
    }
}
