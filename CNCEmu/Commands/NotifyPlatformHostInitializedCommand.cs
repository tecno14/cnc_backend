﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class NotifyPlatformHostInitializedCommand
    {
        public static List<Blaze.Tdf> NotifyPlatformHostInitialized(PlayerInfo pi)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            Result.Add(Blaze.TdfInteger.Create("PHID", pi.UserId));
            Result.Add(Blaze.TdfInteger.Create("PHST", 0));
            return Result;
        }

    }
}
