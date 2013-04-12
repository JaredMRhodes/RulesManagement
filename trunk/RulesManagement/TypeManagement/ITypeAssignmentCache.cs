using System;
using System.Collections.Generic;

namespace RulesManagement.TypeManagement
{
    interface ITypeAssignmentCache
    {
        void Reset();

        bool KnowsType(Type type);

        IEnumerable<Type> GetGraph(Type type);

        void AddType(Type typeToAdd, IEnumerable<Type> typesToExamine);
    }
}
