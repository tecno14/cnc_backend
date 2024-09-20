using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    class NotifyPlayerRemovedCommand
    {
        public static List<Tdf> NotifyPlayerRemoved(Player pi, long pid, long cntx, long reas)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("CNTX", cntx),
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("PID\0", pid),
                TdfInteger.Create("REAS", reas)
            };
            return Result;
        }
    }
}
