using System;
using System.Collections.Generic;
using System.Globalization;
using Cosmos.Conversions.Core;

namespace Cosmos.Conversions.Determiners
{
    /// <summary>
    /// Internal core conversion helper from string to DateTimeOffset
    /// </summary>
    internal static class StringDateTimeOffsetDeterminer
    {
        /// <summary>
        /// Is
        /// </summary>
        /// <param name="str"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <param name="dtAct"></param>
        /// <returns></returns>
        public static bool Is(string str,
            DateTimeStyles style = DateTimeStyles.None,
            IFormatProvider formatProvider = null,
            Action<DateTimeOffset> dtAct = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            var result = DateTimeOffset.TryParse(str, formatProvider.SafeDateTime(), style, out var dateTimeOffset);
            if (!result)
                result = ValueDeterminer.IsXxxAgain<DateTimeOffset>(str);
            if (result)
                dtAct?.Invoke(dateTimeOffset);
            return result;
        }

        /// <summary>
        /// Is
        /// </summary>
        /// <param name="str"></param>
        /// <param name="tries"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <param name="dtAct"></param>
        /// <returns></returns>
        public static bool Is(string str,
            IEnumerable<IConversionTry<string, DateTimeOffset>> tries,
            DateTimeStyles style = DateTimeStyles.None,
            IFormatProvider formatProvider = null,
            Action<DateTimeOffset> dtAct = null)
        {
            return ValueDeterminer.IsXXX(str, string.IsNullOrWhiteSpace,
                (s, act) => Is(s, style, formatProvider, act), tries, dtAct);
        }

        /// <summary>
        /// To
        /// </summary>
        /// <param name="str"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static DateTimeOffset To(string str,
            DateTimeStyles style = DateTimeStyles.None,
            IFormatProvider formatProvider = null,
            DateTimeOffset defaultVal = default)
        {
            return DateTimeOffset.TryParse(str, formatProvider.SafeDateTime(), style, out var offset)
                ? offset
                : defaultVal;
        }

        /// <summary>
        /// To
        /// </summary>
        /// <param name="str"></param>
        /// <param name="impls"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static DateTimeOffset To(string str,
            IEnumerable<IConversionImpl<string, DateTimeOffset>> impls,
            DateTimeStyles style = DateTimeStyles.None,
            IFormatProvider formatProvider = null)
        {
            return ValueConverter.ToXxx(str, (s, act) => Is(s, style, formatProvider.SafeDateTime(), act), impls);
        }

        /// <summary>
        /// Exact DateTimeOffset Determiner
        /// </summary>
        public static class Exact
        {
            /// <summary>
            /// Is
            /// </summary>
            /// <param name="str"></param>
            /// <param name="format"></param>
            /// <param name="style"></param>
            /// <param name="formatProvider"></param>
            /// <param name="dtAct"></param>
            /// <returns></returns>
            public static bool Is(string str,
                string format,
                DateTimeStyles style = DateTimeStyles.None,
                IFormatProvider formatProvider = null,
                Action<DateTimeOffset> dtAct = null)
            {
                if (string.IsNullOrWhiteSpace(str))
                    return false;
                var result = DateTimeOffset.TryParseExact(str, format, formatProvider.SafeDateTime(), style, out var dateTimeOffset);
                if (result)
                    dtAct?.Invoke(dateTimeOffset);
                return result;
            }

            /// <summary>
            /// Is
            /// </summary>
            /// <param name="str"></param>
            /// <param name="format"></param>
            /// <param name="tries"></param>
            /// <param name="style"></param>
            /// <param name="formatProvider"></param>
            /// <param name="dtAct"></param>
            /// <returns></returns>
            public static bool Is(string str,
                string format,
                IEnumerable<IConversionTry<string, DateTimeOffset>> tries,
                DateTimeStyles style = DateTimeStyles.None,
                IFormatProvider formatProvider = null,
                Action<DateTimeOffset> dtAct = null)
            {
                return ValueDeterminer.IsXXX(str, string.IsNullOrWhiteSpace,
                    (s, act) => Is(s, format, style, formatProvider, act), tries, dtAct);
            }

            /// <summary>
            /// To
            /// </summary>
            /// <param name="str"></param>
            /// <param name="format"></param>
            /// <param name="style"></param>
            /// <param name="formatProvider"></param>
            /// <param name="defaultVal"></param>
            /// <returns></returns>
            public static DateTimeOffset To(string str,
                string format,
                DateTimeStyles style = DateTimeStyles.None,
                IFormatProvider formatProvider = null,
                DateTimeOffset defaultVal = default)
            {
                return DateTimeOffset.TryParseExact(str, format, formatProvider.SafeDateTime(), style, out var offset)
                    ? offset
                    : defaultVal;
            }

            /// <summary>
            /// To
            /// </summary>
            /// <param name="str"></param>
            /// <param name="format"></param>
            /// <param name="impls"></param>
            /// <param name="style"></param>
            /// <param name="formatProvider"></param>
            /// <returns></returns>
            public static DateTimeOffset To(string str,
                string format,
                IEnumerable<IConversionImpl<string, DateTimeOffset>> impls,
                DateTimeStyles style = DateTimeStyles.None,
                IFormatProvider formatProvider = null)
            {
                return ValueConverter.ToXxx(str, (s, act) => Is(s, format, style, formatProvider.SafeDateTime(), act), impls);
            }
        }
    }
}