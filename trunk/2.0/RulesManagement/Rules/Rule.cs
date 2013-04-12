using System;

namespace RulesManagement.Rules
{
    public abstract class Rule<T> :  IRule<T>
    {
        public abstract bool RunRule(T item);


        public string RuleName
        {
            get 
            {
                return this.GetType().Name;
            }
        }
    }
}
