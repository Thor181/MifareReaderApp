using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff.Results
{
    public class PasswordValidationResult 
    {
        public bool IsSuccess { get; set; } = true;
        public List<string> Errors { get; set; } = new();
    }
}
