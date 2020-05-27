﻿using System.Linq;
using System.Text;

namespace Cosmos.Conversions.Scale
{
    /// <summary>
    /// Bytes Conversion Utilities
    /// </summary>
    public static class ByteConverter
    {
        /// <summary>
        /// Convert from bytes to ASCII <see cref="string"/>.
        /// </summary>
        /// <example>in: new byte[] {65, 66, 67}; out: ABC</example>
        /// <param name="bytes"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string ToASCII(byte[] bytes) => Encoding.ASCII.GetString(bytes, 0, bytes.Length);

        /// <summary>
        /// Convert from byte to binary.
        /// </summary>
        /// <example>in: (byte)128; out: 10000000</example>
        /// <param name="byte"></param>
        /// <returns></returns>
        public static string ToBinary(byte @byte) => DecimalismConverter.ToBinary(@byte);

        /// <summary>
        /// Convert high address 'h' and low address 'l' to decimalism.
        /// </summary>
        /// <example>in: (byte)65, (byte)66; out: 16706</example>
        /// <example>in: (byte)66, (byte)65; out: 16961</example>
        /// <param name="h">high address byte</param>
        /// <param name="l">low address byte</param>
        /// <returns></returns>
        public static int ToDecimalism(byte h, byte l) => h << 8 | l;

        /// <summary>
        /// Convert high address 'h' and low address 'l' to decimalism.
        /// </summary>
        /// <example>in: (byte)255, (byte)66; out: 65346</example>
        /// <example>in: (byte)66, (byte)255; out: -190</example>
        /// <param name="h">high address byte</param>
        /// <param name="l">low address byte</param>
        /// <param name="isRadix"></param>
        /// <returns></returns>
        public static int ToDecimalism(byte h, byte l, bool isRadix)
        {
            var v = (ushort) (h << 0 | l);

            if (isRadix && h > 127)
            {
                v = (ushort) ~v;
                v = (ushort) (v + 1);
                return -1 * v;
            }

            return v;
        }

        /// <summary>
        /// Convert from byte to hexadecimal.
        /// </summary>
        /// <example>in: (byte)128; out: 80</example>
        /// <param name="byte"></param>
        /// <returns></returns>
        public static string ToHexadecimal(byte @byte) => @byte.ToString("X2");

        /// <summary>
        /// Convert from bytes to hexadecimal.
        /// </summary>
        /// <example>in: new byte[] {65, 66, 67}; out: 414243</example>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexadecimal(byte[] bytes)
        {
            var ret = "";
            if (bytes != null)
            {
                ret = bytes.Aggregate(ret, (current, t) => $"{current}{t:X2}");
            }

            return ret;
        }

        /// <summary>
        /// Convert from high address 'h' and low address 'l' to hexadecimal.
        /// </summary>
        /// <example>in: (byte)65, (byte)66; out: 4142</example>
        /// <example>in: (byte)66, (byte)65; out: 4241</example>
        /// <param name="h">high address byte</param>
        /// <param name="l">low address byte</param>
        /// <returns></returns>
        public static string ToHexadecimal(byte h, byte l) => $"{ToHexadecimal(h)}{ToHexadecimal(l)}";
    }
}