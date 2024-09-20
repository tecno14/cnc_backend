using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;


namespace CNCEmu
{
    class NotifyClientGameSetupCommand
    {
        public static List<Tdf> NotifyClientGameSetup(Player pi, Player srv)
        {
            long reas = 1;
            List<Tdf> Result = new List<Tdf>();
            List<Tdf> GAME = new List<Tdf>
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
            Result.Add(TdfStruct.Create("GAME", GAME));
            List<TdfStruct> PROS = new List<TdfStruct>();
            for (int i = 0; i < 32; i++)
                if (srv.Game.players[i] != null)
                    PROS.Add(BlazeHelper.MakePROSEntry(i, srv.Game.players[i]));
            Result.Add(TdfList.Create("PROS", 3, PROS.Count, PROS));
            List<Tdf> VALU = new List<Tdf>
            {
                TdfInteger.Create("DCTX", reas)
            };
            Result.Add(TdfUnion.Create("REAS", 0, TdfStruct.Create("VALU", VALU)));

            return Result;
        }
    }
}
