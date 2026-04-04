using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    public static class NotifyGameStateChangeCommand
    {
        public static List<Tdf> NotifyGameStateChange(User pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.ActiveGame.id),
                TdfInteger.Create("GSTA", pi.ActiveGame.GSTA)
            };
            return Result;
        }
    }
}
