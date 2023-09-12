using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff.Results
{
    public class ValidationResult<T>
    {
        public bool IsSuccess { get; set; } = true;
        public IList<T> NotValidEntities { get; set; }

        public ValidationResult(IList<T> notValidEntitiesCollection)
        {
            NotValidEntities = notValidEntitiesCollection;
        }
    }
}
