using System.Net.Sockets;
using System.Threading;

namespace CNCEmu.Structs
{
    /// <summary>
    /// Dummy class type for server when a new client connect
    /// </summary>
    public struct State
    {
        public TcpListener Server { get; set; }

        public CancellationToken Token { get; set; }
    }
}
