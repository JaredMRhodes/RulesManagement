using System;
using System.Collections.Generic;
using System.Text;

namespace RulesManagement.Sets
{
    public interface IBaseRuleSet
    {
        bool RuleResult { get; }
        void PrecacheType(Type type);
        void PrecacheType<T>();
        RunRulesOnChildren RunRulesOnChildren { set; }
        IEnumerable<string> RulesInSet { get; }
    }
}
