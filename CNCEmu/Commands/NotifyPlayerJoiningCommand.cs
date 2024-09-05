using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace CNCEmu
{
    class NotifyPlayerJoiningCommand
    {
        public static List<Blaze.Tdf> NotifyPlayerJoining(PlayerInfo pi)
        {
            uint t = Blaze.GetUnixTimeStamp();
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            List<Blaze.Tdf> PDAT = new List<Blaze.Tdf>();
            PDAT.Add(Blaze.TdfInteger.Create("EXID", pi.UserId));
            PDAT.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            PDAT.Add(Blaze.TdfInteger.Create("LOC\0", pi.Loc));
            PDAT.Add(Blaze.TdfString.Create("NAME", pi.Profile.Name));
            PDAT.Add(Blaze.TdfInteger.Create("PID\0", pi.UserId));
            PDAT.Add(BlazeHelper.CreateNETFieldUnion(pi, "PNET"));
            PDAT.Add(Blaze.TdfInteger.Create("SID\0", pi.Slot));
            PDAT.Add(Blaze.TdfInteger.Create("STAT", pi.Stat));
            PDAT.Add(Blaze.TdfInteger.Create("TIDX", 0xFFFF));
            PDAT.Add(Blaze.TdfInteger.Create("TIME", t));
            PDAT.Add(Blaze.TdfInteger.Create("UID\0", pi.UserId));
            Result.Add(Blaze.TdfStruct.Create("PDAT", PDAT));

            return Result;
        }

    }
}
