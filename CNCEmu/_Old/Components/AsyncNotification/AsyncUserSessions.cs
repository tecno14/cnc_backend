using BlazeLibWV;
using BlazeLibWV.Models;
using CNCEmu.Models;
using CNCEmu.Services.Network;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class AsyncUserSessions
    {
        public static void UserSessionExtendedDataUpdateNotification(Player src, Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> Result = new List<Tdf>
            {
                BlazeHelper.CreateUserDataStruct(pi),
                TdfInteger.Create("USID", pi.UserId)
            };
            byte[] buff = Blaze.CreatePacket(0x7802, 1, 0, 0x2000, 0, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [7802:0001] UserSessionExtendedDataUpdateNotification");
        }

        public static void NotifyUserAdded(Player src, Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> result = new List<Tdf>
            {
                BlazeHelper.CreateUserDataStruct(pi),
                BlazeHelper.CreateUserStruct(pi)
            };
            byte[] buff = Blaze.CreatePacket(0x7802, 0x2, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [7802:0001] NotifyUserAdded");
        }

        public static void NotifyUserRemoved(Player src, Packet p, long pid, NetworkStream ns)
        {
            List<Tdf> result = new List<Tdf>
            {
                TdfInteger.Create("BUID", pid)
            };
            byte[] buff = Blaze.CreatePacket(0x7802, 0x3, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [7802:0001] NotifyUserRemoved");
        }

        public static void NotifyUserStatus(Player src, Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> result = new List<Tdf>
            {
                TdfInteger.Create("FLGS", 3),
                TdfInteger.Create("ID\0\0", pi.UserId)
            };
            byte[] buff = Blaze.CreatePacket(0x7802, 0x5, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [7802:0001] NotifyUserStatus");
        }
    }
}
