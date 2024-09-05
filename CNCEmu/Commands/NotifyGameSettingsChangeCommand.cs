using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace CNCEmu
{
    class NotifyGameSettingsChangeCommand
    {
        public static List<Blaze.Tdf> NotifyGameSettingsChange(PlayerInfo pi)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("ATTR", pi.Game.GSET));
            Result.Add(Blaze.TdfInteger.Create("GID", pi.Game.id));
            return Result;
        }

    }
}
