using MifareReaderApp.Stuff.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff.Port
{
    public class DataInterpreter
    {
        public static BaseResult GetCardNumber(string data)
        {
            var result = new BaseResult();

            var splitted = data.Split('[', ']').ElementAtOrDefault(1);
            if (splitted == null)
            {
                result.IsSuccess = false;
                result.Message = $"Не удалось разобрать команду \"{data}\"";
                return result;
            }

            result.Message = splitted;

            return result;
        }
    }
}
