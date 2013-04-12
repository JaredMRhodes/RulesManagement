using System;
using System.Collections.Generic;

namespace RulesManagement.TypeManagement
{
    /// <summary>
    /// Implementation for IIsEnumerable class
    /// </summary>
    internal class IsEnumerable : IIsEnumerable
    {
        private IDictionary<Type, bool> _isEnumerableMap;
        private IDictionary<Type, Type> _underlyingEnumerableTypeMap;

        public IsEnumerable()
        {
            _isEnumerableMap = new Dictionary<Type, bool>();
            _underlyingEnumerableTypeMap = new Dictionary<Type, Type>();
        }
        
        public bool IsTypeEnumerable<T>()
        {
            return this.IsTypeEnumerable(typeof(T));
        }

        public bool IsTypeEnumerable(Type type)
        {
            if (_isEnumerableMap.ContainsKey(type))
            {
                return _isEnumerableMap[type];
            }
            bool isEnumerable = type.GetInterface("IEnumerable`1") != null;
            _isEnumerableMap.Add(type, isEnumerable);
            if (isEnumerable)
            {
                if (typeof(Array).IsAssignableFrom(type))
                {
                    _underlyingEnumerableTypeMap.Add(type, type.GetElementType());
                }
                else
                {
                    var genericArguements = type.GetGenericArguments();
                    if (genericArguements.Length == 1)
                    {
                        _underlyingEnumerableTypeMap.Add(type, genericArguements[0]);
                    }
                    else if (genericArguements.Length == 2)
                    {
                        _underlyingEnumerableTypeMap.Add(type, genericArguements[1]);
                    }
                }
            }
            return isEnumerable;
        }

        public Type GetChildType<T>()
        {
            return this.GetChildType(typeof(T));
        }

        public Type GetChildType(Type type)
        {
            if (!_underlyingEnumerableTypeMap.ContainsKey(type))
            {
                if (!this.IsTypeEnumerable(type))
                {
                    return null;
                }
            }
            return _underlyingEnumerableTypeMap[type];
        }
    }
}
