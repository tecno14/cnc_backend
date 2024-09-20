using CNCEmu.Interfaces;
using CNCEmu.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CNCEmu.Tcp
{
    /// <summary>
    /// Listens for incoming TCP connections.
    /// </summary>
    public class TcpServer
    {
        private readonly TcpListener _listener;
        private readonly ITcpConnectionService _connectionService;
        private readonly Dictionary<TcpClient, DateTime> _activeConnections = new Dictionary<TcpClient, DateTime>();

        public TcpServer(IPAddress ipAddress, int port, ITcpConnectionService connectionService)
        {
            _listener = new TcpListener(ipAddress, port);
            _connectionService = connectionService;
        }

        public void Start()
        {
            _listener.Start();
            Console.WriteLine("Server started...");
            while (true)
            {
                var client = _listener.AcceptTcpClient();
                var clientHandler = new TcpClientHandler(client, _connectionService, _activeConnections);
                Task.Run(() => clientHandler.HandleClientAsync());
            }
        }
    }
}
