using System;
using System.Collections.Generic;
using RulesManagement.Rules;
using RulesManagement.Validation;

namespace RulesManagement.Sets
{
    public interface IValidationSet : IBaseRuleSet, IDisposable
    {
        void AddValidationRule<T>(IValidationRule<T> rule);
        IValidationSet RunValidationRules<T>(T item);
        IValidationSet RunValidaitonRules(params object[] items);
        ValidationResult ValidationResult { get; }
    }
}
