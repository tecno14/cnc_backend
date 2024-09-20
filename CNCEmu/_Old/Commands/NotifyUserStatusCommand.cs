using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;


namespace CNCEmu
{
    class NotifyUserStatusCommand
    {
        public static List<Tdf> NotifyUserStatus(Player pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("FLGS", 3),
                TdfInteger.Create("ID\0\0", pi.UserId)
            };
            return Result;
        }
    }
}
