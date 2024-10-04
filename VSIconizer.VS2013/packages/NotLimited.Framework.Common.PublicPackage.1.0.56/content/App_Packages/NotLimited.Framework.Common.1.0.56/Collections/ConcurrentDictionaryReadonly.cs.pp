//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NotLimited.Framework.Common.Collections
{
    public class ConcurrentDictionaryReadonly<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        public ConcurrentDictionaryReadonly()
        {
        }

        public ConcurrentDictionaryReadonly(int concurrencyLevel, int capacity) : base(concurrencyLevel, capacity)
        {
        }

        public ConcurrentDictionaryReadonly(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection)
        {
        }

        public ConcurrentDictionaryReadonly(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        public ConcurrentDictionaryReadonly(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : base(collection, comparer)
        {
        }

        public ConcurrentDictionaryReadonly(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : base(concurrencyLevel, collection, comparer)
        {
        }

        public ConcurrentDictionaryReadonly(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer) : base(concurrencyLevel, capacity, comparer)
        {
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys { get { return Keys; } }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values { get { return Values; } }
    }
}