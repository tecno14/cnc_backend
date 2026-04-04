using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    class NotifyGameSettingsChangeCommand
    {
        public static List<Tdf> NotifyGameSettingsChange(User pi)
        {
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("ATTR", pi.ActiveGame.GSET),
                TdfInteger.Create("GID", pi.ActiveGame.id)
            };
            return Result;
        }
    }
}
