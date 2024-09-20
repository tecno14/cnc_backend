namespace BlazeLibWV.Models
{
    public class TdfUnion : Tdf
    {
        public byte UnionType;
        public Tdf UnionContent;

        public static TdfUnion Create(string Label, byte unionType = 0x7F, Tdf data = null)
        {
            TdfUnion res = new TdfUnion();
            res.Set(Label, 6);
            res.UnionType = unionType;
            res.UnionContent = data;
            return res;
        }
    }
}
