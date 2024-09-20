using BlazeLibWV.Struct;

namespace BlazeLibWV.Models
{
    public class TdfDoubleVal : Tdf
    {
        public DoubleVal Value;

        public static TdfDoubleVal Create(string Label, DoubleVal v)
        {
            TdfDoubleVal res = new TdfDoubleVal();
            res.Set(Label, 8);
            res.Value = v;
            return res;
        }
    }
}
