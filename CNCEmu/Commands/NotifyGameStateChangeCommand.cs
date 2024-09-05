using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class NotifyGameStateChangeCommand
    {
        public static List<Blaze.Tdf> NotifyGameStateChange(PlayerInfo pi)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            Result.Add(Blaze.TdfInteger.Create("GSTA", pi.Game.GSTA));
            return Result;
        }
    }
}
