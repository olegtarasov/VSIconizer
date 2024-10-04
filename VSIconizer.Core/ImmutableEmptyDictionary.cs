using System;
using System.Collections;
using System.Collections.Generic;

namespace VSIconizer.Core
{
    internal sealed class ImmutableEmptyDictionary<TKey,TValue> : IReadOnlyDictionary<TKey,TValue>
    {
        public static ImmutableEmptyDictionary<TKey,TValue> Instance { get; } = new ImmutableEmptyDictionary<TKey, TValue>();

        private ImmutableEmptyDictionary()
        {
        }

        public bool ContainsKey(TKey key) => false;

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default;
            return false;
        }

        public TValue this[TKey key] => throw new ArgumentOutOfRangeException(paramName: nameof(key));

        public IEnumerable<TKey> Keys => Array.Empty<TKey>();

        public IEnumerable<TValue> Values => Array.Empty<TValue>();

        public int Count => 0;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            IReadOnlyList< KeyValuePair<TKey, TValue> > emptyList = Array.Empty< KeyValuePair<TKey, TValue> >();
            return emptyList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
