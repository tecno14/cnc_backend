namespace BlazeLibWV.Models
{
    public class TdfList : Tdf
    {
        public byte SubType;
        public int Count;
        public object List;

        public static TdfList Create(string Label, byte subtype, int count, object list)
        {
            TdfList res = new TdfList();
            res.Set(Label, 4);
            res.SubType = subtype;
            res.Count = count;
            res.List = list;
            return res;
        }
    }
}
