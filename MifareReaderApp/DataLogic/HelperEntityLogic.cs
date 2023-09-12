using MifareReaderApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.DataLogic
{
    public class HelperEntityLogic<T> : BaseLogic where T : class, IHelperEntity
    {
        public T? Get(string name)
        {
            var entity = DbContext.Set<T>().SingleOrDefault(x => x.Name == name);

            return entity;
        }

        public T? First()
        {
            var entity = DbContext.Set<T>().FirstOrDefault();

            return entity;
        }
    }
}
