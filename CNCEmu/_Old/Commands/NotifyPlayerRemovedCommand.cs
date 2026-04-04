using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    class NotifyPlayerRemovedCommand
    {
        public static List<Tdf> NotifyPlayerRemoved(User pi, long pid, long cntx, long reas)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("CNTX", cntx),
                TdfInteger.Create("GID\0", pi.ActiveGame.id),
                TdfInteger.Create("PID\0", pid),
                TdfInteger.Create("REAS", reas)
            };
            return Result;
        }
    }
}
