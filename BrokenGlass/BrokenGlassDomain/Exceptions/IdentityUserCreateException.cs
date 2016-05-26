using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokenGlassDomain.Exceptions
{
    class IdentityUserCreateException : Exception
    {
        public IdentityUserCreateException() : base()
        {
        
        }

        public IdentityUserCreateException(string message) : base(message)
        {

        }
        public IdentityUserCreateException(string message, Exception innerException) : base(message,innerException)
        {

        }
    }
}
