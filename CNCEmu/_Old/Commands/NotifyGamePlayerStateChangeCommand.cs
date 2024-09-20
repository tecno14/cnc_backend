using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    class NotifyGamePlayerStateChangeCommand
    {
        public static List<Tdf> NotifyGamePlayerStateChange(Player pi, long stat)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("PID\0", pi.UserId),
                TdfInteger.Create("STAT", stat)
            };
            return Result;
        }
    }
}
