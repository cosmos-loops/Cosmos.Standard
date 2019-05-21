﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Cosmos.Guava
{
    public partial class Joiner : IGuavaMapJoiner
    {
        #region SkipValueNulls

        IGuavaMapJoiner IGuavaMapJoiner.SkipNulls()
        {
            Options.SetSkipValueNulls();
            return this;
        }

        IGuavaMapJoiner IGuavaMapJoiner.SkipNulls(SkipNullType type)
        {
            Options.SetSkipValueNulls(type);
            return this;
        }

        #endregion

        #region UseForNull

        IGuavaMapJoiner IGuavaMapJoiner.UseForNull(string key, string value)
        {
            Options.SetMapReplace<string, string, string, string>(k => key, s => value);
            return this;
        }

        IGuavaMapJoiner IGuavaMapJoiner.UseForNull(Func<string, string> keyFunc, Func<string, string> valueFunc)
        {
            Options.SetMapReplace(keyFunc, valueFunc);
            return this;
        }

        IGuavaMapJoiner IGuavaMapJoiner.UseForNull<T1, T2>(T1 key, T2 value)
        {
            Options.SetMapReplace<T1, T1, T2, T2>(t => key, t => value);
            return this;
        }

        IGuavaMapJoiner IGuavaMapJoiner.UseForNull<T1, T2>(Func<T1, T1> keyFunc, Func<T2, T2> valueFunc)
        {
            Options.SetMapReplace(keyFunc, valueFunc);
            return this;
        }

        #endregion

        #region FromTuple

        IGuavaTupleJoiner IGuavaMapJoiner.FromTuple() => this;

        #endregion

        #region Join - KeyValuePair

        string IGuavaMapJoiner.Join(IEnumerable<string> list)
        {
            var replacer = Options.GetMapReplace<string, string, string, string>();
            var defaultKey = replacer.KeyFunc?.Invoke(string.Empty) ?? string.Empty;
            var defaultValue = replacer.ValueFunc?.Invoke(string.Empty) ?? string.Empty;
            return JoinToKeyValuePairString(list, defaultKey, defaultValue, s => s, v => v, replacer);
        }

        string IGuavaMapJoiner.Join(IEnumerable<string> list, string defaultKey, string defaultValue)
        {
            var replacer = Options.GetMapReplace<string, string, string, string>();
            return JoinToKeyValuePairString(list, defaultKey, defaultValue, s => s, v => v, replacer);
        }

        string IGuavaMapJoiner.Join<T>(IEnumerable<T> list, ITypeConverter<T, string> converter)
        {
            var replacer = Options.GetMapReplace<T, T, T, T>();
            var defaultKey = replacer.KeyFunc == null ? default : replacer.KeyFunc(default);
            var defaultValue = replacer.ValueFunc == null ? default : replacer.ValueFunc(default);
            return JoinToKeyValuePairString(list, defaultKey, defaultValue, converter.To, converter.To, replacer);
        }

        string IGuavaMapJoiner.Join<T>(IEnumerable<T> list, T defaultKey, T defaultValue, ITypeConverter<T, string> converter)
        {
            var replacer = Options.GetMapReplace<T, T, T, T>();
            return JoinToKeyValuePairString(list, defaultKey, defaultValue, converter.To, converter.To, replacer);
        }

        string IGuavaMapJoiner.Join(string str1, string str2, params string[] restStrings)
        {
            var list = new List<string> { str1, str2 };
            list.AddRange(restStrings);
            return ((IGuavaMapJoiner)this).Join(list);
        }

        private string JoinToKeyValuePairString<T>(IEnumerable<T> list, T defaultKey, T defaultValue, Func<T, string> keyFunc, Func<T, string> valueFunc,
            (Func<T, T> KeyFunc, Func<T, T> ValueFunc) replacer)
        {
            if (list == null)
                return string.Empty;

            var instances = list.ToList();
            if (!instances.Any())
                return string.Empty;

            if (instances.Count % 2 == 1)
                instances.Add(defaultValue);

            var timesToLoops = instances.Count / 2;
            var index = 0;
            var middle = new List<string>();
            for (var i = 0; i < timesToLoops; i++)
            {
                var k = instances[index++];
                var v = instances[index++];
                var key = keyFunc(k);
                var value = valueFunc(v);

                if (JoinerUtils.SkipMap(Options, key, value))
                    continue;
                else if (JoinerUtils.NeedFixMapValue(Options, key, value))
                {
                    key = JoinerUtils.FixMapKeySafety(k, key, value, defaultKey, keyFunc, replacer.KeyFunc, Options.SkipValueNullType);
                    value = JoinerUtils.FixMapValueSafety(v, value, key, defaultValue, valueFunc, replacer.ValueFunc, Options.SkipValueNullType);
                }

                middle.Add($"{key}{Options.MapSeparator}{value}");
            }

            return middle.JoinToString(_on, JoinerUtils.GetMapPredicate(Options));
        }


        #endregion

        #region Private class

        private partial class JoinerOptions
        {
            #region Skip Value Nulls - Map

            public bool SkipValueNullsFlag { get; private set; }

            public SkipNullType SkipValueNullType { get; private set; } = SkipNullType.Nothing;

            public void SetSkipValueNulls()
            {
                SkipValueNullsFlag = true;
                SkipValueNullType = SkipNullType.WhenBoth;
            }

            public void SetSkipValueNulls(SkipNullType type)
            {
                SkipValueNullsFlag = type != SkipNullType.Nothing;
                SkipValueNullType = type;
            }

            #endregion

            #region UseForNull - Map

            private JoinerObjectReplacer MapReplacer { get; set; }
            private bool MapValueReplacerFlag { get; set; }

            public (Func<T1, T2> KeyFunc, Func<T3, T4> ValueFunc) GetMapReplace<T1, T2, T3, T4>()
            {
                var keyFunc = MapValueReplacerFlag ? MapReplacer?.GetMapKey<T1, T2>() : null;
                var valueFunc = MapValueReplacerFlag ? MapReplacer?.GetMapValue<T3, T4>() : null;
                return (keyFunc, valueFunc);
            }

            public void SetMapReplace<T1, T2, T3, T4>(Func<T1, T2> mapKeyFunc, Func<T3, T4> mapValueFunc)
            {
                MapValueReplacerFlag = true;
                MapReplacer = JoinerObjectReplacer.CreateForMap(mapKeyFunc, mapValueFunc);
                SetSkipValueNulls(SkipNullType.Nothing);
            }

            #endregion

            #region WithKeyValueSeparator

            public string MapSeparator { get; private set; }

            public void SetMapSeparator(string separator)
            {
                MapSeparator = separator;
            }

            public void SetMapSeparator(char separator)
            {
                MapSeparator = $"{separator}";
            }

            #endregion
        }

        private static partial class JoinerUtils
        {
            public static bool SkipMap(JoinerOptions options, string key, string value)
            {
                if (options.SkipValueNullsFlag)
                {
                    switch (options.SkipValueNullType)
                    {
                        case SkipNullType.Nothing:
                            return false;

                        case SkipNullType.WhenBoth:
                            return string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value);

                        case SkipNullType.WhenEither:
                            return string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value);

                        case SkipNullType.WhenKeyIsNull:
                            return string.IsNullOrWhiteSpace(key);

                        case SkipNullType.WhenValueIsNull:
                            return string.IsNullOrWhiteSpace(value);
                    }
                }
                return false;
            }

            public static bool NeedFixMapValue(JoinerOptions options, string key, string value)
            {
                switch (options.SkipValueNullType)
                {
                    case SkipNullType.Nothing:
                        return string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value);

                    case SkipNullType.WhenBoth:
                        return false;

                    case SkipNullType.WhenEither:
                        return false;

                    case SkipNullType.WhenKeyIsNull:
                        return !string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(value);

                    case SkipNullType.WhenValueIsNull:
                        return string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value);
                }

                return false;
            }

            public static string FixMapKeySafety<T>(T k, string key, string value, T defaultKey, Func<T, string> to, Func<T, T> keyFunc, SkipNullType type)
            {
                if (!string.IsNullOrWhiteSpace(key))
                    return key;

                if (type == SkipNullType.WhenEither)
                    return key;

                if (type == SkipNullType.Nothing || (type == SkipNullType.WhenValueIsNull && !string.IsNullOrWhiteSpace(value)))
                {
                    return to(keyFunc == null ? defaultKey : keyFunc(k));
                }

                return key;
            }

            public static string FixMapValueSafety<T>(T v, string value, string key, T defaultValue, Func<T, string> to, Func<T, T> valueFunc, SkipNullType type)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    return value;

                if (type == SkipNullType.WhenEither)
                    return value;

                if (type == SkipNullType.Nothing || (type == SkipNullType.WhenKeyIsNull && !string.IsNullOrWhiteSpace(key)))
                {
                    return to(valueFunc == null ? defaultValue : valueFunc(v));
                }

                return value;
            }

            public static Func<string, bool> GetMapPredicate(JoinerOptions options)
            {
                if (options.SkipValueNullsFlag)
                    return s => s != options.MapSeparator;
                return null;
            }
        }

        #endregion
    }

    public interface IGuavaMapJoiner
    {
        IGuavaMapJoiner SkipNulls();
        IGuavaMapJoiner SkipNulls(SkipNullType type);
        IGuavaMapJoiner UseForNull(string key, string value);
        IGuavaMapJoiner UseForNull(Func<string, string> keyFunc, Func<string, string> valueFunc);
        IGuavaMapJoiner UseForNull<T1, T2>(T1 key, T2 value);
        IGuavaMapJoiner UseForNull<T1, T2>(Func<T1, T1> keyFunc, Func<T2, T2> valueFunc);
        IGuavaTupleJoiner FromTuple();
        string Join(IEnumerable<string> list);
        string Join(IEnumerable<string> list, string defaultKey, string defaultValue);
        string Join<T>(IEnumerable<T> list, ITypeConverter<T, string> converter);
        string Join<T>(IEnumerable<T> list, T defaultKey, T defaultValue, ITypeConverter<T, string> converter);
        string Join(string str1, string str2, params string[] restStrings);
    }
}