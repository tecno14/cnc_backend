using BlazeLibWV.Struct;

namespace BlazeLibWV.Models
{
    public class TdfTrippleVal : Tdf
    {
        public TrippleVal Value;

        public static TdfTrippleVal Create(string Label, TrippleVal v)
        {
            TdfTrippleVal res = new TdfTrippleVal();
            res.Set(Label, 9);
            res.Value = v;
            return res;
        }
    }
}
