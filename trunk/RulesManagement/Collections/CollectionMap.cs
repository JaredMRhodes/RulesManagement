using System;
using System.Collections.Generic;

namespace RulesManagement.Collections
{
    /// <summary>
    /// This class is used to encapsulate key value pairs where the pair is a collection
    /// </summary>
    /// <typeparam name="Key">Any Type</typeparam>
    /// <typeparam name="Value">Any type to be within the underlying collection</typeparam>
    internal class CollectionMap<Key, Value>
    {
        /// <summary>
        /// The underlying key value map
        /// </summary>
        private Dictionary<Key, ICollection<Value>> _collectionMap;

        public CollectionMap()
        {
            _collectionMap = new Dictionary<Key, ICollection<Value>>();
        }

        public CollectionMap(IEqualityComparer<Key> comparer)
        {
            _collectionMap = new Dictionary<Key, ICollection<Value>>(comparer);
        }

        public void Add(Key key, Value value)
        {
            if (_collectionMap.ContainsKey(key))
            {
                _collectionMap[key].Add(value);
            }
            else
            {
                _collectionMap.Add(key, new List<Value> { value });
            }
        }

        public void Add(Key key, IEnumerable<Value> values)
        {
            if (_collectionMap.ContainsKey(key))
            {
                var collection = _collectionMap[key];
                foreach (var val in values)
                {
                    collection.Add(val);
                }
            }
            else
            {
                _collectionMap.Add(key, new List<Value>(values));
            }
        }

        public void Add(CollectionMap<Key, Value> map)
        {
            foreach (var item in map._collectionMap)
            {
                if (this._collectionMap.ContainsKey(item.Key))
                {
                    foreach (var val in item.Value)
                    {

                        bool contained = false;
                        foreach (var value in this._collectionMap[item.Key])
                        {
                            if (val.Equals(value))
                            {
                                contained = true;
                            }
                        }
                        if (!contained)
                        {
                            this._collectionMap[item.Key].Add(val);
                        }

                    }
                }
                else
                {
                    this.Add(item.Key, item.Value);
                }
            }
        }


        /// <summary>
        /// Get all values mapped to a key
        /// </summary>
        /// <param name="key">Any key of type Key</param>
        /// <returns>A collection of type Value</returns>
        public IEnumerable<Value> this[Key key]
        {
            get
            {
                if (_collectionMap.ContainsKey(key))
                {
                    return _collectionMap[key];
                }
                return new Value[0];
            }
        }

        /// <summary>
        /// Get all values mapped to the collection of keys
        /// </summary>
        /// <param name="keys">A collection type of Key</param>
        /// <returns>All values mapped to the collection of keys</returns>
        public IEnumerable<Value> this[IEnumerable<Key> keys]
        {
            get
            {
                List<Value> items = new List<Value>();
                foreach (Key key in keys)
                {
                    if (_collectionMap.ContainsKey(key))
                    {
                        items.AddRange(_collectionMap[key]);
                    }
                }
                return items;
            }
        }

        /// <summary>
        /// Returns all the keys
        /// </summary>
        public IEnumerable<Key> Keys
        {
            get
            {
                return _collectionMap.Keys;
            }
        }

        /// <summary>
        /// Clear all underlying collections
        /// </summary>
        public void Clear()
        {
            _collectionMap.Clear();
        }
    }
}
