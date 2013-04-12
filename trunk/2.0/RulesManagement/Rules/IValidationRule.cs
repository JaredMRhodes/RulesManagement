using System;
using RulesManagement.Validation;

namespace RulesManagement.Rules
{
    public interface IValidationRule<T>
    {
        ValidationMessage RunValidaiton(T item);

        bool RunRule(T item);

        string RuleName { get; }
    }
}
