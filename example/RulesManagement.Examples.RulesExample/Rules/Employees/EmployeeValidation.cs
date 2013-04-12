using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RulesManagement.Examples.Dal.Definition;
using RulesManagement.Rules;
using RulesManagement.Validation;

namespace RulesManagement.Examples.RulesExample.Rules.Employees
{
    public class EmployeeValidation : ValidationRule<Employee>
    {
        public override ValidationMessage RunValidaiton(Employee item)
        {
            if (item.SalaryPerHour > 50)
            {
                return new ValidationMessage(ValidationState.Failed, "Only executives can make more than 50 per hour");
            }
            if (item.SalaryPerHour < 0)
            {
                return new ValidationMessage(ValidationState.Failed, "They aren't paying us to work here");
            }
            if (String.IsNullOrEmpty(item.Title))
            {
                return new ValidationMessage(ValidationState.Failed, "Every employee has to have a title, even developers");
            }
            return new ValidationMessage(ValidationState.Passed);
        }
    }
}
