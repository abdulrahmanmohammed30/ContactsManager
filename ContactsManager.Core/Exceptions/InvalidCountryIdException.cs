using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class InvalidCountryIdException: ArgumentException
    {
        private const string _message = "Country Id was Invalid";
        public InvalidCountryIdException(): base(_message) { }

        public InvalidCountryIdException(string message): base(message) { }

        public InvalidCountryIdException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
