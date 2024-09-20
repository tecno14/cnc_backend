using System.Windows.Forms;

namespace BlazeLibWV.Models
{
    public class Tdf
    {
        public string Label;
        public uint Tag;
        public byte Type;
        
        public TreeNode ToTree()
        {
            string typedescription = GetTypeDesc();
            return new TreeNode(Label + " : " + Type + " (" + typedescription + ")");
        }

        public string GetTypeDesc()
        {
            switch (Type)
            {
                case 0: return "TdfInteger";
                case 1: return "TdfString";
                case 2: return "TdfBlob";
                case 3: return "TdfStruct";
                case 4: return "TdfList";
                case 5: return "TdfDoubleList";
                case 6: return "TdfUnion";
                case 7: return "TdfIntegerList";
                case 8: return "TdfDoubleVal";
                case 9: return "TdfTrippleVal";
                case 0xA: return "TdfFloat";
                default: return "TdfUnknown";
            }
        }
        
        public void Set(string label, byte type)
        {
            Label = label;
            Type = type;
            Tag = 0;
            byte[] buff = Blaze.Label2Tag(label);
            Tag |= (uint)(buff[0] << 24);
            Tag |= (uint)(buff[1] << 16);
            Tag |= (uint)(buff[2] << 8);
        }
    }
}
