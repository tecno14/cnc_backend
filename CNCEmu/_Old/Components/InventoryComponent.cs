using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;
using BlazeLibWV.Models;
using CNCEmu.Models;
using CNCEmu.Extensions;

namespace CNCEmu
{
    public static class InventoryComponent
    {
        public static void HandlePacket(Packet p, Player pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x00:
                    break;
                case 0x01:
                    GetItems(p, pi, ns);
                    break;
                case 0x06:
                    GetTemplate(p, pi, ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.UserId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [INVENTORY] " + " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void GetItems(Packet p, Player pi, NetworkStream ns)
        {
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Tdf>());
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void GetTemplate(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> Result = new List<Tdf>();
            List<string> t = "{aek971_acog} {aek971_eotech}".ConvertToStringList();
            Result.Add(TdfList.Create("ILST", 1, t.Count, t));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }
    }
}
