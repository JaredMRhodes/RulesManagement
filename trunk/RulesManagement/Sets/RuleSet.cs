using System;
using System.Collections;
using System.Collections.Generic;
using RulesManagement.Collections;
using RulesManagement.Exceptions;
using RulesManagement.Rules;
using RulesManagement.TypeManagement;

namespace RulesManagement.Sets
{
    /// <summary>
    /// A collection of rules
    /// </summary>
    public class RuleSet : IRuleSet
    {
        /// <summary>
        /// The collection of rules mapped by type
        /// </summary>
        private CollectionMap<Type, Predicate<object>> _rules;

        /// <summary>
        /// The cache of type assignability.
        /// </summary>
        private ITypeAssignmentCache _typeAssignmentCache;

        /// <summary>
        /// An object for checking if an object derives from ienumerable
        /// </summary>
        private IIsEnumerable _isEnumerableCache;

        /// <summary>
        /// the result of rules run
        /// </summary>
        private Nullable<bool> _ruleResult;

        /// <summary>
        /// An enumerable to determine if we run rules on the
        /// items contained within a collection
        /// </summary>
        private RunRulesOnChildren _runRulesOnChildren;

        /// <summary>
        /// A collection of all rule names
        /// </summary>
        private ICollection<string> _rulesInSet;

        /// <summary>
        /// Creates a rules set
        /// </summary>
        /// <param name="runRulesOnChildren">Determines if enumerable types will have rules run on their contained types</param>
        public RuleSet(RunRulesOnChildren runRulesOnChildren)
        {
            this._rules = new CollectionMap<Type, Predicate<object>>();
            this._typeAssignmentCache = new TypeAssignmentCache();
            this._isEnumerableCache = new IsEnumerable();
            this.RunRulesOnChildren = runRulesOnChildren;
            this._rulesInSet = new List<string>();
        }

        /// <summary>
        /// Add a rule to the set. When rules are run against an object
        /// if it is assignable to T then that object will be run against that rule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rule"></param>
        public void AddRule<T>(IRule<T> rule)
        {
            this._rules.Add(typeof(T),new Predicate<object>((x)=>rule.RunRule((T)x)));
            this._rulesInSet.Add(rule.RuleName);
            this._typeAssignmentCache.Reset();   //Reset the type assignment cache because a new rule was added
        }

        /// <summary>
        /// Internal function to allow other rule types to add rules without
        /// using the encapsulating types
        /// </summary>
        /// <param name="type"></param>
        /// <param name="_rule"></param>
        internal void AddRule(Type type, Predicate<object> rule,string ruleName)
        {
            this._rules.Add(type, rule);
            this._rulesInSet.Add(ruleName);
            this._typeAssignmentCache.Reset();
        }

        /// <summary>
        /// Add a set of rules to this one
        /// Allows for rule set sharing
        /// </summary>
        /// <param name="set">A rule set</param>
        public void AddRules(RuleSet set)
        {
            this._rules.Add(set._rules);
            foreach (var ruleName in set._rulesInSet)
            {
                this._rulesInSet.Add(ruleName);
            }
            this._typeAssignmentCache.Reset();
        }

        /// <summary>
        /// Run rules against a single ovject
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="item">The object to run rules against</param>
        /// <returns>The current rule set, to check the result see RuleResult</returns>
        public IRuleSet RunRule<T>(T item)
        {
            this.RunRule(typeof(T), item);
            return this;
        }

        /// <summary>
        /// Run rules against all objects passed in
        /// </summary>
        /// <param name="items">Any objects to run rules against</param>
        /// <returns>the current rule set</returns>
        public IRuleSet RunRule(params object[] items)
        {
            foreach (object obj in items)
            {
                this.RunRule(obj.GetType(), obj);
            }
            return this;
        }

        /// <summary>
        /// The interal rule runner
        /// </summary>
        /// <param name="type">The type of the item to be run</param>
        /// <param name="item">the object to run rules against</param>
        private void RunRule(Type type, object item)
        {
            //If this is the first time running the rules, set the rule result
            if (!this._ruleResult.HasValue)
            {
                this._ruleResult = true;
            }
            //If a rule has already failed, do not run any more rules
            if (!this._ruleResult.Value)
            {
                return;
            }
            //If the type cache does not know about this type, add it
            if (!this._typeAssignmentCache.KnowsType(type))
            {
                this._typeAssignmentCache.AddType(type, this._rules.Keys);
            }
            //loop through all rules for this type and types its assignable to
            foreach (var rule in this._rules[this._typeAssignmentCache.GetGraph(type)])
            {
                //if a rule fails, set the result and return as to not run rules frivolously
                if (!rule.Invoke(item))
                {
                    this._ruleResult = false;
                    return;
                }
            }
            //If we are to run rules on children, check if the type is enumerable and then run
            //rules against the child types contained in the enumerable
            if (this._runRulesOnChildren == Sets.RunRulesOnChildren.RunRulesOnChildren)
            {
                if (type != typeof(string) && this._isEnumerableCache.IsTypeEnumerable(type))
                {
                    Type childType = this._isEnumerableCache.GetChildType(type);
                    if (typeof(object) == childType)
                    {
                        foreach (object child in (IEnumerable)item)
                        {
                            this.RunRule(child.GetType(), child);
                        }
                    }
                    else
                    {
                        foreach (object child in (IEnumerable)item)
                        {
                            this.RunRule(childType, child);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This is the result of the run rules
        /// </summary>
        public bool RuleResult
        {
            get
            {
                if (this._ruleResult.HasValue)
                {
                    return this._ruleResult.Value;
                }
                throw new RulesNotRunException("The rules have not been run");
            }
        }

        /// <summary>
        /// Precache a type so that when a rule is run against an
        /// object of that type, it doesn't have to look it up
        /// </summary>
        /// <param name="type">The type being cached</param>
        public void PrecacheType(Type type)
        {
            this._isEnumerableCache.IsTypeEnumerable(type);
            this._typeAssignmentCache.AddType(type,this._rules.Keys);
        }

        /// <summary>
        /// Precache a type so that when a rule is run against an
        /// object of that type, it doesn't have to look it up
        /// </summary>
        /// <typeparam name="T">The type to be cached</typeparam>
        public void PrecacheType<T>()
        {
            this.PrecacheType(typeof(T));
        }

        /// <summary>
        /// Set this property to determine if objects in a collection
        /// should have rules run against them
        /// </summary>
        public RunRulesOnChildren RunRulesOnChildren
        {
            set 
            {
                this._runRulesOnChildren = value;
            }
        }

        /// <summary>
        /// Returns the names of all registered rules
        /// </summary>
        public IEnumerable<string> RulesInSet
        {
            get 
            { 
                return this._rulesInSet; 
            }
        }

        /// <summary>
        /// Resets the result of the set
        /// </summary>
        public void Dispose()
        {
            this._ruleResult = new Nullable<bool>();
        }
    }
}
