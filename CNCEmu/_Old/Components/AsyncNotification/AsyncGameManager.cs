using BlazeLibWV;
using BlazeLibWV.Models;
using CNCEmu.Models;
using CNCEmu.Services.Network;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class AsyncGameManager
    {
        public static void NotifyPlatformHostInitialized(Player src, Packet p, Player pi, NetworkStream ns)
        {
            var result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("PHID", pi.UserId),
                TdfInteger.Create("PHST", 0)
            };
            byte[] buff = Blaze.CreatePacket(4, 0x47, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0047] NotifyPlatformHostInitialized");
        }

        public static void NotifyGameStateChange(Player src, Packet p, Player pi, NetworkStream ns)
        {
            var result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("GSTA", pi.Game.GSTA)
            };
            byte[] buff = Blaze.CreatePacket(4, 0x64, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0064] NotifyGameStateChange");
        }

        public static void NotifyServerGameSetup(Player src, Packet p, Player pi, NetworkStream ns)
        {
            var input = Blaze.ReadPacketContent(p);
            uint t = Blaze.GetUnixTimeStamp();
            pi.Game.GNAM = ((TdfString)input[2]).Value;
            pi.Game.GSET = ((TdfInteger)input[3]).Value;
            pi.Game.VOIP = ((TdfInteger)input[21]).Value;
            pi.Game.VSTR = ((TdfString)input[22]).Value;
            var result = new List<Tdf>();
            var GAME = new List<Tdf>
            {
                TdfList.Create("ADMN", 0, 1, new List<long>(new long[] { pi.UserId })),
                TdfList.Create("CAP\0", 0, 2, new List<long>(new long[] { 0x20, 0 })),
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfString.Create("GNAM", pi.Game.GNAM),
                TdfInteger.Create("GPVH", 666),
                TdfInteger.Create("GSET", pi.Game.GSET),
                TdfInteger.Create("GSID", 1),
                TdfInteger.Create("GSTA", pi.Game.GSTA),
                TdfString.Create("GTYP", ""),
                BlazeHelper.CreateNETField(pi, "HNET"),
                TdfInteger.Create("HSES", 13666),
                TdfInteger.Create("IGNO", 0),
                TdfInteger.Create("MCAP", 0x20),
                BlazeHelper.CreateNQOSField(pi, "NQOS"),
                TdfInteger.Create("NRES", 0),
                TdfInteger.Create("NTOP", 1),
                TdfString.Create("PGID", "")
            };
            var PHST = new List<Tdf>
            {
                TdfInteger.Create("HPID", pi.UserId)
            };
            GAME.Add(TdfStruct.Create("PHST", PHST));
            GAME.Add(TdfInteger.Create("PRES", 1));
            GAME.Add(TdfString.Create("PSAS", "wv"));
            GAME.Add(TdfInteger.Create("QCAP", 0x10));
            GAME.Add(TdfInteger.Create("SEED", 0x2CF2048F));
            GAME.Add(TdfInteger.Create("TCAP", 0x10));
            var THST = new List<Tdf>
            {
                TdfInteger.Create("HPID", pi.UserId)
            };
            GAME.Add(TdfStruct.Create("THST", THST));
            GAME.Add(TdfList.Create("TIDS", 0, 2, new List<long>(new long[] { 1, 2 })));
            GAME.Add(TdfString.Create("UUID", "f5193367-c991-4429-aee4-8d5f3adab938"));
            GAME.Add(TdfInteger.Create("VOIP", pi.Game.VOIP));
            GAME.Add(TdfString.Create("VSTR", pi.Game.VSTR));
            result.Add(TdfStruct.Create("GAME", GAME));
            List<TdfStruct> PROS = new List<TdfStruct>();
            var ee0 = new List<Tdf>
            {
                TdfInteger.Create("EXID", pi.UserId),
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("LOC\0", pi.Loc),
                TdfString.Create("NAME", pi.Profile.Name),
                TdfInteger.Create("PID\0", pi.UserId),
                BlazeHelper.CreateNETFieldUnion(pi, "PNET"),
                TdfInteger.Create("SID\0", pi.Slot),
                TdfInteger.Create("SLOT", 0),
                TdfInteger.Create("STAT", 2),
                TdfInteger.Create("TIDX", 0xFFFF),
                TdfInteger.Create("TIME", t),
                TdfInteger.Create("UID\0", pi.UserId)
            };
            PROS.Add(TdfStruct.Create("0", ee0));
            result.Add(TdfList.Create("PROS", 3, 1, PROS));
            var VALU = new List<Tdf>
            {
                TdfInteger.Create("DCTX", 0)
            };
            result.Add(TdfUnion.Create("REAS", 0, TdfStruct.Create("VALU", VALU)));
            byte[] buff = Blaze.CreatePacket(p.Component, 0x14, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0014] NotifyServerGameSetup");
        }

        public static void NotifyGameSettingsChange(Player src, Packet p, Player pi, NetworkStream ns)
        {
            var result = new List<Tdf>
            {
                TdfInteger.Create("ATTR", pi.Game.GSET),
                TdfInteger.Create("GID", pi.Game.id)
            };
            byte[] buff = Blaze.CreatePacket(p.Component, 0x6E, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:006E] NotifyGameSettingsChange");
        }

        public static void NotifyClientGameSetup(Player src, Packet p, Player pi, Player srv, NetworkStream ns, long reas = 1)
        {
            var result = new List<Tdf>();
            var GAME = new List<Tdf>
            {
                TdfList.Create("ADMN", 0, 1, new List<long>(new long[] { srv.UserId }))
            };
            if (srv.Game.ATTR != null)
                GAME.Add(srv.Game.ATTR);
            GAME.Add(TdfList.Create("CAP\0", 0, 2, new List<long>(new long[] { 0x20, 0 })));
            GAME.Add(TdfInteger.Create("GID\0", srv.Game.id));
            GAME.Add(TdfString.Create("GNAM", srv.Game.GNAM));
            GAME.Add(TdfInteger.Create("GPVH", 666));
            GAME.Add(TdfInteger.Create("GSET", srv.Game.GSET));
            GAME.Add(TdfInteger.Create("GSID", 1));
            GAME.Add(TdfInteger.Create("GSTA", srv.Game.GSTA));
            GAME.Add(TdfString.Create("GTYP", ""));
            GAME.Add(BlazeHelper.CreateNETField(srv, "HNET"));
            GAME.Add(TdfInteger.Create("HSES", 13666));
            GAME.Add(TdfInteger.Create("IGNO", 0));
            GAME.Add(TdfInteger.Create("MCAP", 0x20));
            GAME.Add(BlazeHelper.CreateNQOSField(srv, "NQOS"));
            GAME.Add(TdfInteger.Create("NRES", 0));
            GAME.Add(TdfInteger.Create("NTOP", 1));
            GAME.Add(TdfString.Create("PGID", ""));
            var PHST = new List<Tdf>
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
            var THST = new List<Tdf>
            {
                TdfInteger.Create("HPID", srv.UserId),
                TdfInteger.Create("HSLT", srv.Slot)
            };
            GAME.Add(TdfStruct.Create("THST", THST));
            List<long> playerIdList = new List<long>();
            for (int i = 0; i < 32; i++)
                if (srv.Game.slotUse[i] != -1)
                    playerIdList.Add(srv.Game.slotUse[i]);
            GAME.Add(TdfList.Create("TIDS", 0, 2, playerIdList));
            GAME.Add(TdfString.Create("UUID", "f5193367-c991-4429-aee4-8d5f3adab938"));
            GAME.Add(TdfInteger.Create("VOIP", srv.Game.VOIP));
            GAME.Add(TdfString.Create("VSTR", srv.Game.VSTR));
            result.Add(TdfStruct.Create("GAME", GAME));
            List<TdfStruct> PROS = new List<TdfStruct>();
            for (int i = 0; i < 32; i++)
                if (srv.Game.players[i] != null)
                    PROS.Add(BlazeHelper.MakePROSEntry(i, srv.Game.players[i]));
            result.Add(TdfList.Create("PROS", 3, PROS.Count, PROS));
            var VALU = new List<Tdf>
            {
                TdfInteger.Create("DCTX", reas)
            };
            result.Add(TdfUnion.Create("REAS", 0, TdfStruct.Create("VALU", VALU)));
            byte[] buff = Blaze.CreatePacket(0x4, 0x14, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0014] NotifyClientGameSetup");
        }

        public static void NotifyPlayerJoining(Player src, Packet p, Player pi, NetworkStream ns)
        {
            uint t = Blaze.GetUnixTimeStamp();
            var result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id)
            };
            var PDAT = new List<Tdf>
            {
                TdfInteger.Create("EXID", pi.UserId),
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("LOC\0", pi.Loc),
                TdfString.Create("NAME", pi.Profile.Name),
                TdfInteger.Create("PID\0", pi.UserId),
                BlazeHelper.CreateNETFieldUnion(pi, "PNET"),
                TdfInteger.Create("SID\0", pi.Slot),
                TdfInteger.Create("STAT", pi.Stat),
                TdfInteger.Create("TIDX", 0xFFFF),
                TdfInteger.Create("TIME", t),
                TdfInteger.Create("UID\0", pi.UserId)
            };
            result.Add(TdfStruct.Create("PDAT", PDAT));
            byte[] buff = Blaze.CreatePacket(0x4, 0x15, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0015] NotifyPlayerJoining");
        }

        public static void NotifyGamePlayerStateChange(Player src, Packet p, Player pi, NetworkStream ns, long stat)
        {
            var result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("PID\0", pi.UserId),
                TdfInteger.Create("STAT", stat)
            };
            byte[] buff = Blaze.CreatePacket(0x4, 0x74, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:0074] NotifyGamePlayerStateChange");
        }

        public static void NotifyPlayerJoinCompleted(Player src, Packet p, Player pi, NetworkStream ns)
        {
            var result = new List<Tdf>
            {
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("PID\0", pi.UserId)
            };
            byte[] buff = Blaze.CreatePacket(0x4, 0x1E, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:001E] NotifyPlayerJoinCompleted");
        }

        public static void NotifyPlayerRemoved(Player src, Packet p, Player pi, NetworkStream ns, long pid, long cntx, long reas)
        {
            var result = new List<Tdf>
            {
                TdfInteger.Create("CNTX", cntx),
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("PID\0", pid),
                TdfInteger.Create("REAS", reas)
            };
            byte[] buff = Blaze.CreatePacket(4, 0x28, 0, 0x2000, 0, result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + src.UserId + " [0004:001E] NotifyPlayerRemoved");
        }

    }
}
