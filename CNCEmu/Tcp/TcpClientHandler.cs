using CNCEmu.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CNCEmu.Tcp
{
    /// <summary>
    /// Manages individual client connections and handles communication.
    /// </summary>
    public class TcpClientHandler
    {
        private readonly TcpClient _client;
        private readonly ITcpConnectionService _connectionService;
        private readonly Dictionary<TcpClient, DateTime> _activeConnections;

        public TcpClientHandler(TcpClient client, ITcpConnectionService connectionService, Dictionary<TcpClient, DateTime> activeConnections)
        {
            _client = client;
            _connectionService = connectionService;
            _activeConnections = activeConnections;
        }

        public async Task HandleClientAsync()
        {
            using (var stream = _client.GetStream())
            {
                //stream.ReadTimeout = 30000; // 30 seconds timeout
                //stream.WriteTimeout = 30000; // 30 seconds timeout
                var buffer = new byte[1024];
                int bytesRead;

                try
                {
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        _activeConnections[_client] = DateTime.Now; // Update last activity time
                        await _connectionService.ProcessMessageAsync(message, _client);
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Client connection error: " + ex.Message);
                }
                finally
                {
                    _activeConnections.Remove(_client);
                    _client.Close();
                }
            }
        }
    }
}
