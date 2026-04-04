using System;

namespace CNCEmu.Extensions
{
    public static class GuidExtensions
    {
        public static long GuidToLong(Guid guid)
        {
            byte[] bytes = guid.ToByteArray();
            long value = BitConverter.ToInt64(bytes, 0); // Use the first 8 bytes
            return value;
        }
    }
}
