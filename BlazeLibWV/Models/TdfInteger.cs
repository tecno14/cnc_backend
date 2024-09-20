namespace BlazeLibWV.Models
{
    public class TdfInteger : Tdf
    {
        public long Value;

        public static TdfInteger Create(string Label, long value)
        {
            TdfInteger res = new TdfInteger();
            res.Set(Label, 0);
            res.Value = value;
            return res;
        }
    }
}
