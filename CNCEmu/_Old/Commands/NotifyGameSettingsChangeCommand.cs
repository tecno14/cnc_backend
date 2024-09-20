using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    class NotifyGameSettingsChangeCommand
    {
        public static List<Tdf> NotifyGameSettingsChange(Player pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("ATTR", pi.Game.GSET),
                TdfInteger.Create("GID", pi.Game.id)
            };
            return Result;
        }
    }
}
