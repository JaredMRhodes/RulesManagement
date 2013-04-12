using System;
using System.Collections.Generic;
using RulesManagement.Collections;

namespace RulesManagement.TypeManagement
{
    /// <summary>
    /// Type graph cache allows the caching of assignability of types
    /// This allows for one time calls on establishing whether a rule can
    /// apply to an object type
    /// </summary>
    class TypeAssignmentCache : ITypeAssignmentCache
    {
        /// <summary>
        /// Keeps track of all types requested. Removes redundancy lookups if collection is empty
        /// </summary>
        private ICollection<Type> _typesCached;

        /// <summary>
        /// Map of types and the types they can be assigned to
        /// </summary>
        private CollectionMap<Type, Type> _typesMap;

        public TypeAssignmentCache()
        {
            _typesMap = new CollectionMap<Type, Type>();
            _typesCached = new List<Type>();
        }

        /// <summary>
        /// Clears all underlying lists
        /// </summary>
        public void Reset()
        {
            _typesMap.Clear();
            _typesCached.Clear();
        }

        /// <summary>
        /// Returns true if type has been cached, false if not
        /// </summary>
        /// <param name="type">The type to check against cache</param>
        /// <returns>true if type has been cached, false if not</returns>
        public bool KnowsType(Type type)
        {
            return _typesCached.Contains(type);
        }

        /// <summary>
        /// Examines the typeToAdd against a collection of types
        /// (typesToExamine) to see if there is any assignability
        /// </summary>
        /// <param name="typeToAdd">The type to check for assignability</param>
        /// <param name="typesToExamine">The types to see if typeToAdd can be assigned to</param>
        public void AddType(Type typeToAdd, IEnumerable<Type> typesToExamine)
        {
            _typesCached.Add(typeToAdd);
            foreach (Type type in typesToExamine)
            {
                if (type.IsAssignableFrom(typeToAdd))
                {
                    _typesMap.Add(typeToAdd, type);
                }
            }
            if (!this.Contains(_typesMap[typeToAdd],typeToAdd))
            {
                _typesMap.Add(typeToAdd, typeToAdd);
            }
        }

        /// <summary>
        /// Determine if the colleciton contains the type
        /// </summary>
        /// <param name="enumerable">The collection</param>
        /// <param name="typeToAdd">The type to add</param>
        /// <returns>True if it is contained, false if not</returns>
        private bool Contains(IEnumerable<Type> enumerable, Type typeToAdd)
        {
            foreach (Type type in enumerable)
            {
                if (typeToAdd == type)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns all types in the underlying graph mapped to the known type
        /// </summary>
        /// <param name="type">The type to grab the assignability for</param>
        /// <returns>all types in the underlying graph mapped to the known type</returns>
        public IEnumerable<Type> GetGraph(Type type)
        {
            return _typesMap[type];
        }
    }
}
