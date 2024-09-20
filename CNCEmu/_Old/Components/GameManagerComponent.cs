using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;
using CNCEmu.Services.Network;
using BlazeLibWV.Models;
using CNCEmu.Models;
using CNCEmu.Utils.Logger;

namespace CNCEmu
{
    public static class GameManagerComponent
    {
        public static void HandlePacket(Packet p, Player pi, NetworkStream ns)
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
                    ResetDedicatedServer(p, pi, ns);
                    break;
                case 0x64:
                    GetGameListSnapshot(p, pi, ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.UserId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [GAMEMANAGER] " + " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void CreateGame(Packet p, Player pi, NetworkStream ns)
        {
            pi.Stat = 4;
            pi.Slot = pi.Game.GetNextSlot();
            pi.Game.SetNextSlot((int)pi.UserId);
            pi.Game.id = 1;
            pi.Game.isRunning = true;
            pi.Game.GSTA = 7;
            pi.Game.players[0] = pi;

            List<Tdf> result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("GSTA", pi.Game.GSTA)
            };
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);

            //Send NotifyGameStateChange Packet
            List<Tdf> Result2 = NotifyGameStateChangeCommand.NotifyGameStateChange(pi);
            byte[] buff2 = Blaze.CreatePacket(4, 0x64, 0, 0x2000, p.ID, Result2);
            LogService.LogPacket("NotifyGameStateChange", Convert.ToInt32(pi.UserId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            //Send NotifyServerGameSetup Packet
            List<Tdf> Result3 = NotifyServerGameSetupCommand.NotifyServerGameSetup(p, pi);
            byte[] buff3 = Blaze.CreatePacket(p.Component, 0x14, 0, 0x2000, p.ID, Result3);
            LogService.LogPacket("NotifyServerGameSetup", Convert.ToInt32(pi.UserId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            ns.Flush();
        }

        public static void SetGameModRegister(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> result = new List<Tdf>
            {
                Blaze.ReadPacketContent(p)[0]
            };

            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Tdf>());
            ns.Write(buff, 0, buff.Length);

            byte[] buff2 = Blaze.CreatePacket(p.Component, 0x7B, 0, 0x2000, p.ID, result);
            ns.Write(buff2, 0, buff2.Length);

            ns.Flush();
        }

        public static void SetPlayerCapacity(Packet p, Player pi, NetworkStream ns)
        {
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Tdf>());
            ns.Write(buff, 0, buff.Length);

            List<Tdf> result = new List<Tdf>
            {
                TdfInteger.Create("GID", pi.UserId),
                TdfInteger.Create("LFPJ", 0)
            };
            byte[] buff2 = Blaze.CreatePacket(p.Component, 0x6F, 0, 0x2000, p.ID, result);
            ns.Write(buff2, 0, buff2.Length);
            ns.Flush();
        }

        public static void GetFullGameData(Packet p, Player pi, NetworkStream ns)
        {
            // Get server info
            Player srv = BlazeServer.GetServerInfo();
            if (srv == null)
            {
                BlazeServer.Log("[CLNT] #" + pi.UserId + " : cant find game to join!", System.Drawing.Color.Red);
                return;
            }
            pi.Game = srv.Game;
            pi.Slot = srv.Game.GetNextSlot();
            srv.Game.SetNextSlot((int)pi.UserId);

            List<Tdf> result = new List<Tdf>();
            List<TdfStruct> LGAM = new List<TdfStruct>();
            List<Tdf> ee0 = new List<Tdf>();
            List<Tdf> GAME = new List<Tdf>
            {
                TdfList.Create("ADMN", 0, 1, new List<long>(new long[] { srv.UserId })),
                srv.Game.ATTR,
                TdfList.Create("CAP\0", 0, 2, new List<long>(new long[] { 0x20, 0 })),
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfString.Create("GNAM", pi.Game.GNAM),
                TdfInteger.Create("GPVH", 666),
                TdfInteger.Create("GSET", pi.Game.GSET),
                TdfInteger.Create("GSID", 1),
                TdfInteger.Create("GSTA", pi.Game.GSTA),
                TdfString.Create("GTYP", ""),
                BlazeHelper.CreateNETField(srv, "HNET"),
                TdfInteger.Create("HSES", 13666),
                TdfInteger.Create("IGNO", 0),
                TdfInteger.Create("MCAP", 0x20),
                BlazeHelper.CreateNQOSField(srv, "NQOS"),
                TdfInteger.Create("NRES", 0),
                TdfInteger.Create("NTOP", 1),
                TdfString.Create("PGID", "")
            };
            List<Tdf> PHST = new List<Tdf>
            {
                TdfInteger.Create("HPID", srv.UserId),
                TdfInteger.Create("HSLT", srv.Slot)
            };
            GAME.Add(TdfStruct.Create("PHST", PHST));
            GAME.Add(TdfInteger.Create("PRES", 1));
            GAME.Add(TdfString.Create("PSAS", "wv"));
            GAME.Add(TdfInteger.Create("QCAP", 0x10));
            GAME.Add(TdfInteger.Create("SEED", 0x2CF2048F));
            GAME.Add(TdfInteger.Create("TCAP", 0x10));
            List<Tdf> THST = new List<Tdf>
            {
                TdfInteger.Create("HPID", srv.UserId),
                TdfInteger.Create("HPID", srv.Slot)
            };
            GAME.Add(TdfStruct.Create("THST", THST));
            GAME.Add(TdfString.Create("UUID", "f5193367-c991-4429-aee4-8d5f3adab938"));
            GAME.Add(TdfInteger.Create("VOIP", pi.Game.VOIP));
            GAME.Add(TdfString.Create("VSTR", pi.Game.VSTR));
            ee0.Add(TdfStruct.Create("GAME", GAME));
            LGAM.Add(TdfStruct.Create("0", ee0));
            result.Add(TdfList.Create("LGAM", 3, 1, LGAM));
            byte[] buff = Blaze.CreatePacket(p.Component, 0x67, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void UpdateMeshConnection(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> input = Blaze.ReadPacketContent(p);
            List<TdfStruct> entries = (List<TdfStruct>)((TdfList)input[1]).List;
            TdfInteger pid = (TdfInteger)entries[0].Values[1];
            TdfInteger stat = (TdfInteger)entries[0].Values[2];
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Tdf>());
            ns.Write(buff, 0, buff.Length);

            Player target = BlazeServer.GetPlayerById(pid.Value);
            if (target != null)
            {
                if (stat.Value == 2)
                {
                    if (pi.IsServer)
                    {
                        //AsyncUserSessions.UserSessionExtendedDataUpdateNotification(pi, p, target, ns);
                        List<Tdf> Result1 = UserSessionExtendedDataUpdateNotificationCommand.UserSessionExtendedDataUpdateNotification(pi);
                        byte[] buff1 = Blaze.CreatePacket(0x7802, 1, 0, 0x2000, p.ID, Result1);
                        LogService.LogPacket("UserSessionExtendedDataUpdateNotification", Convert.ToInt32(pi.UserId), buff1); //TestLog
                        ns.Write(buff1, 0, buff1.Length);

                        //AsyncGameManager.NotifyGamePlayerStateChange(pi, p, target, ns, 4);
                        List<Tdf> Result2 = NotifyGamePlayerStateChangeCommand.NotifyGamePlayerStateChange(pi, 4);
                        byte[] buff2 = Blaze.CreatePacket(0x4, 0x74, 0, 0x2000, p.ID, Result2);
                        LogService.LogPacket("NotifyGamePlayerStateChange", Convert.ToInt32(pi.UserId), buff2); //TestLog
                        ns.Write(buff2, 0, buff2.Length);

                        //AsyncGameManager.NotifyPlayerJoinCompleted(pi, p, target, ns);
                        List<Tdf> Result3 = NotifyPlayerJoinCompletedCommand.NotifyPlayerJoinCompleted(pi);
                        byte[] buff3 = Blaze.CreatePacket(0x4, 0x1E, 0, 0x2000, p.ID, Result3);
                        LogService.LogPacket("NotifyPlayerJoinCompleted", Convert.ToInt32(pi.UserId), buff3); //TestLog
                        ns.Write(buff3, 0, buff3.Length);
                    }
                    else
                    {
                        //AsyncGameManager.NotifyGamePlayerStateChange(pi, p, pi, ns, 4);
                        List<Tdf> Result4 = NotifyGamePlayerStateChangeCommand.NotifyGamePlayerStateChange(pi, 4);
                        byte[] buff4 = Blaze.CreatePacket(0x4, 0x74, 0, 0x2000, p.ID, Result4);
                        LogService.LogPacket("NotifyGamePlayerStateChange", Convert.ToInt32(pi.UserId), buff4); //TestLog
                        ns.Write(buff4, 0, buff4.Length);

                        //AsyncGameManager.NotifyPlayerJoinCompleted(pi, p, pi, ns);
                        List<Tdf> Result5 = NotifyPlayerJoinCompletedCommand.NotifyPlayerJoinCompleted(pi);
                        byte[] buff5 = Blaze.CreatePacket(0x4, 0x1E, 0, 0x2000, p.ID, Result5);
                        LogService.LogPacket("NotifyPlayerJoinCompleted", Convert.ToInt32(pi.UserId), buff5); //TestLog
                        ns.Write(buff5, 0, buff5.Length);
                    }
                }
                else
                {
                    //AsyncUserSessions.NotifyUserRemoved(pi, p, pid.Value, ns);
                    //Send NotifyPlayerRemoved Packet 
                    List<Tdf> Result6 = NotifyUserRemovedCommand.NotifyUserRemoved(pi, p.ID);
                    byte[] buff6 = Blaze.CreatePacket(0x7802, 0x3, 0, 0x2000, p.ID, Result6);
                    LogService.LogPacket("NotifyUserRemoved", Convert.ToInt32(pi.UserId), buff6); //TestLog
                    ns.Write(buff6, 0, buff6.Length);
                }

            }
            ns.Flush();
        }


        public static void RemovePlayer(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> input = Blaze.ReadPacketContent(p);
            TdfInteger CNTX = (TdfInteger)input[1];
            TdfInteger PID = (TdfInteger)input[3];
            TdfInteger REAS = (TdfInteger)input[4];
            pi.Game.RemovePlayer((int)PID.Value);
            GC.Collect();
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Tdf>());
            ns.Write(buff, 0, buff.Length);
            foreach (Player player in BlazeServer.GetPlayers())
            {
                if (player != null && player.UserId == PID.Value)
                {
                    player.Cntx = CNTX.Value;
                    foreach (Player player2 in pi.Game.players)
                    {
                        if (player2 != null && player2.UserId != PID.Value)
                        {
                            try
                            {
                                //AsyncGameManager.NotifyPlayerRemoved(player, p, player, player.ns, PID.Value, CNTX.Value, REAS.Value);
                                //Send NotifyPlayerRemoved Packet 
                                List<Tdf> Result2 = NotifyPlayerRemovedCommand.NotifyPlayerRemoved(pi, PID.Value, CNTX.Value, REAS.Value);
                                byte[] buff2 = Blaze.CreatePacket(p.Component, 0x6E, 0, 0x2000, p.ID, Result2);
                                LogService.LogPacket("NotifyPlayerRemoved", Convert.ToInt32(pi.UserId), buff2); //TestLog
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

        public static void ReplayGame(Packet p, Player pi, NetworkStream ns)
        {
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Tdf>());
            ns.Write(buff, 0, buff.Length);
            pi.Game.GSTA = 130;
            pi.Timeout.Restart();
            foreach (Player peer in pi.Game.players)
            {
                if (peer != null)
                {
                    //AsyncGameManager.NotifyGameStateChange(peer, p, pi, peer.ns);
                    //Send NotifyGameStateChange Packet
                    List<Tdf> Result2 = NotifyServerGameSetupCommand.NotifyServerGameSetup(p,pi);
                    byte[] buff2 = Blaze.CreatePacket(p.Command, 0x14, 0, 0x2000, p.ID, Result2);
                    LogService.LogPacket("NotifyServerGameSetup", Convert.ToInt32(pi.UserId), buff2); //TestLog
                    ns.Write(buff2, 0, buff2.Length);
                }
            }
            ns.Flush();
        }

        public static void ResetDedicatedServer(Packet p, Player pi, NetworkStream ns)
        {
            if (pi.Game == null)
            {
                pi.Game = new GameInfo();
            }

            pi.Stat = 4;
            pi.Slot = pi.Game.GetNextSlot();
            pi.Game.SetNextSlot((int)pi.UserId);
            pi.Game.id = 1;
            pi.Game.isRunning = true;
            pi.Game.GSTA = 7;
            pi.Game.players[0] = pi;

            List<Tdf> result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("GSTA", pi.Game.GSTA)
            };
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);

            ////Send NotifyGameStateChange Packet
            //List<Tdf> Result2 = NotifyGameStateChangeCommand.NotifyGameStateChange(pi);
            //byte[] buff2 = Blaze.CreatePacket(4, 0x64, 0, 0x2000, p.ID, Result2);
            //LogService.LogPacket("NotifyGameStateChange", Convert.ToInt32(pi.userId), buff2); //TestLog
            //ns.Write(buff2, 0, buff2.Length);

            //Send NotifyServerGameSetup Packet
            List<Tdf> Result3 = NotifyServerGameSetupCommand.NotifyServerGameSetup(p, pi);
            byte[] buff3 = Blaze.CreatePacket(p.Component, 0x14, 0, 0x2000, p.ID, Result3);
            LogService.LogPacket("NotifyServerGameSetup", Convert.ToInt32(pi.UserId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            ns.Flush();
        }

        // Work in progress implementation of getGameListSnapshot
        // Ideal journey: getGameListSnapshot >> GetGameListResponse >> NotifyGameListUpdate >> destroyGameList 
        public static void GetGameListSnapshot(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> result = new List<Tdf>
            {
                // GLID: (TdfInteger)
                TdfInteger.Create("GLID", 1),

                // MAXF: (TdfInteger)
                TdfInteger.Create("MAXF", 2000),

                // NGD: (TdfInteger)
                TdfInteger.Create("NGD", 0)
            };

            // Create response packet
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            LogService.LogPacket("getGameListSnapshot", Convert.ToInt32(pi.UserId), buff);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void StartMatchmaking(Packet p, Player pi, NetworkStream ns)
        {
            Player srv = BlazeServer.GetServerInfo();
            if (srv == null)
            {
                BlazeServer.Log("[CLNT] #" + pi.UserId + " : cant find game to join!", System.Drawing.Color.OrangeRed);
                return;
            }
            pi.Game = srv.Game;
            pi.Slot = srv.Game.GetNextSlot();
            srv.Game.SetNextSlot((int)pi.UserId);

            List<Tdf> result = new List<Tdf>
            {
                TdfInteger.Create("MSID", pi.UserId)
            };
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);
            pi.Stat = 2;

            //AsyncUserSessions.NotifyUserAdded(pi, p, srv, ns);
            //Send NotifyUserAdded Packet
            List<Tdf> Result2 = NotifyUserAddedCommand.NotifyUserAdded(pi);
            byte[] buff2 = Blaze.CreatePacket(0x7802, 0x2, 0, 0x2000, p.ID, Result2);
            LogService.LogPacket("NotifyUserAdded", Convert.ToInt32(pi.UserId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            //AsyncUserSessions.NotifyUserStatus(pi, p, srv, ns);
            //Send NotifyUserStatus Packet
            List<Tdf> Result3 = NotifyUserStatusCommand.NotifyUserStatus(pi);
            byte[] buff3 = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, p.ID, Result3);
            LogService.LogPacket("NotifyUserStatus", Convert.ToInt32(pi.UserId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            //AsyncGameManager.NotifyClientGameSetup(pi, p, pi, srv, ns);
            //NotifyClientGameSetup 
            List<Tdf> Result4 = NotifyClientGameSetupCommand.NotifyClientGameSetup(pi,srv);
            byte[] buff4 = Blaze.CreatePacket(0x4, 0x14, 0, 0x2000, p.ID, Result4);
            LogService.LogPacket("NotifyClientGameSetup", Convert.ToInt32(pi.UserId), buff4); //TestLog
            ns.Write(buff4, 0, buff4.Length);

            //AsyncUserSessions.NotifyUserAdded(srv, p, pi, srv.ns);
            //Send NotifyUserAdded Packet
            List<Tdf> Result5 = NotifyUserAddedCommand.NotifyUserAdded(pi);
            byte[] buff5 = Blaze.CreatePacket(0x7802, 0x2, 0, 0x2000, p.ID, Result5);
            LogService.LogPacket("NotifyUserAdded", Convert.ToInt32(pi.UserId), buff5); //TestLog
            srv.NetworkStream.Write(buff5, 0, buff5.Length);

            //AsyncUserSessions.NotifyUserStatus(srv, p, pi, srv.ns);
            //Send NotifyUserStatus Server Packet
            List<Tdf> Result6 = NotifyUserStatusCommand.NotifyUserStatus(pi);
            byte[] buff6 = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, p.ID, Result6);
            LogService.LogPacket("NotifyUserStatus", Convert.ToInt32(pi.UserId), buff6); //TestLog
            srv.NetworkStream.Write(buff6, 0, buff6.Length);

            //AsyncGameManager.NotifyPlayerJoining(srv, p, pi, srv.ns);
            //Send NotifyPlayerJoining Packet
            List<Tdf> Result7 = NotifyPlayerJoiningCommand.NotifyPlayerJoining(pi);
            byte[] buff7 = Blaze.CreatePacket(0x4, 0x15, 0, 0x2000, 0, Result7);
            LogService.LogPacket("NotifyPlayerJoiningtoServer", Convert.ToInt32(pi.UserId), buff7); //TestLog
            srv.NetworkStream.Write(buff7, 0, buff7.Length);

            //AsyncUserSessions.UserSessionExtendedDataUpdateNotification(srv, p, pi, srv.ns);
            //Send UserSessionExtendedDataUpdateNotification Server Packet
            List<Tdf> Result8 = UserSessionExtendedDataUpdateNotificationCommand.UserSessionExtendedDataUpdateNotification(pi);
            byte[] buff8 = Blaze.CreatePacket(0x7802, 1, 0, 0x2000, p.ID, Result7);
            LogService.LogPacket("UserSessionExtendedDataUpdateNotification", Convert.ToInt32(pi.UserId), buff8); //TestLog
            srv.NetworkStream.Write(buff8, 0, buff8.Length);

            ns.Flush();
            srv.NetworkStream.Flush();
        }

        public static void DestroyGame(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> result = new List<Tdf>
            {
                Blaze.ReadPacketContent(p)[0]
            };
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void SetGameAttributes(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> input = Blaze.ReadPacketContent(p);
            pi.Game.ATTR = (TdfDoubleList)input[0];
            List<Tdf> result = new List<Tdf>();
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);

            foreach (Player peer in pi.Game.players)
            {
                if (peer != null)
                    try
                    {
                        //Send NotifyGameSettingsChange Packet
                        List<Tdf> Result2 = NotifyGameSettingsChangeCommand.NotifyGameSettingsChange(pi);
                        byte[] buff2 = Blaze.CreatePacket(p.Component, 0x6E, 0, 0x2000, p.ID, Result2);
                        LogService.LogPacket("NotifyGameSettingsChange", Convert.ToInt32(pi.UserId), buff2); //TestLog
                        ns.Write(buff2, 0, buff2.Length);
                    }
                    catch
                    {
                        pi.Game.RemovePlayer((int)peer.UserId);
                        BlazeServer.Log("[CLNT] #" + pi.UserId + " : 'SetGameAttributes' peer crashed!", System.Drawing.Color.Red);
                    }
            }

            ns.Flush();
        }

        public static void JoinGame(Packet p, Player pi, NetworkStream ns)
        {
            Player srv = BlazeServer.GetServerInfo();
            if (srv == null)
            {
                BlazeServer.Log("[CLNT] #" + pi.UserId + " : cant find game to join!", System.Drawing.Color.OrangeRed);
                return;
            }
            pi.Game = srv.Game;
            pi.Slot = srv.Game.GetNextSlot();
            BlazeServer.Log("[CLNT] #" + pi.UserId + " : assigned Slot Id " + pi.Slot, System.Drawing.Color.Blue);
            if (pi.Slot == 255)
            {
                BlazeServer.Log("[CLNT] #" + pi.UserId + " : server full!", System.Drawing.Color.OrangeRed);
                return;
            }
            srv.Game.SetNextSlot((int)pi.UserId);
            srv.Game.players[pi.Slot] = pi;

            List<Tdf> result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", srv.Game.id),
                TdfInteger.Create("JGS\0", 0)
            };
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);
            pi.Stat = 2;

            //AsyncUserSessions.NotifyUserAdded(pi, p, pi, ns);
            //Send NotifyUserAdded Packet
            List<Tdf> Result2 = NotifyUserAddedCommand.NotifyUserAdded(pi);
            byte[] buff2 = Blaze.CreatePacket(0x7802, 0x2, 0, 0x2000, p.ID, Result2);
            LogService.LogPacket("NotifyUserAdded", Convert.ToInt32(pi.UserId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            //AsyncUserSessions.NotifyUserStatus(pi, p, pi, ns);
            //Send NotifyUserStatus Packet
            List<Tdf> Result3 = NotifyUserStatusCommand.NotifyUserStatus(pi);
            byte[] buff3 = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, p.ID, Result3);
            LogService.LogPacket("NotifyUserStatus", Convert.ToInt32(pi.UserId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            //AsyncGameManager.NotifyClientGameSetup(pi, p, pi, srv, ns);
            //Send NotifyClientGameSetup Packet
            List<Tdf> Result4 = NotifyClientGameSetupCommand.NotifyClientGameSetup(pi, srv);
            byte[] buff4 = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, p.ID, Result4);
            LogService.LogPacket("NotifyClientGameSetup", Convert.ToInt32(pi.UserId), buff4); //TestLog
            ns.Write(buff4, 0, buff4.Length);

            //AsyncUserSessions.NotifyUserAdded(srv, p, pi, srv.ns);
            //Send NotifyUserAdded Server Packet
            List<Tdf> Result5 = NotifyUserAddedCommand.NotifyUserAdded(pi);
            byte[] buff5 = Blaze.CreatePacket(0x7802, 0x2, 0, 0x2000, p.ID, Result5);
            LogService.LogPacket("NotifyUserAddedtoServer", Convert.ToInt32(pi.UserId), buff5); //TestLog
            srv.NetworkStream.Write(buff5, 0, buff5.Length);

            //AsyncUserSessions.NotifyUserStatus(srv, p, pi, srv.ns);
            //Send NotifyUserStatus Server Packet
            List<Tdf> Result6 = NotifyUserStatusCommand.NotifyUserStatus(pi);
            byte[] buff6 = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, p.ID, Result6);
            LogService.LogPacket("NotifyUserStatustoServer", Convert.ToInt32(pi.UserId), buff6); //TestLog
            srv.NetworkStream.Write(buff6, 0, buff6.Length);

            //AsyncUserSessions.UserSessionExtendedDataUpdateNotification(srv, p, pi, srv.ns);
            //Send UserSessionExtendedDataUpdateNotification Server Packet
            List<Tdf> Result7 = UserSessionExtendedDataUpdateNotificationCommand.UserSessionExtendedDataUpdateNotification(pi);
            byte[] buff7 = Blaze.CreatePacket(0x7802, 1, 0, 0x2000, p.ID, Result7);
            LogService.LogPacket("UserSessionExtendedDataUpdateNotificationtoServer", Convert.ToInt32(pi.UserId), buff7); //TestLog
            srv.NetworkStream.Write(buff7, 0, buff7.Length);

            //AsyncGameManager.NotifyPlayerJoining(srv, p, pi, srv.ns);
            List<Tdf> Result8 = NotifyPlayerJoiningCommand.NotifyPlayerJoining(pi);
            byte[] buff8 = Blaze.CreatePacket(0x4, 0x15, 0, 0x2000, 0, Result8);
            LogService.LogPacket("NotifyPlayerJoiningtoServer", Convert.ToInt32(pi.UserId), buff7); //TestLog
            srv.NetworkStream.Write(buff8, 0, buff8.Length);

            ns.Flush();
            srv.NetworkStream.Flush();
        }


        public static void AdvanceGameState(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> input = Blaze.ReadPacketContent(p);
            pi.Game.GSTA = (uint)((TdfInteger)input[1]).Value;
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Tdf>());
            ns.Write(buff, 0, buff.Length);

            foreach (Player peer in pi.Game.players)
            {
                if (peer != null)
                {
                    // Send NotifyGameStateChange Packet
                    List<Tdf> Result2 = NotifyGameStateChangeCommand.NotifyGameStateChange(pi);
                    byte[] buff2 = Blaze.CreatePacket(4, 0x64, 0, 0x2000, p.ID, Result2);
                    LogService.LogPacket("NotifyGameStateChange", Convert.ToInt32(pi.UserId), buff2); //TestLog
                    ns.Write(buff2, 0, buff2.Length);
                }
            }

            ns.Flush();
        }


        public static void FinalizeGameCreation(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> result = new List<Tdf>();
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);

            if (pi.IsServer)
            {
                // Send NotifyPlatformHostInitialized Packet
                List<Tdf> Result2 = NotifyPlatformHostInitializedCommand.NotifyPlatformHostInitialized(pi);
                byte[] buff2 = Blaze.CreatePacket(4, 0x47, 0, 0x2000, p.ID, Result2);
                LogService.LogPacket("NotifyPlatformHostInitialized", Convert.ToInt32(pi.UserId), buff2); //TestLog
                ns.Write(buff2, 0, buff2.Length);
            }

            ns.Flush();
        }

        public static void SetPlayerTeam(Packet p, Player pi, NetworkStream ns)
        {
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Tdf>());
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }
    }
}
