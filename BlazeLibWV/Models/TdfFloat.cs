namespace BlazeLibWV.Models
{
    public class TdfFloat : Tdf
    {
        public float Value;

        public static TdfFloat Create(string Label, float value)
        {
            TdfFloat res = new TdfFloat();
            res.Set(Label, 0xA);
            res.Value = value;
            return res;
        }
    }
}
