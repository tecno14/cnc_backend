using BlazeLibWV;
using BlazeLibWV.Models;
using CNCEmu.Models;
using CNCEmu.Utils.Logger;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class UserSessionsComponent
    {
        public static void HandlePacket(Packet p, Player pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x00:
                    break;
                case 0x14:
                    UpdateNetworkInfo(p, pi, ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.UserId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [USERSESSION] " + " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void UpdateNetworkInfo(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> input = Blaze.ReadPacketContent(p);
            TdfUnion addr = (TdfUnion)input[0];
            TdfStruct valu = (TdfStruct)addr.UnionContent;
            TdfStruct exip = (TdfStruct)valu.Values[0];
            TdfStruct inip = (TdfStruct)valu.Values[1];
            pi.InIp = ((TdfInteger)inip.Values[0]).Value;
            pi.ExIp = ((TdfInteger)exip.Values[0]).Value;
            pi.ExPort = pi.InPort = (uint)((TdfInteger)inip.Values[1]).Value;
            TdfStruct nqos = (TdfStruct)input[2];
            pi.Nat = ((TdfInteger)nqos.Values[1]).Value;
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Tdf>());
            ns.Write(buff, 0, buff.Length);

            // Send UserSessionExtendedDataUpdateNotification Packet
            List<Tdf> Result2 = UserSessionExtendedDataUpdateNotificationCommand.UserSessionExtendedDataUpdateNotification(pi);
            byte[] buff2 = Blaze.CreatePacket(0x7802, 1, 0, 0x2000, p.ID, Result2);
            LogService.LogPacket("UserSessionExtendedDataUpdateNotification", Convert.ToInt32(pi.UserId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            ns.Flush();
        }
    }
}
