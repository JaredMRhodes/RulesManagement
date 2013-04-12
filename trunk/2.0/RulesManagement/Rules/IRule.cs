using System;

namespace RulesManagement.Rules
{
    public interface IRule<T>
    {
        bool RunRule(T item);

        string RuleName { get; }
    }
}
