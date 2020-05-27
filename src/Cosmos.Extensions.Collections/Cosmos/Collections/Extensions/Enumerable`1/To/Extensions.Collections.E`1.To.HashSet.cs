using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Cosmos.Collections
{
    /// <summary>
    /// Enumerable extensions
    /// </summary>
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// To hashset
        /// </summary>
        /// <param name="src"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> src) where T : IComparable<T> =>
            src.ToHashSet(EqualityComparer<T>.Default);

        /// <summary>
        /// To hashset
        /// </summary>
        /// <param name="src"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> src, IEqualityComparer<T> comparer)
        where T : IComparable<T>
        {
            if (src is null)
                throw new ArgumentNullException(nameof(src));
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));
            return new HashSet<T>(src, comparer);
        }

        /// <summary>
        /// To HashSet
        /// </summary>
        /// <param name="src"></param>
        /// <param name="ignoreDup"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> src, bool ignoreDup) where T : IComparable<T> =>
            ignoreDup
                ? src.Distinct().ToHashSet(EqualityComparer<T>.Default)
                : src.ToHashSet(EqualityComparer<T>.Default);

        /// <summary>
        /// To HashSet
        /// </summary>
        /// <param name="src"></param>
        /// <param name="keyFunc"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public static HashSet<TKey> ToHashSet<T, TKey>(this IEnumerable<T> src, Func<T, TKey> keyFunc) where TKey : IComparable<TKey>
        {
            if (keyFunc is null) throw new ArgumentNullException(nameof(keyFunc));
            return src.Select(i => keyFunc(i)).ToHashSet(EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// To HashSet ignore duplicates
        /// </summary>
        /// <param name="src"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static HashSet<T> ToHashSetIgnoringDuplicates<T>(this IEnumerable<T> src) where T : IComparable<T> => src.ToHashSet(true);
    }
}