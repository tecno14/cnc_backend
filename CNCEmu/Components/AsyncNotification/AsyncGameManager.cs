﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;
using CNCEmu.Services.Network;

namespace CNCEmu
{
    public static class AsyncGameManager
    {
        public static void NotifyPlatformHostInitialized(PlayerInfo src, Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            result.Add(Blaze.TdfInteger.Create("PHID", pi.UserId));
            result.Add(Blaze.TdfInteger.Create("PHST", 0));
            byte[] buff = Blaze.CreatePacket(4, 0x47, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0047] NotifyPlatformHostInitialized");
        }

        public static void NotifyGameStateChange(PlayerInfo src, Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            result.Add(Blaze.TdfInteger.Create("GSTA", pi.Game.GSTA));
            byte[] buff = Blaze.CreatePacket(4, 0x64, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0064] NotifyGameStateChange");
        }

        public static void NotifyServerGameSetup(PlayerInfo src, Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> input = Blaze.ReadPacketContent(p);
            uint t = Blaze.GetUnixTimeStamp();
            pi.Game.GNAM = ((Blaze.TdfString)input[2]).Value;
            pi.Game.GSET = ((Blaze.TdfInteger)input[3]).Value;
            pi.Game.VOIP = ((Blaze.TdfInteger)input[21]).Value;
            pi.Game.VSTR = ((Blaze.TdfString)input[22]).Value;
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            List<Blaze.Tdf> GAME = new List<Blaze.Tdf>();
            GAME.Add(Blaze.TdfList.Create("ADMN", 0, 1, new List<long>(new long[] { pi.UserId })));
            GAME.Add(Blaze.TdfList.Create("CAP\0", 0, 2, new List<long>(new long[] { 0x20, 0 })));
            GAME.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            GAME.Add(Blaze.TdfString.Create("GNAM", pi.Game.GNAM));
            GAME.Add(Blaze.TdfInteger.Create("GPVH", 666));
            GAME.Add(Blaze.TdfInteger.Create("GSET", pi.Game.GSET));
            GAME.Add(Blaze.TdfInteger.Create("GSID", 1));
            GAME.Add(Blaze.TdfInteger.Create("GSTA", pi.Game.GSTA));
            GAME.Add(Blaze.TdfString.Create("GTYP", ""));
            GAME.Add(BlazeHelper.CreateNETField(pi, "HNET"));
            GAME.Add(Blaze.TdfInteger.Create("HSES", 13666));
            GAME.Add(Blaze.TdfInteger.Create("IGNO", 0));
            GAME.Add(Blaze.TdfInteger.Create("MCAP", 0x20));
            GAME.Add(BlazeHelper.CreateNQOSField(pi, "NQOS"));
            GAME.Add(Blaze.TdfInteger.Create("NRES", 0));
            GAME.Add(Blaze.TdfInteger.Create("NTOP", 1));
            GAME.Add(Blaze.TdfString.Create("PGID", ""));
            List<Blaze.Tdf> PHST = new List<Blaze.Tdf>();
            PHST.Add(Blaze.TdfInteger.Create("HPID", pi.UserId));
            GAME.Add(Blaze.TdfStruct.Create("PHST", PHST));
            GAME.Add(Blaze.TdfInteger.Create("PRES", 1));
            GAME.Add(Blaze.TdfString.Create("PSAS", "wv"));
            GAME.Add(Blaze.TdfInteger.Create("QCAP", 0x10));
            GAME.Add(Blaze.TdfInteger.Create("SEED", 0x2CF2048F));
            GAME.Add(Blaze.TdfInteger.Create("TCAP", 0x10));
            List<Blaze.Tdf> THST = new List<Blaze.Tdf>();
            THST.Add(Blaze.TdfInteger.Create("HPID", pi.UserId));
            GAME.Add(Blaze.TdfStruct.Create("THST", THST));
            GAME.Add(Blaze.TdfList.Create("TIDS", 0, 2, new List<long>(new long[] { 1, 2 })));
            GAME.Add(Blaze.TdfString.Create("UUID", "f5193367-c991-4429-aee4-8d5f3adab938"));
            GAME.Add(Blaze.TdfInteger.Create("VOIP", pi.Game.VOIP));
            GAME.Add(Blaze.TdfString.Create("VSTR", pi.Game.VSTR));
            result.Add(Blaze.TdfStruct.Create("GAME", GAME));
            List<Blaze.TdfStruct> PROS = new List<Blaze.TdfStruct>();
            List<Blaze.Tdf> ee0 = new List<Blaze.Tdf>();
            ee0.Add(Blaze.TdfInteger.Create("EXID", pi.UserId));
            ee0.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            ee0.Add(Blaze.TdfInteger.Create("LOC\0", pi.Loc));
            ee0.Add(Blaze.TdfString.Create("NAME", pi.Profile.Name));
            ee0.Add(Blaze.TdfInteger.Create("PID\0", pi.UserId));
            ee0.Add(BlazeHelper.CreateNETFieldUnion(pi, "PNET"));
            ee0.Add(Blaze.TdfInteger.Create("SID\0", pi.Slot));
            ee0.Add(Blaze.TdfInteger.Create("SLOT", 0));
            ee0.Add(Blaze.TdfInteger.Create("STAT", 2));
            ee0.Add(Blaze.TdfInteger.Create("TIDX", 0xFFFF));
            ee0.Add(Blaze.TdfInteger.Create("TIME", t));
            ee0.Add(Blaze.TdfInteger.Create("UID\0", pi.UserId));
            PROS.Add(Blaze.TdfStruct.Create("0", ee0));
            result.Add(Blaze.TdfList.Create("PROS", 3, 1, PROS));
            List<Blaze.Tdf> VALU = new List<Blaze.Tdf>();
            VALU.Add(Blaze.TdfInteger.Create("DCTX", 0));
            result.Add(Blaze.TdfUnion.Create("REAS", 0, Blaze.TdfStruct.Create("VALU", VALU)));
            byte[] buff = Blaze.CreatePacket(p.Component, 0x14, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0014] NotifyServerGameSetup");
        }

