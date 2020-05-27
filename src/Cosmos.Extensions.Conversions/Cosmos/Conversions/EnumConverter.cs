using System;
using Cosmos.Conversions.Core;

namespace Cosmos.Conversions
{
    /// <summary>
    /// Enum converter
    /// </summary>
    public static class EnumConverter
    {
        /// <summary>
        /// Convert <see cref="string"/> to TEnum.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultVal"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(string member, bool ignoreCase = false, TEnum defaultVal = default) where TEnum : struct, Enum
        {
            return EnumConv.StringToEnum(member, defaultVal, ignoreCase);
        }

        /// <summary>
        /// Convert <see cref="string"/> to the given type enum.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="enumType"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static object ToEnum(string member, Type enumType, bool ignoreCase = false, object defaultVal = default)
        {
            return EnumConv.StringToEnum(member, enumType, defaultVal, ignoreCase);
        }

        /// <summary>
        /// Convert <see cref="object"/> to TEnum.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="defaultVal"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(object member, TEnum defaultVal = default) where TEnum : struct, Enum
        {
            return EnumConv.ObjToEnum(member, defaultVal);
        }

        /// <summary>
        /// Convert <see cref="object"/> to the given type enum.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="enumType"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static object ToEnum(object member, Type enumType, object defaultVal = default)
        {
            return EnumConv.ObjToEnum(member, enumType, defaultVal);
        }
    }
}