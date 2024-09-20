namespace BlazeLibWV.Models
{
    public class TdfBlob : Tdf
    {
        public byte[] Data;

        public static TdfBlob Create(string Label, byte[] data = null)
        {
            TdfBlob res = new TdfBlob();
            res.Set(Label, 2);
            if (data == null)
                res.Data = new byte[0];
            else
                res.Data = data;
            return res;
        }
    }
}
