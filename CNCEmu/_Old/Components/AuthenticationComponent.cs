using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using static BlazeLibWV.Blaze;
using System.Net.Sockets;
using CNCEmu.Services.Network;
using CNCEmu.Models;
using CNCEmu.Services;
using BlazeLibWV.Models;
using CNCEmu.Utils.Logger;

namespace CNCEmu
{
    public static class AuthenticationComponent
    {
        public static void HandlePacket(Packet p, Player pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x00:
                    break;
                case 0x3C:
                    ExpressLogin(p, pi, ns);
                    break;
                case 0x28:
                    Login(p, pi, ns);
                    break;
                case 0x64:
                    ListPersonas(p, pi, ns);
                    break;
                case 0x6E:
                    LoginPersona(p, pi, ns);
                    break;
                case 0x78:
                    LogoutPersona(p, pi, ns);
                    break;
                case 0x46:
                    Logout(p, pi, ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.UserId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [AUTHENTICATIONCOMP] " + " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void ExpressLogin(Packet p, Player pi, NetworkStream ns)
        {
            uint t = GetUnixTimeStamp();

            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("AGUP", 0), // Can Age Up
                TdfInteger.Create("ANON", 0), // Is Anonymous
                TdfInteger.Create("NTOS", 0), // Needs Legal Docs (TOS)
                TdfString.Create("PCTK", "PlayerTicket_1337") // PCLogin Token
            };

            List<Tdf> SESS = new List<Tdf>
            {
                TdfInteger.Create("BUID", pi.UserId), //BlazeUserID
                TdfString.Create("KEY", "SessionKey_1337"),
                TdfString.Create("MAIL", "cnc-2013-pc@ea.com"),
                TdfInteger.Create("UID\0", pi.UserId),
                TdfInteger.Create("FRSC", 0),
                TdfInteger.Create("FRST", 0),
                TdfInteger.Create("LLOG", 1403663841)
            };

            Result.Add(TdfStruct.Create("SESS", SESS));

            List<Tdf> PDTL = new List<Tdf>
            {
                TdfString.Create("DSNM", pi.Profile.Name),
                TdfInteger.Create("LAST", t),
                TdfInteger.Create("PID\0", pi.UserId),
                TdfInteger.Create("PLAT", 4), //#1 XBL2 #2 PS3 #3 WII #4 PC
                TdfInteger.Create("STAS", 2),
                TdfInteger.Create("XREF", 0)
            };
            Result.Add(TdfStruct.Create("PDTL", PDTL));
            Result.Add(TdfInteger.Create("SPAM", 0));
            Result.Add(TdfInteger.Create("UNDR", 0));

            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            LogService.LogPacket("ExpressLog", Convert.ToInt32(pi.UserId), buff); //TestLog
            ns.Write(buff, 0, buff.Length);

            // Send UserAuthenticated Packet
            List<Tdf> Result2 = UserAuthenticatedCommand.UserAuthenticated(pi);
            byte[] buff2 = Blaze.CreatePacket(0x7802, 8, 0, 0x2000, p.ID, Result2);
            LogService.LogPacket("UserAuthenticated", Convert.ToInt32(pi.UserId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            //Send UserUpdated Packet
            List<Tdf> Result3 = UserUpdatedCommand.UserUpdated(pi);
            byte[] buff3 = Blaze.CreatePacket(0x7802, 5, 0, 0x2000, p.ID, Result3);
            LogService.LogPacket("UserUpdated", Convert.ToInt32(pi.UserId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            //Send UserAdded Packet
            List<Tdf> Result4 = UserAddedCommand.UserAdded(pi);
            byte[] buff4 = Blaze.CreatePacket(0x7802, 2, 0, 0x2000, p.ID, Result4);
            LogService.LogPacket("UserAdded", Convert.ToInt32(pi.UserId), buff4); //TestLog
            ns.Write(buff4, 0, buff4.Length);

            ns.Flush();
        }

        public static void Login(Packet p, Player pi, NetworkStream ns)
        {
            if (!pi.IsServer)
            {
                List<Tdf> input = Blaze.ReadPacketContent(p);
                TdfString MAIL = (TdfString)input[0];
                string mail = MAIL.Value;

                pi.Profile = ProfileService.Instance.GetProfileByEmail(mail);
                var id = pi.Profile?.Id ?? 0;

                if (pi.Profile == null)
                {
                    BlazeServer.Log("[CLNT] #" + pi.UserId + " Could not find player profile for mail: " + mail + " !", System.Drawing.Color.Red);
                    pi.UserId = 0;
                    return;
                }
                else
                {
                    BlazeServer.RemovePlayer(id);
                    pi.UserId = id;
                    BlazeServer.Log("[CLNT] New ID #" + pi.UserId + " Client Playername = \"" + pi.Profile.Name + "\"", System.Drawing.Color.Blue);
                }
            }

            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("ANON", 0),
                TdfInteger.Create("NTOS", 0),
                TdfString.Create("PCTK", "")
            };

            List<TdfStruct> playerentries = new List<TdfStruct>();
            List<Tdf> PlayerEntry = new List<Tdf>
            {
                TdfString.Create("DSNM", pi.Profile.Name),
                TdfInteger.Create("LAST", 0),
                TdfInteger.Create("PID\0", pi.UserId),
                TdfInteger.Create("STAS", 2),
                TdfInteger.Create("XREF", 0),
                TdfInteger.Create("XTYP", 0)
            };
            playerentries.Add(TdfStruct.Create("0", PlayerEntry));
            Result.Add(TdfList.Create("PLST", 3, 1, playerentries));

            Result.Add(TdfString.Create("SKEY", "123456"));
            Result.Add(TdfInteger.Create("SPAM", 0));
            Result.Add(TdfInteger.Create("UID\0", pi.UserId));
            Result.Add(TdfInteger.Create("UNDR", 0));

            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            LogService.LogPacket("Log", Convert.ToInt32(pi.UserId), buff); //TestLog

            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void LoginPersona(Packet p, Player pi, NetworkStream ns)
        {
            uint t = Blaze.GetUnixTimeStamp();
            List<Tdf> SESS = new List<Tdf>
            {
                TdfInteger.Create("BUID", pi.UserId),
                TdfInteger.Create("FRST", 0),
                TdfString.Create("KEY\0", "some_client_key"),
                TdfInteger.Create("LLOG", t),
                TdfString.Create("MAIL", "")
            };
            List<Tdf> PDTL = new List<Tdf>
            {
                TdfString.Create("DSNM", pi.Profile.Name),
                TdfInteger.Create("LAST", t),
                TdfInteger.Create("PID\0", pi.UserId),
                TdfInteger.Create("STAS", 0),
                TdfInteger.Create("XREF", 0),
                TdfInteger.Create("XTYP", 0)
            };
            SESS.Add(TdfStruct.Create("PDTL", PDTL));
            SESS.Add(TdfInteger.Create("UID\0", pi.UserId));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, SESS);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();

            AsyncUserSessions.NotifyUserAdded(pi, p, pi, ns);
            AsyncUserSessions.NotifyUserStatus(pi, p, pi, ns);

            // Send UserAuthenticated Packet
            List<Tdf> Result2 = UserAuthenticatedCommand.UserAuthenticated(pi);
            byte[] buff2 = Blaze.CreatePacket(0x7802, 8, 0, 0x2000, p.ID, Result2);
            LogService.LogPacket("UserAuthenticated", Convert.ToInt32(pi.UserId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            //Send UserUpdated Packet
            List<Tdf> Result3 = UserUpdatedCommand.UserUpdated(pi);
            byte[] buff3 = Blaze.CreatePacket(0x7802, 5, 0, 0x2000, p.ID, Result3);
            LogService.LogPacket("UserUpdated", Convert.ToInt32(pi.UserId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            //Send UserAdded Packet
            List<Tdf> Result4 = UserAddedCommand.UserAdded(pi);
            byte[] buff4 = Blaze.CreatePacket(0x7802, 2, 0, 0x2000, p.ID, Result4);
            LogService.LogPacket("UserAdded", Convert.ToInt32(pi.UserId), buff4); //TestLog
            ns.Write(buff4, 0, buff4.Length);
        }

        public static void LogoutPersona(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> result = new List<Tdf>();
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();

            AsyncUserSessions.NotifyUserRemoved(pi, p, pi.UserId, ns);
            AsyncUserSessions.NotifyUserStatus(pi, p, pi, ns);
        }

        public static void Logout(Packet p, Player pi, NetworkStream ns)
        {
            //Send logout Packet
            List<Tdf> Result = UserAddedCommand.UserAdded(pi);
            byte[] buff = Blaze.CreatePacket(1, 46, 0, 0x2000, p.ID, Result);
            LogService.LogPacket("logout", Convert.ToInt32(pi.UserId), buff); //TestLog
            ns.Write(buff, 0, buff.Length);
        }

        public static void ListPersonas(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> result = new List<Tdf>();
            List<TdfStruct> entries = new List<TdfStruct>();
            List<Tdf> e = new List<Tdf>
            {
                TdfString.Create("DSNM", pi.Profile.Name),
                TdfInteger.Create("LAST", Blaze.GetUnixTimeStamp()),
                TdfInteger.Create("PID\0", pi.Profile.Id),
                TdfInteger.Create("STAS", 2),
                TdfInteger.Create("XREF", 0),
                TdfInteger.Create("XTYP", 0)
            };
            entries.Add(TdfStruct.Create("0", e));
            result.Add(TdfList.Create("PINF", 3, 1, entries));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

    }
}