using System;

namespace RulesManagement.Exceptions
{
    public class RulesNotRunException : InvalidOperationException
    {
        public RulesNotRunException()
        {

        }

        public RulesNotRunException(string message)
            : base(message)
        {

        }
    }
}
