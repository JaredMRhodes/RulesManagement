using System;
using System.Collections.Generic;

namespace RulesManagement.Validation
{
    /// <summary>
    /// The result of multiple validation rules
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// The validation state of all messages added
        /// </summary>
        private ValidationState _validationState;

        /// <summary>
        /// Collection of all validaiton messages
        /// </summary>
        private ICollection<ValidationMessage> _validationMessages;

        /// <summary>
        /// The state of all the validation states
        /// </summary>
        public ValidationState ValidationState
        {
            get
            {
                return _validationState;
            }
            private set
            {
                //check current validation state vs the validation being added. do not go up from failed validations
                if (_validationState == Validation.ValidationState.Passed && value != Validation.ValidationState.Passed)
                {
                    _validationState = value;
                }
                else if (_validationState == Validation.ValidationState.Failed && value != Validation.ValidationState.Passed && value != Validation.ValidationState.Failed)
                {
                    _validationState = value;
                }
                else
                {
                    _validationState = Validation.ValidationState.Error;
                }
            }
        }

        public ValidationResult()
        {
            _validationState = ValidationState.Passed;
            _validationMessages = new List<ValidationMessage>();
        }

        /// <summary>
        /// Add a validaiton message to this result collection
        /// </summary>
        /// <param name="message">The validation message</param>
        internal void Add(ValidationMessage message)
        {
            ValidationState = message.State;
            _validationMessages.Add(message);
        }

        /// <summary>
        /// Validation result messages
        /// </summary>
        public IEnumerable<ValidationMessage> ValidationMessages
        {
            get
            {
                return _validationMessages;
            }
        }

        /// <summary>
        /// Get Validaiton results of a paticular state
        /// </summary>
        /// <param name="state">The state of the validation message</param>
        /// <returns>A collection of validaton messages with the state passed in</returns>
        public IEnumerable<ValidationMessage> GetMessageOfType(Predicate<ValidationState> predicate)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(this._validationMessages.Count);
            foreach (ValidationMessage message in this._validationMessages)
            {
                if (predicate.Invoke(message.State))
                {
                    messages.Add(message);
                }
            }
            return messages;
        }

        public bool Passed
        {
            get
            {
                return this.ValidationState == Validation.ValidationState.Passed;
            }
        }
    }
}
