using System.Linq;
using System.Text;

namespace CNCEmu.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Convert a byte array to a hex string
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(this byte[] buff) =>
           string.Concat(buff.Select(b => b.ToString("X2")));

        /// <summary>
        /// Convert a byte array to a label
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string BytesToLabel(this byte[] data)
        {
            byte[] result = new byte[4];
            result[3] = (byte)((data[2] & 0x3F) + 0x20);
            result[2] = (byte)(((data[2] >> 6) | ((data[1] & 0x0F) << 2)) + 0x20);
            result[1] = (byte)(((data[1] >> 4) | ((data[0] & 0x03) << 4)) + 0x20);
            result[0] = (byte)((data[0] >> 2) + 0x20);
            return Encoding.UTF8.GetString(result);
        }
    }
}
