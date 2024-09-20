namespace BlazeLibWV.Models
{
    public class TdfDoubleList : Tdf
    {
        public byte SubType1;
        public byte SubType2;
        public int Count;
        public object List1;
        public object List2;

        public static TdfDoubleList Create(string Label, byte subtype1, byte subtype2, object list1, object list2, int count)
        {
            var res = new TdfDoubleList();
            res.Set(Label, 5);
            res.SubType1 = subtype1;
            res.SubType2 = subtype2;
            res.List1 = list1;
            res.List2 = list2;
            res.Count = count;
            return res;
        }
    }
}
