using System.IO;
using System.Net.Security;
using System.Net.Sockets;

namespace CNCEmu.Extensions
{
    public static class SteamExtensions
    {
        /// <summary>
        /// Read a compressed integer
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ReadCompressedInteger(this Stream s)
        {
            long result = s.ReadByte() & 0x3F;
            int shift = 6;
            for (byte b; (b = (byte)s.ReadByte()) >= 0x80; shift += 7)
                result |= (long)(b & 0x7F) << shift;
            return result;
        }

        /// <summary>
        /// Write a compressed integer
        /// </summary>
        /// <param name="s"></param>
        /// <param name="l"></param>
        public static void WriteCompressedInteger(this Stream s, long l)
        {
            s.WriteByte(l < 0x40 ? (byte)l : (byte)((l & 0x3F) | 0x80));
            for (l >>= 6; l >= 0x80; l >>= 7)
                s.WriteByte((byte)((l & 0x7F) | 0x80));
            if (l > 0) s.WriteByte((byte)l);
        }

        /// <summary>
        /// Read content from an SSL stream
        /// </summary>
        /// <param name="sslStream"></param>
        /// <returns></returns>
        public static byte[] ReadContentSSL(this SslStream sslStream)
        {
            using (var res = new MemoryStream())
            {
                byte[] buff = new byte[0x10000];
                sslStream.ReadTimeout = 100;
                try { while (sslStream.Read(buff, 0, buff.Length) > 0) res.Write(buff, 0, buff.Length); } catch { }
                sslStream.Flush();
                return res.ToArray();
            }
        }

        /// <summary>
        /// Read content from a TCP stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadContentTCP(this NetworkStream stream)
        {
            using (var res = new MemoryStream())
            {
                byte[] buff = new byte[0x10000];
                stream.ReadTimeout = 100;
                try { while (stream.Read(buff, 0, buff.Length) > 0) res.Write(buff, 0, buff.Length); } catch { }
                stream.Flush();
                return res.ToArray();
            }
        }
    }
}
