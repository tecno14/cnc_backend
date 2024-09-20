using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    public static class UserAuthenticatedCommand
    {
        public static List<Tdf> UserAuthenticated(Player pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("ALOC", 1403663841),
                TdfInteger.Create("BUID", pi.UserId),
                TdfString.Create("DSNM", pi.Profile.Name),
                TdfInteger.Create("FRSC", 0),
                TdfInteger.Create("FRST", 0),
                TdfString.Create("KEY", "SESSKY"),
                TdfInteger.Create("LAST", 1403663841),
                TdfInteger.Create("LLOG", 1403663841),
                TdfString.Create("MAIL", "cnc.server.pc@ea.com"),
                TdfInteger.Create("PID", pi.UserId),
                TdfInteger.Create("PLAT", 4),
                TdfInteger.Create("UID", pi.UserId),
                TdfInteger.Create("USTP", 0),
                TdfInteger.Create("XREF", 0)
            };

            return Result;
        }

    }
}
