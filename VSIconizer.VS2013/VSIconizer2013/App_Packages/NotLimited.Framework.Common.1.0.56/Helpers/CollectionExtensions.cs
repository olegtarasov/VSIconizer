//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;

namespace NotLimited.Framework.Common.Helpers
{
    public static class CollectionExtensions
    {
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> source)
        {
            foreach (var pair in source)
            {
                dictionary[pair.Key] = pair.Value;
            }
        }

        public static bool EqualsTo<TSrc, TDst>(this IReadOnlyList<TSrc> source, List<TDst> destination, Func<TSrc, TDst, bool> comparer)
        {
            if (ReferenceEquals(source, destination))
                return true;

            if (source == null || destination == null)
                return false;

            if (source.Count != destination.Count)
                return false;

            for (int i = 0; i < source.Count; i++)
            {
                if (!comparer(source[i], destination[i]))
                    return false;
            }

            return true;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> source)
        {
            foreach (var item in source)
                collection.Add(item);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static HashSet<T> ConcatHashSet<T>(this IEnumerable<T> source, IEnumerable<T> target)
        {
            var result = CreateHashSetFromSource(source);
            foreach (var item in target)
                result.Add(item);

            return result;
        }

        public static HashSet<T> ConcatHashSet<T>(this IEnumerable<T> source, params T[] set)
        {
            var result = CreateHashSetFromSource(source);
            foreach (var item in set)
                result.Add(item);

            return result;
        }

        private static HashSet<T> CreateHashSetFromSource<T>(IEnumerable<T> source)
        {
            var sourceSet = source as HashSet<T>;
            return sourceSet == null ? new HashSet<T>(source) : new HashSet<T>(sourceSet, sourceSet.Comparer);
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static List<T> CreateEmptyIfNull<T>(this List<T> source)
        {
            return source ?? new List<T>();
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T def)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return def;
        }

        public static T LastOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T def)
        {
            var result = def;
            foreach (var item in source)
            {
                if (predicate(item))
                    result = item;
            }

            return result;
        }

        public static TValue Max<TSource, TValue>(this IEnumerable<TSource> source, Func<TSource, double> keySelector, Func<TSource, TValue> valueSelector)
        {
            double max = double.MinValue;
            TValue value = default(TValue);

            foreach (var item in source)
            {
                var key = keySelector(item);
                if (key > max)
                {
                    max = key;
                    value = valueSelector(item);
                }
            }

            return value;
        }

        public static TValue Max<TSource, TKey, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector) where TKey : IComparable<TKey>
        {
            return SelectByComparison(source, keySelector, valueSelector, 1);
        }

        public static TValue Min<TSource, TKey, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector) where TKey : IComparable<TKey>
        {
            return SelectByComparison(source, keySelector, valueSelector, -1);
        }

        private static TValue SelectByComparison<TSource, TKey, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, 
            Func<TSource, TValue> valueSelector,
            int comparisonResult) where TKey : IComparable<TKey>
        {
            if (!source.Any())
            {
                return default(TValue);
            }

            var first = source.First();
            var curKey = keySelector(first);
            var curValue = valueSelector(first);

            foreach (var item in source.Skip(1))
            {
                var key = keySelector(item);
                if (key.CompareTo(curKey) == comparisonResult)
                {
                    curKey = key;
                    curValue = valueSelector(item);
                }
            }

            return curValue;
        }

        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryRemove(key, out value);
        }

        public static void AddOrUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary.AddOrUpdate(key, value, (key1, value1) => value);
        }

        public static IEnumerable<TElement> IterateQueueList<TElement, TContainer>(
            this TContainer root, 
            Func<TContainer, IEnumerable<TElement>> elementAccessor,
            Func<TContainer, IEnumerable<TContainer>> childrenAccessor)
        {
            if (root == null) throw new ArgumentNullException("root");
            if (elementAccessor == null) throw new ArgumentNullException("elementAccessor");
            if (childrenAccessor == null) throw new ArgumentNullException("childrenAccessor");

            var queue = new Queue<TContainer>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                foreach (var element in elementAccessor(item))
                {
                    yield return element;
                }

                foreach (var child in childrenAccessor(item))
                {
                    queue.Enqueue(child);
                }
            }
        }

        public static IEnumerable<TElement> IterateQueue<TElement, TContainer>(
            this TContainer root,
            Func<TContainer, TElement> elementAccessor,
            Func<TContainer, IEnumerable<TContainer>> childrenAccessor)
        {
            if (root == null) throw new ArgumentNullException("root");
            if (elementAccessor == null) throw new ArgumentNullException("elementAccessor");
            if (childrenAccessor == null) throw new ArgumentNullException("childrenAccessor");

            var queue = new Queue<TContainer>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                
                yield return elementAccessor(item);

                foreach (var child in childrenAccessor(item))
                {
                    queue.Enqueue(child);
                }
            }
        }
    }
}