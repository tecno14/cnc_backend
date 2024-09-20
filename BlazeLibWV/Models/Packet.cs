namespace BlazeLibWV.Models
{
    public class Packet
    {
        public ushort Length;
        public ushort Component;
        public ushort Command;
        public ushort Error;
        public ushort QType;
        public ushort ID;
        public ushort extLength;
        public byte[] Content;
    }
}
