using CNCEmu.Models;
using System.Diagnostics;
using System.Net.Sockets;

namespace CNCEmu
{
    public class PlayerInfo
    {
        public GameInfo Game { get; set; }

        public NetworkStream NetworkStream { get; set; }
        
        public Profile Profile { get; set; }
        
        public Stopwatch Timeout { get; set; }

        public bool IsServer { get; set; }

        public string Version { get; set; }

        public long UserId { get; set; }

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
