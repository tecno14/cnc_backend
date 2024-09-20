using BlazeLibWV;
using BlazeLibWV.Models;
using CNCEmu.Models;
using System;
using System.Collections.Generic;

namespace CNCEmu
{
    class NotifyPlayerJoiningCommand
    {
        public static List<Tdf> NotifyPlayerJoining(Player pi)
        {
            uint t = Blaze.GetUnixTimeStamp();
            
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id)
            };

            List<Tdf> PDAT = new List<Tdf>
            {
                TdfInteger.Create("EXID", pi.UserId),
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("LOC\0", pi.Loc),
                TdfString.Create("NAME", pi.Profile.Name),
                TdfInteger.Create("PID\0", pi.UserId),
                BlazeHelper.CreateNETFieldUnion(pi, "PNET"),
                TdfInteger.Create("SID\0", pi.Slot),
                TdfInteger.Create("STAT", pi.Stat),
                TdfInteger.Create("TIDX", 0xFFFF),
                TdfInteger.Create("TIME", t),
                TdfInteger.Create("UID\0", pi.UserId)
            };
            
            Result.Add(TdfStruct.Create("PDAT", PDAT));

            return Result;
        }

    }
}
