﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class UserSessionsComponent
    {
        public static void HandlePacket(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
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

        public static void UpdateNetworkInfo(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> input = Blaze.ReadPacketContent(p);
            Blaze.TdfUnion addr = (Blaze.TdfUnion)input[0];
            Blaze.TdfStruct valu = (Blaze.TdfStruct)addr.UnionContent;
            Blaze.TdfStruct exip = (Blaze.TdfStruct)valu.Values[0];
            Blaze.TdfStruct inip = (Blaze.TdfStruct)valu.Values[1];
            pi.InIp = ((Blaze.TdfInteger)inip.Values[0]).Value;
            pi.ExIp = ((Blaze.TdfInteger)exip.Values[0]).Value;
            pi.ExPort = pi.InPort = (uint)((Blaze.TdfInteger)inip.Values[1]).Value;
            Blaze.TdfStruct nqos = (Blaze.TdfStruct)input[2];
            pi.Nat = ((Blaze.TdfInteger)nqos.Values[1]).Value;
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Blaze.Tdf>());
            ns.Write(buff, 0, buff.Length);

            // Send UserSessionExtendedDataUpdateNotification Packet
            List<Blaze.Tdf> Result2 = UserSessionExtendedDataUpdateNotificationCommand.UserSessionExtendedDataUpdateNotification(pi);
            byte[] buff2 = Blaze.CreatePacket(0x7802, 1, 0, 0x2000, p.ID, Result2);
            Logger.LogPacket("UserSessionExtendedDataUpdateNotification", Convert.ToInt32(pi.UserId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            ns.Flush();
        }
    }
}
