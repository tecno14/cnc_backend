using System.Collections.Generic;

namespace BlazeLibWV.Models
{
    public class TdfStruct : Tdf
    {
        public List<Tdf> Values;
        public bool startswith2;

        public static TdfStruct Create(string Label, List<Tdf> list, bool start2 = false)
        {
            TdfStruct res = new TdfStruct
            {
                startswith2 = start2
            };
            res.Set(Label, 3);
            res.Values = list;
            return res;
        }
    }
}
