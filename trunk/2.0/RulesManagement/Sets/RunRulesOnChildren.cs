using System;

namespace RulesManagement.Sets
{
    /// <summary>
    /// An enumeration to help the user of the library
    /// determine if they should run rules on collection of children.
    /// Use this instead of a boolean for clarity
    /// </summary>
    public enum RunRulesOnChildren
    {
        /// <summary>
        /// Use this if you would like to run rules on enumerable objects children
        /// </summary>
        RunRulesOnChildren = 0,

        /// <summary>
        /// Use this if you would not like to run rules on enumerable objects children
        /// </summary>
        DoNotRunRulesOnChildren = 1
    }
}
