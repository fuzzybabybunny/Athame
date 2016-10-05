using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.CommonModel
{
    public class InvalidSessionException : Exception
    {
        public InvalidSessionException(string message) : base(message)
        {
            
        }
    }
}
