﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using Cosmos.Conversions.Determiners;
using Cosmos.Numeric;

namespace Cosmos.Text
{
    public static class StringJudgeExtensions
    {
        #region Is

        /// <summary>
        /// Determine whether the given string is char. <br />
        /// 判断给定的字符串是否是 char。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChar(this string str) => StringCharDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is boolean. <br />
        /// 判断给定的字符串是否是 boolean。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBoolean(this string str) => StringBooleanDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is <see cref="Encoding"/>. <br />
        /// 判断给定的字符串是否是 <see cref="Encoding"/>。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEncoding(this string str) => StringEncodingDeterminer.Is(str);
        
        /// <summary>
        /// Determine whether the given string is a version number. <br />
        /// 判断给定的字符串是否是版本号。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsVersion(this string str) => StringVersionDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is an IP address. <br />
        /// 判断给定的字符串是否是 IP 地址。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIpAddress(this string str) => StringIpAddressDeterminer.Is(str);

        #endregion

        #region Is DateTime

        /// <summary>
        /// Determine whether the given string is DateTime. <br />
        /// 判断给定的字符串是否为 DateTime。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string str) => StringDateTimeDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is DateTimeOffset. <br />
        /// 判断给定的字符串是否为 DateTimeOffset。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTimeOffset(this string str) => StringDateTimeOffsetDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is TimeSpan. <br />
        /// 判断给定的字符串是否为 TimeSpan。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTimeSpan(this string str) => StringTimeSpanDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is DateTime. <br />
        /// 判断给定的字符串是否为 DateTime。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool IsDateTimeExact(this string str, string format) => StringDateTimeDeterminer.Exact.Is(str, format);

        /// <summary>
        /// Determine whether the given string is DateTimeOffset. <br />
        /// 判断给定的字符串是否为 DateTimeOffset。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool IsDateTimeOffsetExact(this string str, string format) => StringDateTimeOffsetDeterminer.Exact.Is(str, format);

        /// <summary>
        /// Determine whether the given string is TimeSpan. <br />
        /// 判断给定的字符串是否为 TimeSpan。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool IsTimeSpanExact(this string str, string format) => StringTimeSpanDeterminer.Exact.Is(str, format);

        #endregion

        #region Is Enum

        /// <summary>
        /// Determine whether the given string is an enumeration of the specified type.<br />
        /// 判断给定的字符串是否为指定类型的枚举。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static bool IsEnum(this string str, Type enumType) => StringEnumDeterminer.Is(str, enumType);

        /// <summary>
        /// Determine whether the given string is an enumeration of the specified type.<br />
        /// 判断给定的字符串是否为指定类型的枚举。
        /// </summary>
        /// <param name="str"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static bool IsEnum<TEnum>(this string str) where TEnum : struct => StringEnumDeterminer<TEnum>.Is(str);

        #endregion

        #region Is Guid

        private static readonly Regex GuidSchema
            = new Regex("^[A-Fa-f0-9]{32}$|" +
                        "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                        "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2},{0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");

        /// <summary>
        /// Determine whether the given string is Guid. <br />
        /// 判断给定的字符串是否为 Guid。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsGuid(this string str)
        {
            return StringGuidDeterminer.Is(str) && IsValidGuid(str);

            static bool IsValidGuid(string guidStr) =>
                !string.IsNullOrWhiteSpace(guidStr) && GuidSchema.Match(guidStr).Success;
        }

        /// <summary>
        /// Determine whether the given string is Guid. <br />
        /// 判断给定的字符串是否为 Guid。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool IsGuidExact(this string str, string format)
        {
            return StringGuidDeterminer.Exact.Is(str, format);
        }

        #endregion

        #region Is Numeric

        /// <summary>
        /// Determine whether the given string is a numeric value. <br />
        /// 判断给定的字符串是否是数值。
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string @string) => NumericJudge.IsNumeric(@string);

        /// <summary>
        /// Determine whether the given string is byte. <br />
        /// 判断给定的字符串是否是 byte。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsByte(this string str) => StringByteDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is sbyte. <br />
        /// 判断给定的字符串是否是 sbyte。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsSByte(this string str) => StringSByteDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is short. <br />
        /// 判断给定的字符串是否是 short。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsShort(this string str) => StringShortDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is int. <br />
        /// 判断给定的字符串是否是 int。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(this string str) => StringIntDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is long. <br />
        /// 判断给定的字符串是否是 long。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLong(this string str) => StringLongDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is ushort. <br />
        /// 判断给定的字符串是否是 ushort。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUShort(this string str) => StringUShortDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is uint. <br />
        /// 判断给定的字符串是否是 uint。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUInt(this string str) => StringUIntDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is ulong. <br />
        /// 判断给定的字符串是否是 ulong。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsULong(this string str) => StringULongDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is int16(short). <br />
        /// 判断给定的字符串是否是 int16(short)。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt16(this string str) => StringShortDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is int32(int). <br />
        /// 判断给定的字符串是否是 int32(int)。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt32(this string str) => StringIntDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is int64(long). <br />
        /// 判断给定的字符串是否是 int64(long)。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt64(this string str) => StringLongDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is uint16(ushort). <br />
        /// 判断给定的字符串是否是 uint16(ushort)。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUInt16(this string str) => StringUShortDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is uint32(uint). <br />
        /// 判断给定的字符串是否是 uint32(uint)。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUInt32(this string str) => StringUIntDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is uint64(ulong). <br />
        /// 判断给定的字符串是否是 uint64(ulong)。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool UsUInt64(this string str) => StringULongDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is float. <br />
        /// 判断给定的字符串是否是 float。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsFloat(this string str) => StringFloatDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is double. <br />
        /// 判断给定的字符串是否是 double。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(this string str) => StringDoubleDeterminer.Is(str);

        /// <summary>
        /// Determine whether the given string is decimal. <br />
        /// 判断给定的字符串是否是 decimal。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string str) => StringDecimalDeterminer.Is(str);

        #endregion
        
        #region Is WebUrl / Email

        /// <summary>
        /// Determine whether the given string is a URL path. <br />
        /// 判断给定的字符串是否是 Url 路径。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsWebUrl(this string target) => StringJudge.IsWebUrl(target);

        /// <summary>
        /// Determine whether the given string is an email address. <br />
        /// 判断给定的字符串是否是电子邮件路径。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsEmail(this string target) => StringJudge.IsEmail(target);

        #endregion
    }
}