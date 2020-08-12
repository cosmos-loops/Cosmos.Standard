﻿namespace Cosmos
{
    /// <summary>
    /// Float arguments checking extensions
    /// </summary>
    public static partial class CosmosPreconditionsExtensions
    {
        /// <summary>
        /// 检查单精度浮点数是否超界
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        public static void CheckOutOfRange(this float argument, float min, float max, string argumentName, string message = null)
            => Checker.IsNotOutOfRange(argument, min, max, argumentName, message);

        /// <summary>
        /// 检查单精度浮点数是否超界
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        public static void CheckOutOfRange(this float? argument, float min, float max, string argumentName, string message = null)
            => Checker.IsNotOutOfRange(argument, min, max, argumentName, message);

        /// <summary>
        /// 检查浮点数是否为负
        /// <para> 如果为负则抛出 ArgumentOutOfRangeException </para>
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        public static void CheckNegative(this float argument, string argumentName, string message = null)
            => Checker.IsNotNegative(argument, argumentName, message);

        /// <summary>
        /// 检查浮点数是否为负
        /// <para> 如果为负则抛出 ArgumentOutOfRangeException </para>
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        public static void CheckNegative(this float? argument, string argumentName, string message = null)
            => Checker.IsNotNegative(argument, argumentName, message);

        /// <summary>
        /// 检查浮点数是否为负或为零
        /// <para> 如果为负或为零则抛出 ArgumentOutOfRangeException </para>
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        public static void CheckNegativeOrZero(this float argument, string argumentName, string message = null)
            => Checker.IsNotNegativeOrZero(argument, argumentName, message);

        /// <summary>
        /// 检查浮点数是否为负或为零
        /// <para> 如果为负或为零则抛出ArgumentOutOfRangeException</para>
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        public static void CheckNegativeOrZero(this float? argument, string argumentName, string message = null)
            => Checker.IsNotNegativeOrZero(argument, argumentName, message);
    }
}