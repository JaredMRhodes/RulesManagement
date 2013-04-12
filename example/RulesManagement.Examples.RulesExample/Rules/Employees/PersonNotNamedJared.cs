using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RulesManagement.Examples.Dal.Definition;
using RulesManagement.Rules;
using RulesManagement.Validation;

namespace RulesManagement.Examples.RulesExample.Rules.Employees
{
    public class PersonNotNamedJared : ValidationRule<Person>
    {
        public override ValidationMessage RunValidaiton(Person item)
        {
            if (String.Compare(item.FirstName, "Jared", true) == 0 && String.Compare(item.LastName,"Rhodes",true) == 0)
            {
                return new ValidationMessage(ValidationState.Failed, "There can be only one Jared Rhodes");
            }
            return new ValidationMessage(ValidationState.Passed);
        }
    }
}
