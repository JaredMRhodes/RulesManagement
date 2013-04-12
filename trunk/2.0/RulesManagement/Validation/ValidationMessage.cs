using System;

namespace RulesManagement.Validation
{
    public class ValidationMessage
    {
        public ValidationMessage(ValidationState state)
        {
            this.State = state;
            this.Message = String.Empty;
        }

        public ValidationMessage(ValidationState state,string message)
        {
            this.State = state;
            this.Message = message;
        }

        public string Message { get; private set; }
        public ValidationState State { get; private set; }
    }
}
