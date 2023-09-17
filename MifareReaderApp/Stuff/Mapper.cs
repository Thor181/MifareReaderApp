using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    public class Mapper
    {
        public static TTarget Map<TTarget, TSource>(TSource source) where TTarget : class 
                                                                    where TSource : class
        {
            var targetProps = MetadataInfo<TSource>.GetProperties();
            throw new NotImplementedException();
        }
    }
}
