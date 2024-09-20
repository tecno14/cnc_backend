using System.Collections.Generic;

namespace BlazeLibWV.Models
{
    public class TdfIntegerList : Tdf
    {
        public int Count;
        public List<long> List;

        public static TdfIntegerList Create(string Label, int count, List<long> list)
        {
            TdfIntegerList res = new TdfIntegerList();
            res.Set(Label, 7);
            res.Count = count;
            res.List = list;
            return res;
        }
    }
}
