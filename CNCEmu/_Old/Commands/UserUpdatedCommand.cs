using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    public static class UserUpdatedCommand
    {
        public static List<Tdf> UserUpdated(Player pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("FLGS", 3),
                TdfInteger.Create("ID", pi.UserId)
            };

            return Result;
        }

    }
}
