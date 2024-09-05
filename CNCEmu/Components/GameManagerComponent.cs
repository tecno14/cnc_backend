using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;
using CNCEmu.Services.Network;

namespace CNCEmu
{
    public static class GameManagerComponent
    {
        public static void HandlePacket(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x00:
                    break;
                case 0x1:
                    CreateGame(p, pi, ns);
                    break;
                case 0x2:
                    DestroyGame(p, pi, ns);
                    break;
                case 0x3:
                    AdvanceGameState(p, pi, ns);
                    break;
                case 0x5:
                    SetPlayerCapacity(p, pi, ns);
                    break;
                case 0x7:
                    SetGameAttributes(p, pi, ns);
                    break;
                case 0x9:
                    JoinGame(p, pi, ns);
                    break;
                case 0xB:
                    RemovePlayer(p, pi, ns);
                    break;
                case 0xD:
                    StartMatchmaking(p, pi, ns);
                    break;
                case 0xf:
                    FinalizeGameCreation(p, pi, ns);
                    break;
                case 0x13:
                    ReplayGame(p, pi, ns);
                    break;
                case 0x1d:
                    UpdateMeshConnection(p, pi, ns);
                    break;
                case 0x67:
                    GetFullGameData(p, pi, ns);
                    break;
                case 0x6C:
                    SetPlayerTeam(p, pi, ns);
                    break;
                case 0x29:
                    SetGameModRegister(p, pi, ns);
                    break;
                case 0x19:
                    resetDedicatedServer(p, pi, ns);
                    break;
                case 0x64:
                    getGameListSnapshot(p, pi, ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.UserId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [GAMEMANAGER] " + " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void CreateGame(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            pi.Stat = 4;
            pi.Slot = pi.Game.getNextSlot();
            pi.Game.setNextSlot((int)pi.UserId);
            pi.Game.id = 1;
            pi.Game.isRunning = true;
            pi.Game.GSTA = 7;
            pi.Game.players[0] = pi;

            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            result.Add(Blaze.TdfInteger.Create("GSTA", pi.Game.GSTA));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);

            //Send NotifyGameStateChange Packet
            List<Blaze.Tdf> Result2 = NotifyGameStateChangeCommand.NotifyGameStateChange(pi);
            byte[] buff2 = Blaze.CreatePacket(4, 0x64, 0, 0x2000, p.ID, Result2);
            Logger.LogPacket("NotifyGameStateChange", Convert.ToInt32(pi.UserId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            //Send NotifyServerGameSetup Packet
            List<Blaze.Tdf> Result3 = NotifyServerGameSetupCommand.NotifyServerGameSetup(p, pi);
            byte[] buff3 = Blaze.CreatePacket(p.Component, 0x14, 0, 0x2000, p.ID, Result3);
            Logger.LogPacket("NotifyServerGameSetup", Convert.ToInt32(pi.UserId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            ns.Flush();
        }

        public static void SetGameModRegister(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.ReadPacketContent(p)[0]);

            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Blaze.Tdf>());
            ns.Write(buff, 0, buff.Length);

            byte[] buff2 = Blaze.CreatePacket(p.Component, 0x7B, 0, 0x2000, p.ID, result);
            ns.Write(buff2, 0, buff2.Length);

            ns.Flush();
        }

        public static void SetPlayerCapacity(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Blaze.Tdf>());
            ns.Write(buff, 0, buff.Length);

            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("GID", pi.UserId));
            result.Add(Blaze.TdfInteger.Create("LFPJ", 0));
            byte[] buff2 = Blaze.CreatePacket(p.Component, 0x6F, 0, 0x2000, p.ID, result);
            ns.Write(buff2, 0, buff2.Length);
            ns.Flush();
        }

        public static void GetFullGameData(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            // Get server info
            PlayerInfo srv = BlazeServer.GetServerInfo();
            if (srv == null)
            {
                BlazeServer.Log("[CLNT] #" + pi.UserId + " : cant find game to join!", System.Drawing.Color.Red);
                return;
            }
            pi.Game = srv.Game;
            pi.Slot = srv.Game.getNextSlot();
            srv.Game.setNextSlot((int)pi.UserId);

            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            List<Blaze.TdfStruct> LGAM = new List<Blaze.TdfStruct>();
            List<Blaze.Tdf> ee0 = new List<Blaze.Tdf>();
            List<Blaze.Tdf> GAME = new List<Blaze.Tdf>();
            GAME.Add(Blaze.TdfList.Create("ADMN", 0, 1, new List<long>(new long[] { srv.UserId })));
            GAME.Add(srv.Game.ATTR);
            GAME.Add(Blaze.TdfList.Create("CAP\0", 0, 2, new List<long>(new long[] { 0x20, 0 })));
            GAME.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            GAME.Add(Blaze.TdfString.Create("GNAM", pi.Game.GNAM));
            GAME.Add(Blaze.TdfInteger.Create("GPVH", 666));
            GAME.Add(Blaze.TdfInteger.Create("GSET", pi.Game.GSET));
            GAME.Add(Blaze.TdfInteger.Create("GSID", 1));
            GAME.Add(Blaze.TdfInteger.Create("GSTA", pi.Game.GSTA));
            GAME.Add(Blaze.TdfString.Create("GTYP", ""));
            GAME.Add(BlazeHelper.CreateNETField(srv, "HNET"));
            GAME.Add(Blaze.TdfInteger.Create("HSES", 13666));
            GAME.Add(Blaze.TdfInteger.Create("IGNO", 0));
            GAME.Add(Blaze.TdfInteger.Create("MCAP", 0x20));
            GAME.Add(BlazeHelper.CreateNQOSField(srv, "NQOS"));
            GAME.Add(Blaze.TdfInteger.Create("NRES", 0));
            GAME.Add(Blaze.TdfInteger.Create("NTOP", 1));
            GAME.Add(Blaze.TdfString.Create("PGID", ""));
            List<Blaze.Tdf> PHST = new List<Blaze.Tdf>();
            PHST.Add(Blaze.TdfInteger.Create("HPID", srv.UserId));
            PHST.Add(Blaze.TdfInteger.Create("HSLT", srv.Slot));
            GAME.Add(Blaze.TdfStruct.Create("PHST", PHST));
            GAME.Add(Blaze.TdfInteger.Create("PRES", 1));
            GAME.Add(Blaze.TdfString.Create("PSAS", "wv"));
            GAME.Add(Blaze.TdfInteger.Create("QCAP", 0x10));
            GAME.Add(Blaze.TdfInteger.Create("SEED", 0x2CF2048F));
            GAME.Add(Blaze.TdfInteger.Create("TCAP", 0x10));
            List<Blaze.Tdf> THST = new List<Blaze.Tdf>();
            THST.Add(Blaze.TdfInteger.Create("HPID", srv.UserId));
            THST.Add(Blaze.TdfInteger.Create("HPID", srv.Slot));
            GAME.Add(Blaze.TdfStruct.Create("THST", THST));
            GAME.Add(Blaze.TdfString.Create("UUID", "f5193367-c991-4429-aee4-8d5f3adab938"));
            GAME.Add(Blaze.TdfInteger.Create("VOIP", pi.Game.VOIP));
            GAME.Add(Blaze.TdfString.Create("VSTR", pi.Game.VSTR));
            ee0.Add(Blaze.TdfStruct.Create("GAME", GAME));
            LGAM.Add(Blaze.TdfStruct.Create("0", ee0));
            result.Add(Blaze.TdfList.Create("LGAM", 3, 1, LGAM));
            byte[] buff = Blaze.CreatePacket(p.Component, 0x67, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void UpdateMeshConnection(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> input = Blaze.ReadPacketContent(p);
            List<Blaze.TdfStruct> entries = (List<Blaze.TdfStruct>)((Blaze.TdfList)input[1]).List;
            Blaze.TdfInteger pid = (Blaze.TdfInteger)entries[0].Values[1];
            Blaze.TdfInteger stat = (Blaze.TdfInteger)entries[0].Values[2];
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Blaze.Tdf>());
            ns.Write(buff, 0, buff.Length);

            PlayerInfo target = BlazeServer.GetPlayerById(pid.Value);
            if (target != null)
            {
                if (stat.Value == 2)
                {
                    if (pi.IsServer)
                    {
                        //AsyncUserSessions.UserSessionExtendedDataUpdateNotification(pi, p, target, ns);
                        List<Blaze.Tdf> Result1 = UserSessionExtendedDataUpdateNotificationCommand.UserSessionExtendedDataUpdateNotification(pi);
                        byte[] buff1 = Blaze.CreatePacket(0x7802, 1, 0, 0x2000, p.ID, Result1);
                        Logger.LogPacket("UserSessionExtendedDataUpdateNotification", Convert.ToInt32(pi.UserId), buff1); //TestLog
                        ns.Write(buff1, 0, buff1.Length);

                        //AsyncGameManager.NotifyGamePlayerStateChange(pi, p, target, ns, 4);
                        List<Blaze.Tdf> Result2 = NotifyGamePlayerStateChangeCommand.NotifyGamePlayerStateChange(pi, 4);
                        byte[] buff2 = Blaze.CreatePacket(0x4, 0x74, 0, 0x2000, p.ID, Result2);
                        Logger.LogPacket("NotifyGamePlayerStateChange", Convert.ToInt32(pi.UserId), buff2); //TestLog
                        ns.Write(buff2, 0, buff2.Length);

                        //AsyncGameManager.NotifyPlayerJoinCompleted(pi, p, target, ns);
                        List<Blaze.Tdf> Result3 = NotifyPlayerJoinCompletedCommand.NotifyPlayerJoinCompleted(pi);
                        byte[] buff3 = Blaze.CreatePacket(0x4, 0x1E, 0, 0x2000, p.ID, Result3);
                        Logger.LogPacket("NotifyPlayerJoinCompleted", Convert.ToInt32(pi.UserId), buff3); //TestLog
                        ns.Write(buff3, 0, buff3.Length);
                    }
                    else
                    {
                        //AsyncGameManager.NotifyGamePlayerStateChange(pi, p, pi, ns, 4);
                        List<Blaze.Tdf> Result4 = NotifyGamePlayerStateChangeCommand.NotifyGamePlayerStateChange(pi, 4);
                        byte[] buff4 = Blaze.CreatePacket(0x4, 0x74, 0, 0x2000, p.ID, Result4);
                        Logger.LogPacket("NotifyGamePlayerStateChange", Convert.ToInt32(pi.UserId), buff4); //TestLog
                        ns.Write(buff4, 0, buff4.Length);

                        //AsyncGameManager.NotifyPlayerJoinCompleted(pi, p, pi, ns);
                        List<Blaze.Tdf> Result5 = NotifyPlayerJoinCompletedCommand.NotifyPlayerJoinCompleted(pi);
                        byte[] buff5 = Blaze.CreatePacket(0x4, 0x1E, 0, 0x2000, p.ID, Result5);
                        Logger.LogPacket("NotifyPlayerJoinCompleted", Convert.ToInt32(pi.UserId), buff5); //TestLog
                        ns.Write(buff5, 0, buff5.Length);
                    }
                }
                else
                {
                    //AsyncUserSessions.NotifyUserRemoved(pi, p, pid.Value, ns);
                    //Send NotifyPlayerRemoved Packet 
                    List<Blaze.Tdf> Result6 = NotifyUserRemovedCommand.NotifyUserRemoved(pi, p.ID);
                    byte[] buff6 = Blaze.CreatePacket(0x7802, 0x3, 0, 0x2000, p.ID, Result6);
                    Logger.LogPacket("NotifyUserRemoved", Convert.ToInt32(pi.UserId), buff6); //TestLog
                    ns.Write(buff6, 0, buff6.Length);
                }

            }
            ns.Flush();
        }


        public static void RemovePlayer(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> input = Blaze.ReadPacketContent(p);
            Blaze.TdfInteger CNTX = (Blaze.TdfInteger)input[1];
            Blaze.TdfInteger PID = (Blaze.TdfInteger)input[3];
            Blaze.TdfInteger REAS = (Blaze.TdfInteger)input[4];
            pi.Game.removePlayer((int)PID.Value);
            GC.Collect();
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Blaze.Tdf>());
            ns.Write(buff, 0, buff.Length);
            foreach (PlayerInfo player in BlazeServer.GetPlayers())
            {
                if (player != null && player.UserId == PID.Value)
                {
                    player.Cntx = CNTX.Value;
                    foreach (PlayerInfo player2 in pi.Game.players)
                    {
                        if (player2 != null && player2.UserId != PID.Value)
                        {
                            try
                            {
                                //AsyncGameManager.NotifyPlayerRemoved(player, p, player, player.ns, PID.Value, CNTX.Value, REAS.Value);
                                //Send NotifyPlayerRemoved Packet 
                                List<Blaze.Tdf> Result2 = NotifyPlayerRemovedCommand.NotifyPlayerRemoved(pi, PID.Value, CNTX.Value, REAS.Value);
                                byte[] buff2 = Blaze.CreatePacket(p.Component, 0x6E, 0, 0x2000, p.ID, Result2);
                                Logger.LogPacket("NotifyPlayerRemoved", Convert.ToInt32(pi.UserId), buff2); //TestLog
                                ns.Write(buff2, 0, buff2.Length);
                            }
                            catch
                            {
                                BlazeServer.Log("[CLNT] #" + pi.UserId + " : 'RemovePlayer' peer crashed!", System.Drawing.Color.Red);
                            }
                        }
                    }
                }
            }
            ns.Flush();
        }

        public static void ReplayGame(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Blaze.Tdf>());
            ns.Write(buff, 0, buff.Length);
            pi.Game.GSTA = 130;
            pi.Timeout.Restart();
            foreach (PlayerInfo peer in pi.Game.players)
            {
                if (peer != null)
                {
                    //AsyncGameManager.NotifyGameStateChange(peer, p, pi, peer.ns);
                    //Send NotifyGameStateChange Packet
                    List<Blaze.Tdf> Result2 = NotifyServerGameSetupCommand.NotifyServerGameSetup(p,pi);
                    byte[] buff2 = Blaze.CreatePacket(p.Command, 0x14, 0, 0x2000, p.ID, Result2);
                    Logger.LogPacket("NotifyServerGameSetup", Convert.ToInt32(pi.UserId), buff2); //TestLog
                    ns.Write(buff2, 0, buff2.Length);
                }
            }
            ns.Flush();
        }

