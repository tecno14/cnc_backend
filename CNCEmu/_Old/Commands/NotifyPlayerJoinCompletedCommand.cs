using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;


namespace CNCEmu
{
    class NotifyPlayerJoinCompletedCommand
    {
        public static List<Tdf> NotifyPlayerJoinCompleted(Player pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("PID\0", pi.UserId)
            };
            return Result;
        }
    }
}
