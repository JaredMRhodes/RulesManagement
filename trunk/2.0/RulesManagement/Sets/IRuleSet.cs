using System;
using System.Collections.Generic;
using RulesManagement.Rules;

namespace RulesManagement.Sets
{
    public interface IRuleSet : IBaseRuleSet, IDisposable
    {
        void AddRule<T>(IRule<T> rule);
        IRuleSet RunRule<T>(T item);
        IRuleSet RunRule(params object[] items);
    }
}