        public static void resetDedicatedServer(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            if (pi.Game == null)
            {
                pi.Game = new GameInfo();
            }

            pi.Stat = 4;
            pi.Slot = pi.Game.getNextSlot();
            pi.Game.setNextSlot((int)pi.UserId);
            pi.Game.id = 1;
            pi.Game.isRunning = true;
            pi.Game.GSTA = 7;
            pi.Game.players[0] = pi;

            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            result.Add(Blaze.TdfInteger.Create("GSTA", pi.Game.GSTA));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);

            ////Send NotifyGameStateChange Packet
            //List<Blaze.Tdf> Result2 = NotifyGameStateChangeCommand.NotifyGameStateChange(pi);
            //byte[] buff2 = Blaze.CreatePacket(4, 0x64, 0, 0x2000, p.ID, Result2);
            //Logger.LogPacket("NotifyGameStateChange", Convert.ToInt32(pi.userId), buff2); //TestLog
            //ns.Write(buff2, 0, buff2.Length);

            //Send NotifyServerGameSetup Packet
            List<Blaze.Tdf> Result3 = NotifyServerGameSetupCommand.NotifyServerGameSetup(p, pi);
            byte[] buff3 = Blaze.CreatePacket(p.Component, 0x14, 0, 0x2000, p.ID, Result3);
            Logger.LogPacket("NotifyServerGameSetup", Convert.ToInt32(pi.UserId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            ns.Flush();
        }

        // Work in progress implementation of getGameListSnapshot
        // Ideal journey: getGameListSnapshot >> GetGameListResponse >> NotifyGameListUpdate >> destroyGameList 
        public static void getGameListSnapshot(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();

            // GLID: (TdfInteger)
            result.Add(Blaze.TdfInteger.Create("GLID", 1));

            // MAXF: (TdfInteger)
            result.Add(Blaze.TdfInteger.Create("MAXF", 2000));

            // NGD: (TdfInteger)
            result.Add(Blaze.TdfInteger.Create("NGD", 0));

            // Create response packet
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            Logger.LogPacket("getGameListSnapshot", Convert.ToInt32(pi.UserId), buff);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void StartMatchmaking(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            PlayerInfo srv = BlazeServer.GetServerInfo();
            if (srv == null)
            {
                BlazeServer.Log("[CLNT] #" + pi.UserId + " : cant find game to join!", System.Drawing.Color.OrangeRed);
                return;
            }
            pi.Game = srv.Game;
            pi.Slot = srv.Game.getNextSlot();
            srv.Game.setNextSlot((int)pi.UserId);

            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("MSID", pi.UserId));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);
            pi.Stat = 2;

            //AsyncUserSessions.NotifyUserAdded(pi, p, srv, ns);
            //Send NotifyUserAdded Packet
            List<Blaze.Tdf> Result2 = NotifyUserAddedCommand.NotifyUserAdded(pi);
            byte[] buff2 = Blaze.CreatePacket(0x7802, 0x2, 0, 0x2000, p.ID, Result2);
            Logger.LogPacket("NotifyUserAdded", Convert.ToInt32(pi.UserId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            //AsyncUserSessions.NotifyUserStatus(pi, p, srv, ns);
            //Send NotifyUserStatus Packet
            List<Blaze.Tdf> Result3 = NotifyUserStatusCommand.NotifyUserStatus(pi);
            byte[] buff3 = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, p.ID, Result3);
            Logger.LogPacket("NotifyUserStatus", Convert.ToInt32(pi.UserId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            //AsyncGameManager.NotifyClientGameSetup(pi, p, pi, srv, ns);
            //NotifyClientGameSetup 
            List<Blaze.Tdf> Result4 = NotifyClientGameSetupCommand.NotifyClientGameSetup(pi,srv);
            byte[] buff4 = Blaze.CreatePacket(0x4, 0x14, 0, 0x2000, p.ID, Result4);
            Logger.LogPacket("NotifyClientGameSetup", Convert.ToInt32(pi.UserId), buff4); //TestLog
            ns.Write(buff4, 0, buff4.Length);

            //AsyncUserSessions.NotifyUserAdded(srv, p, pi, srv.ns);
            //Send NotifyUserAdded Packet
            List<Blaze.Tdf> Result5 = NotifyUserAddedCommand.NotifyUserAdded(pi);
            byte[] buff5 = Blaze.CreatePacket(0x7802, 0x2, 0, 0x2000, p.ID, Result5);
            Logger.LogPacket("NotifyUserAdded", Convert.ToInt32(pi.UserId), buff5); //TestLog
            srv.NetworkStream.Write(buff5, 0, buff5.Length);

            //AsyncUserSessions.NotifyUserStatus(srv, p, pi, srv.ns);
            //Send NotifyUserStatus Server Packet
            List<Blaze.Tdf> Result6 = NotifyUserStatusCommand.NotifyUserStatus(pi);
            byte[] buff6 = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, p.ID, Result6);
            Logger.LogPacket("NotifyUserStatus", Convert.ToInt32(pi.UserId), buff6); //TestLog
            srv.NetworkStream.Write(buff6, 0, buff6.Length);

            //AsyncGameManager.NotifyPlayerJoining(srv, p, pi, srv.ns);
            //Send NotifyPlayerJoining Packet
            List<Blaze.Tdf> Result7 = NotifyPlayerJoiningCommand.NotifyPlayerJoining(pi);
            byte[] buff7 = Blaze.CreatePacket(0x4, 0x15, 0, 0x2000, 0, Result7);
            Logger.LogPacket("NotifyPlayerJoiningtoServer", Convert.ToInt32(pi.UserId), buff7); //TestLog
            srv.NetworkStream.Write(buff7, 0, buff7.Length);

            //AsyncUserSessions.UserSessionExtendedDataUpdateNotification(srv, p, pi, srv.ns);
            //Send UserSessionExtendedDataUpdateNotification Server Packet
            List<Blaze.Tdf> Result8 = UserSessionExtendedDataUpdateNotificationCommand.UserSessionExtendedDataUpdateNotification(pi);
            byte[] buff8 = Blaze.CreatePacket(0x7802, 1, 0, 0x2000, p.ID, Result7);
            Logger.LogPacket("UserSessionExtendedDataUpdateNotification", Convert.ToInt32(pi.UserId), buff8); //TestLog
            srv.NetworkStream.Write(buff8, 0, buff8.Length);

            ns.Flush();
            srv.NetworkStream.Flush();
        }


        public static void DestroyGame(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.ReadPacketContent(p)[0]);
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }


        public static void SetGameAttributes(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> input = Blaze.ReadPacketContent(p);
            pi.Game.ATTR = (Blaze.TdfDoubleList)input[0];
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);

            foreach (PlayerInfo peer in pi.Game.players)
            {
                if (peer != null)
                    try
                    {
                        //Send NotifyGameSettingsChange Packet
                        List<Blaze.Tdf> Result2 = NotifyGameSettingsChangeCommand.NotifyGameSettingsChange(pi);
                        byte[] buff2 = Blaze.CreatePacket(p.Component, 0x6E, 0, 0x2000, p.ID, Result2);
                        Logger.LogPacket("NotifyGameSettingsChange", Convert.ToInt32(pi.UserId), buff2); //TestLog
                        ns.Write(buff2, 0, buff2.Length);
                    }
                    catch
                    {
                        pi.Game.removePlayer((int)peer.UserId);
                        BlazeServer.Log("[CLNT] #" + pi.UserId + " : 'SetGameAttributes' peer crashed!", System.Drawing.Color.Red);
                    }
            }

            ns.Flush();
        }

