using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RulesManagement.Examples.RulesExample.Rules.Employees;
using RulesManagement.Sets;

namespace RulesManagement.Examples.RulesExample.Rules
{
    public class EmployeeBasicRules : ValidationSet
    {
        public EmployeeBasicRules()
            : base(RunRulesOnChildren.RunRulesOnChildren)
        {
            this.AddValidationRule(new EmployeeValidation());
            this.AddValidationRule(new PersonNotNamedJared());
        }
    }
}
