using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    class NotifyUserRemovedCommand
    {
        public static List<Tdf> NotifyUserRemoved(Player pi, long pid)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("BUID", pid)
            };
            return Result;
        }

    }
}