        public static void JoinGame(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            PlayerInfo srv = BlazeServer.GetServerInfo();
            if (srv == null)
            {
                BlazeServer.Log("[CLNT] #" + pi.UserId + " : cant find game to join!", System.Drawing.Color.OrangeRed);
                return;
            }
            pi.Game = srv.Game;
            pi.Slot = srv.Game.getNextSlot();
            BlazeServer.Log("[CLNT] #" + pi.UserId + " : assigned Slot Id " + pi.Slot, System.Drawing.Color.Blue);
            if (pi.Slot == 255)
            {
                BlazeServer.Log("[CLNT] #" + pi.UserId + " : server full!", System.Drawing.Color.OrangeRed);
                return;
            }
            srv.Game.setNextSlot((int)pi.UserId);
            srv.Game.players[pi.Slot] = pi;

            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("GID\0", srv.Game.id));
            result.Add(Blaze.TdfInteger.Create("JGS\0", 0));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);
            pi.Stat = 2;

            //AsyncUserSessions.NotifyUserAdded(pi, p, pi, ns);
            //Send NotifyUserAdded Packet
            List<Blaze.Tdf> Result2 = NotifyUserAddedCommand.NotifyUserAdded(pi);
            byte[] buff2 = Blaze.CreatePacket(0x7802, 0x2, 0, 0x2000, p.ID, Result2);
            Logger.LogPacket("NotifyUserAdded", Convert.ToInt32(pi.UserId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            //AsyncUserSessions.NotifyUserStatus(pi, p, pi, ns);
            //Send NotifyUserStatus Packet
            List<Blaze.Tdf> Result3 = NotifyUserStatusCommand.NotifyUserStatus(pi);
            byte[] buff3 = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, p.ID, Result3);
            Logger.LogPacket("NotifyUserStatus", Convert.ToInt32(pi.UserId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            //AsyncGameManager.NotifyClientGameSetup(pi, p, pi, srv, ns);
            //Send NotifyClientGameSetup Packet
            List<Blaze.Tdf> Result4 = NotifyClientGameSetupCommand.NotifyClientGameSetup(pi, srv);
            byte[] buff4 = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, p.ID, Result4);
            Logger.LogPacket("NotifyClientGameSetup", Convert.ToInt32(pi.UserId), buff4); //TestLog
            ns.Write(buff4, 0, buff4.Length);

            //AsyncUserSessions.NotifyUserAdded(srv, p, pi, srv.ns);
            //Send NotifyUserAdded Server Packet
            List<Blaze.Tdf> Result5 = NotifyUserAddedCommand.NotifyUserAdded(pi);
            byte[] buff5 = Blaze.CreatePacket(0x7802, 0x2, 0, 0x2000, p.ID, Result5);
            Logger.LogPacket("NotifyUserAddedtoServer", Convert.ToInt32(pi.UserId), buff5); //TestLog
            srv.NetworkStream.Write(buff5, 0, buff5.Length);

            //AsyncUserSessions.NotifyUserStatus(srv, p, pi, srv.ns);
            //Send NotifyUserStatus Server Packet
            List<Blaze.Tdf> Result6 = NotifyUserStatusCommand.NotifyUserStatus(pi);
            byte[] buff6 = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, p.ID, Result6);
            Logger.LogPacket("NotifyUserStatustoServer", Convert.ToInt32(pi.UserId), buff6); //TestLog
            srv.NetworkStream.Write(buff6, 0, buff6.Length);

            //AsyncUserSessions.UserSessionExtendedDataUpdateNotification(srv, p, pi, srv.ns);
            //Send UserSessionExtendedDataUpdateNotification Server Packet
            List<Blaze.Tdf> Result7 = UserSessionExtendedDataUpdateNotificationCommand.UserSessionExtendedDataUpdateNotification(pi);
            byte[] buff7 = Blaze.CreatePacket(0x7802, 1, 0, 0x2000, p.ID, Result7);
            Logger.LogPacket("UserSessionExtendedDataUpdateNotificationtoServer", Convert.ToInt32(pi.UserId), buff7); //TestLog
            srv.NetworkStream.Write(buff7, 0, buff7.Length);

            //AsyncGameManager.NotifyPlayerJoining(srv, p, pi, srv.ns);
            List<Blaze.Tdf> Result8 = NotifyPlayerJoiningCommand.NotifyPlayerJoining(pi);
            byte[] buff8 = Blaze.CreatePacket(0x4, 0x15, 0, 0x2000, 0, Result8);
            Logger.LogPacket("NotifyPlayerJoiningtoServer", Convert.ToInt32(pi.UserId), buff7); //TestLog
            srv.NetworkStream.Write(buff8, 0, buff8.Length);

            ns.Flush();
            srv.NetworkStream.Flush();
        }


