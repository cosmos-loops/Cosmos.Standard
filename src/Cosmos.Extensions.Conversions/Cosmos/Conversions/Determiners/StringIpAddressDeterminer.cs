using System;
using System.Collections.Generic;
using System.Net;
using Cosmos.Conversions.Core;

namespace Cosmos.Conversions.Determiners
{
    /// <summary>
    /// Internal core conversion helper from string to IpAddress
    /// </summary>
    internal static class StringIpAddressDeterminer
    {
        /// <summary>
        /// Is
        /// </summary>
        /// <param name="str"></param>
        /// <param name="addressAct"></param>
        /// <returns></returns>
        public static bool Is(string str, Action<IPAddress> addressAct = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            var result = IPAddress.TryParse(str, out var address);
            if (result)
                addressAct?.Invoke(address);
            return result;
        }

        /// <summary>
        /// Is
        /// </summary>
        /// <param name="str"></param>
        /// <param name="tries"></param>
        /// <param name="addressAct"></param>
        /// <returns></returns>
        public static bool Is(string str, IEnumerable<IConversionTry<string, IPAddress>> tries, Action<IPAddress> addressAct = null) =>
            ValueDeterminer.IsXXX(str, string.IsNullOrWhiteSpace, Is, tries, addressAct);

        /// <summary>
        /// To
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static IPAddress To(string str, IPAddress defaultVal = default)
        {
            if (string.IsNullOrWhiteSpace(str))
                return defaultVal;
            return IPAddress.TryParse(str, out var address) ? address : defaultVal;
        }

        /// <summary>
        /// To
        /// </summary>
        /// <param name="str"></param>
        /// <param name="impls"></param>
        /// <returns></returns>
        public static IPAddress To(string str, IEnumerable<IConversionImpl<string, IPAddress>> impls) =>
            ValueConverter.ToXxx(str, Is, impls);
    }
}