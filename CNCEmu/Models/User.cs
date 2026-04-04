using CNCEmu.Enums;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace CNCEmu.Models
{
    /// <summary>
    /// Represents a player info in the game.
    /// reimpplement if 'PlayerInfo' class
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }

        public UserRole Role { get; set; }

        [Obsolete]
        public long UserId { get; set; }

        [Obsolete]
        public bool IsServer { get; set; }

        [Obsolete]
        public string Name { get; set; }

        public Game ActiveGame { get; set; } = default;

        public Stopwatch Timeout { get; set; }

        public NetworkStream NetworkStream { get; set; }

        public Account Profile { get; set; }

        public string Version { get; set; }

        public long ExIp { get; set; }

        public long ExPort { get; set; }

        public long InIp { get; set; }

        public long InPort { get; set; }

        public long Nat { get; set; } = 0;

        public long Loc { get; set; }

        public long Slot { get; set; }

        public long Stat { get; set; }

        public long Cntx { get; set; }
    }
}
