using BlazeLibWV;
using CNCEmu.Constants;
using CNCEmu.Models;
using CNCEmu.Services.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CNCEmu.Services.Network
{
    public class BlazeServer
    {
        private Task _ServerTask;
        private readonly object _ServerStatusLock = new object();
        private readonly object _UserIdLock = new object();
        private readonly Random _Random = new Random();
        private readonly List<PlayerInfo> _PlayersInfo = new List<PlayerInfo>();

        private CancellationTokenSource _TokenSource;

        public TimeSpan MaximumTimeout { get; private set; }

        public int MaximumPlayers { get; private set; }

        public int Port { get; private set; }

        public IPAddress LocalAddress { get; private set; }

        public LogService Logger { get; private set; } = new LogService(nameof(BlazeServer));

        public bool IsRunning => _ServerTask != default && _ServerTask.Status == TaskStatus.Running;

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static BlazeServer Instance { get; private set; } = new BlazeServer();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maximumTimeout"></param>
        /// <param name="maximumPlayers"></param>
        /// <param name="port"></param>
        public BlazeServer(
            TimeSpan maximumTimeout = default,
            int maximumPlayers = -1,
            int port = -1,
            IPAddress localAddress = null)
        {
            MaximumTimeout = maximumTimeout != default ? maximumTimeout : General.BlazerMaximumTimeout;
            MaximumPlayers = maximumPlayers > 0 ? maximumPlayers : General.BlazerMaximumPlayers;
            Port = port > 0 ? port : ProviderInfo.BlazePort;
            LocalAddress = localAddress ?? IPAddress.Parse(ProviderInfo.BackendIP);
        }

        /// <summary>
        /// Start the server
        /// </summary>
        public void Start()
        {
            lock (_ServerStatusLock)
            {
                if (_TokenSource != default)
                {
                    Logger.Write(nameof(Start), "Server is already running", LogType.WARN);
                    return;
                }

                _TokenSource = new CancellationTokenSource();
                _ServerTask = Task.Run(Server);
            }
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void Stop()
        {
            lock (_ServerStatusLock)
            {
                Logger.Write(nameof(Stop), "Stopping the server...", LogType.WARN);

                if (_TokenSource == null)
                {
                    Logger.Write(nameof(Stop), "Server is not running", LogType.WARN);
                    return;
                }

                // Stop the server
                _TokenSource.Cancel();

                // Wait for the server to stop
                _ServerTask.Wait();

                // Reset the token
                _TokenSource = default;
                Logger.Write(nameof(Stop), "Server stopped", LogType.WARN);
            }
        }

        /// <summary>
        /// Main server code
        /// </summary>
        /// <returns></returns>
        private async Task Server()
        {
            try
            {
                Logger.Write(nameof(Server), $"Starting the server ... (port: {Port})", LogType.WARN);

                var server = new TcpListener(LocalAddress, Port);
                server.Start();
                server.BeginAcceptTcpClient(
                    new AsyncCallback(ClientHandler),
                    new State() { Server = server, Token = _TokenSource.Token });

                Logger.Write(nameof(Server), "Server is listening for connections...", LogType.WARN);

                // Wait forever until cancelation
                await Task.Delay(Timeout.Infinite, _TokenSource.Token);

                // Shutdown the server
                Logger.Write(nameof(Server), "Server stopping ...", LogType.WARN);
                server.Stop();
                Logger.Write(nameof(Server), "Server stopped, reason: server token was cancelled", LogType.WARN);
            }
            catch (Exception ex)
            {
                // Ignore the exception if the token is cancelled
                if (_TokenSource.IsCancellationRequested)
                    return;
                Logger.Write(nameof(Server), "Server stopped due to an error", LogType.WARN);
                Logger.Write(nameof(Server), ex);
            }
            finally
            {
                // Reset the token
                _TokenSource = default;
                Logger.Write(nameof(Server), "Server stopped", LogType.WARN);
            }
        }

        /// <summary>
        /// Get a valid new user id
        /// </summary>
        /// <returns></returns>
        private int GetValidNewUserId()
        {
            lock (_UserIdLock)
                while (true)
                {
                    int userId = _Random.Next(1, MaximumPlayers + 1);
                    if (!_PlayersInfo.Exists(x => x.UserId == userId))
                        return userId;
                }
        }

        /// <summary>
        /// Callback for when client connects
        /// </summary>
        /// <param name="ar">State of client connection</param>
        /// <exception cref="Exception"></exception>
        private void ClientHandler(IAsyncResult ar)
        {
            var playerInfo = default(PlayerInfo);
            try
            {
                var state = (State)ar.AsyncState;
                var server = state.Server;
                var client = server.EndAcceptTcpClient(ar);
                var ns = client.GetStream();
                playerInfo = new PlayerInfo
                {
                    UserId = GetValidNewUserId(),
                    NetworkStream = ns,
                    ExIp = 0, // todo: is this supposed to be client ip??                
                    Timeout = Stopwatch.StartNew(),
                };
                
                // Add player to players list
                _PlayersInfo.Add(playerInfo);

                Logger.Write(nameof(ClientHandler), $"Player #{playerInfo.UserId} connected");

                // todo: siperate timeout checking from packet processing may improve performance if processing code get improved (as I think !!)
                while (!state.Token.IsCancellationRequested)
                {
                    // Check Timeout
                    if (playerInfo.Timeout.Elapsed > MaximumTimeout)
                        throw new Exception("Client timed out!");
                    playerInfo.Timeout.Restart();

                    // Process packets
                    ProcessPackets(Helper.ReadContentTCP(ns), playerInfo, ns);
                }

                client.Close();
                Logger.Write(nameof(ClientHandler), $"Player #{playerInfo.UserId} disconnected, reason: Server stopped");
            }
            catch (Exception ex)
            {
                Logger.Write(nameof(ClientHandler), $"Player #{playerInfo?.UserId} disconnected, reason: due to an error", LogType.WARN);
                Logger.Write(nameof(ClientHandler), ex);
            }
            finally
            {
                // Remove player from players list
                if (playerInfo != null && _PlayersInfo.Contains(playerInfo))
                    _PlayersInfo.Remove(playerInfo);
            }
        }

        /// <summary>
        /// Process packets sent from client connection
        /// </summary>
        /// <param name="data"></param>
        /// <param name="playerInfo"></param>
        /// <param name="ns"></param>
        public void ProcessPackets(byte[] data, PlayerInfo playerInfo, NetworkStream ns)
        {
            if (data == null || data.Length == 0)
                return;

            if (Config.MakePacket.ToLower() == "true")
                CNCEmu.Logger.LogPacket("CLNT", Convert.ToInt32(playerInfo.UserId), data);

            // Process all packets
            foreach (var packet in Blaze.FetchAllBlazePackets(new MemoryStream(data)))
            {
                Logger.Write(nameof(ProcessPackets), $"Player #{playerInfo.UserId} sent: {Blaze.PacketToDescriber(packet)}");

                switch (packet.Component)
                {
                    case 0x1:
                        AuthenticationComponent.HandlePacket(packet, playerInfo, ns);
                        break;
                    case 0x4:
                        GameManagerComponent.HandlePacket(packet, playerInfo, ns);
                        break;
                    case 0x7:
                        StatsComponent.HandlePacket(packet, playerInfo, ns);
                        break;
                    case 0x9:
                        UtilComponent.HandlePacket(packet, playerInfo, ns);
                        break;
                    case 0x19:
                        AssociationListsComponent.HandlePacket(packet, playerInfo, ns);
                        break;
                    case 0x23:
                        AccountsComponent.HandlePacket(packet, playerInfo, ns);
                        break;
                    case 0x801:
                        RSPComponent.HandlePacket(packet, playerInfo, ns);
                        break;
                    case 0x803:
                        InventoryComponent.HandlePacket(packet, playerInfo, ns);
                        break;
                    case 0x7802:
                        UserSessionsComponent.HandlePacket(packet, playerInfo, ns);
                        break;
                    default:
                        Logger.Write(nameof(ProcessPackets), $"Player #{playerInfo.UserId} sent unknown packet component: ({packet.Component})", LogType.WARN);
                        break;
                }
            }
        }

        /// <summary>
        /// Temporal logging function to be compatible with old use
        /// Todo: replace all what use this
        /// </summary>
        /// <param name="s"></param>
        /// <param name="color"></param>
        [Obsolete]
        public static void Log(string s, Color? color = default)
        {
            BlazeServer.Instance.Logger.Write(s, color == default ? LogType.INFO : color == Color.Red ? LogType.ERRR : LogType.WARN);
        }

        /// <summary>
        /// Temporal logging function to be compatible with old use
        /// Todo: replace all what use this
        /// </summary>
        /// <param name="who"></param>
        /// <param name="e"></param>
        /// <param name="cName"></param>
        [Obsolete]
        public static void LogError(string who, Exception e)
        {
            BlazeServer.Instance.Logger.Write(who, e);
        }

        [Obsolete]
        public static void RemovePlayer(long userId)
        {
            BlazeServer.Instance._PlayersInfo.RemoveAll(p => p.UserId == userId);
        }

        [Obsolete]
        public static PlayerInfo GetServerInfo()
        {
            return BlazeServer.Instance._PlayersInfo.FirstOrDefault(p => p.IsServer);
        }

        [Obsolete]
        public static PlayerInfo GetPlayerById(long userId)
        {
            return BlazeServer.Instance._PlayersInfo.FirstOrDefault(p => p.UserId == userId);
        }

        [Obsolete]
        public static List<PlayerInfo> GetPlayers()
        {
            return BlazeServer.Instance._PlayersInfo;
        }
    }
}
