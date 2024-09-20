using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    public static class NotifyGameStateChangeCommand
    {
        public static List<Tdf> NotifyGameStateChange(Player pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("GSTA", pi.Game.GSTA)
            };
            return Result;
        }
    }
}
