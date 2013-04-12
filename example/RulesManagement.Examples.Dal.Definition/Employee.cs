using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RulesManagement.Examples.Dal.Definition
{
    public class Employee : Person
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal SalaryPerHour { get; set; }
        public DateTime DateHired { get; set; }
    }
}
