using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Application.Exceptions
{
    public class RegistrationFailedException : Exception
    {
        public RegistrationFailedException(string message) : base(message) { }
    }
}