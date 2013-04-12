using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RulesManagement.Examples.Dal.Definition
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