        public static void AdvanceGameState(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> input = Blaze.ReadPacketContent(p);
            pi.Game.GSTA = (uint)((Blaze.TdfInteger)input[1]).Value;
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Blaze.Tdf>());
            ns.Write(buff, 0, buff.Length);

            foreach (PlayerInfo peer in pi.Game.players)
            {
                if (peer != null)
                {
                    // Send NotifyGameStateChange Packet
                    List<Blaze.Tdf> Result2 = NotifyGameStateChangeCommand.NotifyGameStateChange(pi);
                    byte[] buff2 = Blaze.CreatePacket(4, 0x64, 0, 0x2000, p.ID, Result2);
                    Logger.LogPacket("NotifyGameStateChange", Convert.ToInt32(pi.UserId), buff2); //TestLog
                    ns.Write(buff2, 0, buff2.Length);
                }
            }

            ns.Flush();
        }


        public static void FinalizeGameCreation(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);

            if (pi.IsServer)
            {
                // Send NotifyPlatformHostInitialized Packet
                List<Blaze.Tdf> Result2 = NotifyPlatformHostInitializedCommand.NotifyPlatformHostInitialized(pi);
                byte[] buff2 = Blaze.CreatePacket(4, 0x47, 0, 0x2000, p.ID, Result2);
                Logger.LogPacket("NotifyPlatformHostInitialized", Convert.ToInt32(pi.UserId), buff2); //TestLog
                ns.Write(buff2, 0, buff2.Length);
            }

            ns.Flush();
        }

        public static void SetPlayerTeam(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Blaze.Tdf>());
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }
    }
}
