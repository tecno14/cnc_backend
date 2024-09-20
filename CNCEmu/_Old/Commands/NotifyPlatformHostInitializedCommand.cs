using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    public static class NotifyPlatformHostInitializedCommand
    {
        public static List<Tdf> NotifyPlatformHostInitialized(Player pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("PHID", pi.UserId),
                TdfInteger.Create("PHST", 0)
            };
            return Result;
        }

    }
}
