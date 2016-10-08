using System;

namespace Athame.CommonModel
{
    public class InvalidSessionException : Exception
    {
        public InvalidSessionException(string message) : base(message)
        {
            
        }
    }
}
