using System;

namespace RulesManagement.TypeManagement
{
    /// <summary>
    /// Interface to define contract for checking if types are enumerable or not
    /// </summary>
    internal interface IIsEnumerable
    {
        bool IsTypeEnumerable<T>();
        bool IsTypeEnumerable(Type type);
        Type GetChildType<T>();
        Type GetChildType(Type type);
    }
}
