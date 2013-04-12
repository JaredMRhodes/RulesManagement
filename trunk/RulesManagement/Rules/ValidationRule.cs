using System;
using RulesManagement.Validation;

namespace RulesManagement.Rules
{
    public abstract class ValidationRule<T> : IValidationRule<T>
    {
        public abstract ValidationMessage RunValidaiton(T item);

        public bool RunRule(T item)
        {
            return this.RunValidaiton(item).State == ValidationState.Passed;
        }


        public string RuleName
        {
            get 
            {
                return this.GetType().Name;
            }
        }
    }
}
