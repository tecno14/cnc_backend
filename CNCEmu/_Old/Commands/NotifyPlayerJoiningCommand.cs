using BlazeLibWV;
using BlazeLibWV.Models;
using CNCEmu.Models;
using System;
using System.Collections.Generic;

namespace CNCEmu
{
    class NotifyPlayerJoiningCommand
    {
        public static List<Tdf> NotifyPlayerJoining(User pi)
        {
            uint t = Blaze.GetUnixTimeStamp();
            
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.ActiveGame.id)
            };

            List<Tdf> PDAT = new List<Tdf>
            {
                TdfInteger.Create("EXID", pi.UserId),
                TdfInteger.Create("GID\0", pi.ActiveGame.id),
                TdfInteger.Create("LOC\0", pi.Loc),
                TdfString.Create("NAME", pi.Profile.UserName),
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