        public static void NotifyGameSettingsChange(PlayerInfo src, Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("ATTR", pi.Game.GSET));
            result.Add(Blaze.TdfInteger.Create("GID", pi.Game.id));
            byte[] buff = Blaze.CreatePacket(p.Component, 0x6E, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:006E] NotifyGameSettingsChange");
        }

        public static void NotifyClientGameSetup(PlayerInfo src, Blaze.Packet p, PlayerInfo pi, PlayerInfo srv, NetworkStream ns, long reas = 1)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            List<Blaze.Tdf> GAME = new List<Blaze.Tdf>();
            GAME.Add(Blaze.TdfList.Create("ADMN", 0, 1, new List<long>(new long[] { srv.UserId })));
            if (srv.Game.ATTR != null)
                GAME.Add(srv.Game.ATTR);
            GAME.Add(Blaze.TdfList.Create("CAP\0", 0, 2, new List<long>(new long[] { 0x20, 0 })));
            GAME.Add(Blaze.TdfInteger.Create("GID\0", srv.Game.id));
            GAME.Add(Blaze.TdfString.Create("GNAM", srv.Game.GNAM));
            GAME.Add(Blaze.TdfInteger.Create("GPVH", 666));
            GAME.Add(Blaze.TdfInteger.Create("GSET", srv.Game.GSET));
            GAME.Add(Blaze.TdfInteger.Create("GSID", 1));
            GAME.Add(Blaze.TdfInteger.Create("GSTA", srv.Game.GSTA));
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
            THST.Add(Blaze.TdfInteger.Create("HSLT", srv.Slot));
            GAME.Add(Blaze.TdfStruct.Create("THST", THST));
            List<long> playerIdList = new List<long>();
            for (int i = 0; i < 32; i++)
                if (srv.Game.slotUse[i] != -1)
                    playerIdList.Add(srv.Game.slotUse[i]);
            GAME.Add(Blaze.TdfList.Create("TIDS", 0, 2, playerIdList));
            GAME.Add(Blaze.TdfString.Create("UUID", "f5193367-c991-4429-aee4-8d5f3adab938"));
            GAME.Add(Blaze.TdfInteger.Create("VOIP", srv.Game.VOIP));
            GAME.Add(Blaze.TdfString.Create("VSTR", srv.Game.VSTR));
            result.Add(Blaze.TdfStruct.Create("GAME", GAME));
            List<Blaze.TdfStruct> PROS = new List<Blaze.TdfStruct>();
            for (int i = 0; i < 32; i++)
                if (srv.Game.players[i] != null)
                    PROS.Add(BlazeHelper.MakePROSEntry(i, srv.Game.players[i]));
            result.Add(Blaze.TdfList.Create("PROS", 3, PROS.Count, PROS));
            List<Blaze.Tdf> VALU = new List<Blaze.Tdf>();
            VALU.Add(Blaze.TdfInteger.Create("DCTX", reas));
            result.Add(Blaze.TdfUnion.Create("REAS", 0, Blaze.TdfStruct.Create("VALU", VALU)));
            byte[] buff = Blaze.CreatePacket(0x4, 0x14, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0014] NotifyClientGameSetup");
        }

        public static void NotifyPlayerJoining(PlayerInfo src, Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            uint t = Blaze.GetUnixTimeStamp();
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            List<Blaze.Tdf> PDAT = new List<Blaze.Tdf>();
            PDAT.Add(Blaze.TdfInteger.Create("EXID", pi.UserId));
            PDAT.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            PDAT.Add(Blaze.TdfInteger.Create("LOC\0", pi.Loc));
            PDAT.Add(Blaze.TdfString.Create("NAME", pi.Profile.Name));
            PDAT.Add(Blaze.TdfInteger.Create("PID\0", pi.UserId));
            PDAT.Add(BlazeHelper.CreateNETFieldUnion(pi, "PNET"));
            PDAT.Add(Blaze.TdfInteger.Create("SID\0", pi.Slot));
            PDAT.Add(Blaze.TdfInteger.Create("STAT", pi.Stat));
            PDAT.Add(Blaze.TdfInteger.Create("TIDX", 0xFFFF));
            PDAT.Add(Blaze.TdfInteger.Create("TIME", t));
            PDAT.Add(Blaze.TdfInteger.Create("UID\0", pi.UserId));
            result.Add(Blaze.TdfStruct.Create("PDAT", PDAT));
            byte[] buff = Blaze.CreatePacket(0x4, 0x15, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0015] NotifyPlayerJoining");
        }

        public static void NotifyGamePlayerStateChange(PlayerInfo src, Blaze.Packet p, PlayerInfo pi, NetworkStream ns, long stat)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            result.Add(Blaze.TdfInteger.Create("PID\0", pi.UserId));
            result.Add(Blaze.TdfInteger.Create("STAT", stat));
            byte[] buff = Blaze.CreatePacket(0x4, 0x74, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0074] NotifyGamePlayerStateChange");
        }

        public static void NotifyPlayerJoinCompleted(PlayerInfo src, Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            result.Add(Blaze.TdfInteger.Create("PID\0", pi.UserId));
            byte[] buff = Blaze.CreatePacket(0x4, 0x1E, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:001E] NotifyPlayerJoinCompleted");
        }

        public static void NotifyPlayerRemoved(PlayerInfo src, Blaze.Packet p, PlayerInfo pi, NetworkStream ns, long pid, long cntx, long reas)
        {
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            result.Add(Blaze.TdfInteger.Create("CNTX", cntx));
            result.Add(Blaze.TdfInteger.Create("GID\0", pi.Game.id));
            result.Add(Blaze.TdfInteger.Create("PID\0", pid));
            result.Add(Blaze.TdfInteger.Create("REAS", reas));
            byte[] buff = Blaze.CreatePacket(4, 0x28, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:001E] NotifyPlayerRemoved");
        }

    }
}
