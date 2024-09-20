using BlazeLibWV;
using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;

namespace CNCEmu
{
    public static class NotifyServerGameSetupCommand
    {
        public static List<Tdf> NotifyServerGameSetup(Packet p, Player pi)
        {
            List<Tdf> input = Blaze.ReadPacketContent(p);

            uint t = Blaze.GetUnixTimeStamp();
            pi.Game.GNAM = ((TdfString)input[4]).Value;
            pi.Game.GSET = ((TdfInteger)input[5]).Value;
            pi.Game.VOIP = ((TdfInteger)input[22]).Value;
            pi.Game.VSTR = ((TdfString)input[23]).Value;
            List<Tdf> result = new List<Tdf>();
            List<Tdf> GAME = new List<Tdf>
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
            List<Tdf> PHST = new List<Tdf>
            {
                TdfInteger.Create("HPID", pi.UserId)
            };
            GAME.Add(TdfStruct.Create("PHST", PHST));
            GAME.Add(TdfInteger.Create("PRES", 1));
            GAME.Add(TdfString.Create("PSAS", "wv"));
            GAME.Add(TdfInteger.Create("QCAP", 0x10));
            GAME.Add(TdfInteger.Create("SEED", 0x2CF2048F));
            GAME.Add(TdfInteger.Create("TCAP", 0x10));
            List<Tdf> THST = new List<Tdf>
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
            List<Tdf> ee0 = new List<Tdf>
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
            List<Tdf> VALU = new List<Tdf>
            {
                TdfInteger.Create("DCTX", 0)
            };
            result.Add(TdfUnion.Create("REAS", 0, TdfStruct.Create("VALU", VALU)));

            return result;
        }


    }
}
