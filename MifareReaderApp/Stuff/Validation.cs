using MifareReaderApp.Stuff.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    public class Validation
    {
        public static ValidationResult<string> ValidateStrings(params (string PropName, string PropValue)[] strings)
        {
            var result = new ValidationResult<string>(new List<string>());

            foreach (var item in strings)
            {
                if (string.IsNullOrEmpty(item.PropValue))
                {
                    result.IsSuccess = false;
                    result.NotValidEntities.Add(item.PropName);
                }
            }

            return result;
        }
    }
}
