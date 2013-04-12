using System;
using System.Collections;
using System.Collections.Generic;
using RulesManagement.Collections;
using RulesManagement.Exceptions;
using RulesManagement.TypeManagement;
using RulesManagement.Validation;

namespace RulesManagement.Sets
{
    public delegate TResult Func<T, TResult>(T arg);

    public class ValidationSet : IValidationSet
    {
        private ValidationResult _validationResult;
        private CollectionMap<Type, Func<object, ValidationMessage>> _validaitonRules;
        private RunRulesOnChildren _runRulesOnChildren;
        private ITypeAssignmentCache _typeAssignmentCache;
        private ICollection<string> _rulesInSet;
        private IIsEnumerable _isEnumerableCache;


        public ValidationSet(RunRulesOnChildren runRulesOnChildren)
        {
            this.RunRulesOnChildren = runRulesOnChildren;
            this._validaitonRules = new CollectionMap<Type, Func<object, ValidationMessage>>();
            this._typeAssignmentCache = new TypeAssignmentCache();
            this._rulesInSet = new List<string>();
            this._isEnumerableCache = new IsEnumerable();
        }

        public void AddValidationRule<T>(Rules.IValidationRule<T> rule)
        {
            this._validaitonRules.Add(typeof(T),new Func<object,ValidationMessage>((x)=>
                {
                    return rule.RunValidaiton((T)x);
                }));
            this._rulesInSet.Add(rule.RuleName);
            this._typeAssignmentCache.Reset();
        }

        public void AddValidaitonRules(ValidationSet set)
        {
            this._validaitonRules.Add(set._validaitonRules);
            foreach (var ruleName in set._rulesInSet)
            {
                this._rulesInSet.Add(ruleName);
            }
            this._typeAssignmentCache.Reset();
        }

        public IValidationSet RunValidationRules<T>(T item)
        {
            this.RunValidaitonRule(typeof(T), item);
            return this;
        }
        
        public IValidationSet RunValidaitonRules(params object[] items)
        {
            foreach (object item in items)
            {
                this.RunValidaitonRule(item.GetType(), item);
            }
            return this;
        }

        private void RunValidaitonRule(Type type, object item)
        {
            //if validation result has not been set, set it
            if (this._validationResult == null)
            {
                this._validationResult = new ValidationResult();
            }
            //check if type assignment knows about type
            if (!this._typeAssignmentCache.KnowsType(type))
            {
                this._typeAssignmentCache.AddType(type,_validaitonRules.Keys);
            }
            //get all rules from all assignable types and run them against the object
            foreach (var rule in this._validaitonRules[this._typeAssignmentCache.GetGraph(type)])
            {
                this._validationResult.Add(rule.Invoke(item));
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
                            this.RunValidaitonRule(child.GetType(), child);
                        }
                    }
                    else
                    {
                        foreach (object child in (IEnumerable)item)
                        {
                            this.RunValidaitonRule(childType, child);
                        }
                    }
                }
            }
        }


        public ValidationResult ValidationResult
        {
            get 
            {
                if (this._validationResult == null)
                {
                    throw new RulesNotRunException("Validation rules haven't been run");
                }
                return this._validationResult;
            }
        }

        public bool RuleResult
        {
            get 
            {
                if (this._validationResult == null)
                {
                    throw new RulesNotRunException("Validation rules have not been run");
                }
                return this._validationResult.ValidationState == ValidationState.Passed;
            }
        }

        public void PrecacheType(Type type)
        {
            this._isEnumerableCache.IsTypeEnumerable(type);
            this._typeAssignmentCache.AddType(type, this._validaitonRules.Keys);
        }

        public void PrecacheType<T>()
        {
            this.PrecacheType(typeof(T));
        }

        public RunRulesOnChildren RunRulesOnChildren
        {
            set
            {
                this._runRulesOnChildren = Sets.RunRulesOnChildren.RunRulesOnChildren;
            }
        }

        public IEnumerable<string> RulesInSet
        {
            get
            {
                return this._rulesInSet;
            }
        }

        public void Dispose()
        {
            this._validationResult = null;
        }
    }
}
