namespace BlazeLibWV.Models
{
    public class TdfString : Tdf
    {
        public string Value;

        public static TdfString Create(string Label, string value)
        {
            TdfString res = new TdfString();
            res.Set(Label, 1);
            res.Value = value;
            return res;
        }
    }
}
