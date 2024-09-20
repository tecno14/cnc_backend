using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CNCEmu.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Convert a hex string to a byte array
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(this string hex) =>
            Enumerable
                .Range(0, hex.Length / 2)
                .Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16))
                .ToArray();

        /// <summary>
        /// Convert a label to a byte array
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static byte[] LabelToBytes(this string label)
        {
            var buff = Encoding.UTF8.GetBytes(label)
                .Select(b => (byte)(b - 0x20))
                .ToArray();
            return new byte[]
            {
                (byte)(((buff[0] & 0x3F) << 2) | ((buff[1] & 0x30) >> 4)),
                (byte)(((buff[1] & 0x0F) << 4) | ((buff[2] & 0x3C) >> 2)),
                (byte)(((buff[2] & 0x03) << 6) | (buff[3] & 0x3F))
            };
        }

        /// <summary>
        /// Converts a string containing data enclosed in curly braces into a list of trimmed strings.
        /// Removes '{' characters and splits the input by '}', then adds non-empty, trimmed elements to the list.
        /// </summary>
        /// <param name="data">The input string containing data separated by curly braces.</param>
        /// <returns>A list of non-empty, trimmed strings from the input.</returns>
        public static List<string> ConvertToStringList(this string data) =>
            data.Replace("{", "")
                .Split('}')
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Trim())
                .ToList();

        /// <summary>
        /// Converts a string containing data enclosed in curly braces into two lists of trimmed strings.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        public static void ConvertDoubleStringList(this string data, out List<string> list1, out List<string> list2)
        {
            var res1 = new List<string>();
            var res2 = new List<string>();
            foreach (var line in data.Replace("{", "").Split('}').Where(line => !string.IsNullOrWhiteSpace(line)))
            {
                var parts = line.Trim().Split(';');
                res1.Add(parts[0].Trim());
                res2.Add(parts[1].Trim());
            }
            list1 = res1; list2 = res2;
        }
    }
}
