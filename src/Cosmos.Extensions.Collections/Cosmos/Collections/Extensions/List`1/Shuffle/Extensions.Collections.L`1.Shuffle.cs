using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Cosmos.Collections
{
    /// <summary>
    /// Collection extensions
    /// </summary>
    public static partial class CollectionExtensions
    {
        /// <summary>
        /// Shuffle in place
        /// </summary>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShuffleInPlace<T>(this IList<T> items) => ShuffleInPlace(items, 4);

        /// <summary>
        /// Shuffle in place
        /// </summary>
        /// <param name="items"></param>
        /// <param name="times"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShuffleInPlace<T>(this IList<T> items, int times)
        {
            for (var j = 0; j < times; j++)
            {
                var rnd = new Random((int) (DateTime.Now.Ticks % int.MaxValue) - j);

                for (var i = 0; i < items.Count; i++)
                {
                    var index = rnd.Next(items.Count - 1);
                    var temp = items[index];
                    items[index] = items[i];
                    items[i] = temp;
                }
            }
        }

        /// <summary>
        /// Shuffle to new list
        /// </summary>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ShuffleToNewList<T>(this IList<T> items) => ShuffleToNewList(items, 4);

        /// <summary>
        /// Shuffle to new list
        /// </summary>
        /// <param name="items"></param>
        /// <param name="times"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ShuffleToNewList<T>(this IList<T> items, int times)
        {
            var res = new List<T>(items);
            res.ShuffleInPlace(times);
            return res;
        }
    }
}