using BlazeLibWV;
using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Collections.Generic;


namespace CNCEmu
{
    public static class BlazeHelper
    {
        public static TdfStruct MakeStatGroupEntry(int idx, string catg, string dflt, string ldsc, string name, string sdsc)
        {
            List<Tdf> result = new List<Tdf>
            {
                TdfString.Create("CATG", catg),
                TdfString.Create("DFLT", dflt),
                TdfInteger.Create("DRVD", 0),
                TdfString.Create("FRMT", "%d"),
                TdfString.Create("KIND", ""),
                TdfString.Create("LDSC", ldsc),
                TdfString.Create("META", ""),
                TdfString.Create("NAME", name),
                TdfString.Create("SDSC", sdsc),
                TdfInteger.Create("TYPE", 0)
            };
            return TdfStruct.Create(idx.ToString(), result);
        }

        public static TdfStruct MakePROSEntry(int idx, Player pi)
        {
            uint t = Blaze.GetUnixTimeStamp();
            List<Tdf> result = new List<Tdf>
            {
                TdfInteger.Create("EXID", pi.UserId),
                TdfInteger.Create("GID\0", pi.Game.id),
                TdfInteger.Create("LOC\0", pi.Loc),
                TdfString.Create("NAME", pi.Profile.Name),
                TdfInteger.Create("PID\0", pi.UserId),
                CreateNETFieldUnion(pi, "PNET"),
                TdfInteger.Create("SID\0", pi.Slot),
                TdfInteger.Create("STAT", pi.Stat),
                TdfInteger.Create("TIDX", 0xFFFF),
                TdfInteger.Create("TIME", t),
                TdfInteger.Create("UID\0", pi.UserId)
            };
            return TdfStruct.Create(idx.ToString(), result);
        }

        public static Tdf CreateNETField(Player pi, string label)
        {
            List<TdfStruct> list = new List<TdfStruct>();
            List<Tdf> e0 = new List<Tdf>();
            List<Tdf> EXIP = new List<Tdf>
            {
                TdfInteger.Create("IP\0\0", pi.ExIp),
                TdfInteger.Create("PORT", pi.ExPort)
            };
            e0.Add(TdfStruct.Create("EXIP", EXIP));
            List<Tdf> INIP = new List<Tdf>
            {
                TdfInteger.Create("IP\0\0", pi.InIp),
                TdfInteger.Create("PORT", pi.InPort)
            };
            e0.Add(TdfStruct.Create("INIP", INIP));
            list.Add(TdfStruct.Create("0", e0, true));
            return TdfList.Create(label, 3, 1, list);
        }

        public static Tdf CreateNETFieldUnion(Player pi, string label)
        {
            List<Tdf> VALU = new List<Tdf>();
            List<Tdf> EXIP = new List<Tdf>
            {
                TdfInteger.Create("IP", pi.ExIp),
                TdfInteger.Create("PORT", pi.ExPort)
            };
            VALU.Add(TdfStruct.Create("EXIP", EXIP));
            List<Tdf> INIP = new List<Tdf>
            {
                TdfInteger.Create("IP", pi.InIp),
                TdfInteger.Create("PORT", pi.InPort)
            };
            VALU.Add(TdfStruct.Create("INIP", INIP));
            return TdfUnion.Create(label, 2, TdfStruct.Create("VALU", VALU));
        }

        public static Tdf CreateADDRField(Player pi)
        {
            List<Tdf> ADDR = new List<Tdf>();
            List<Tdf> EXIP = new List<Tdf>
            {
                TdfInteger.Create("IP", pi.ExIp),
                TdfInteger.Create("PORT", pi.ExPort)
            };
            ADDR.Add(TdfStruct.Create("EXIP", EXIP));
            List<Tdf> INIP = new List<Tdf>
            {
                TdfInteger.Create("IP", pi.InIp),
                TdfInteger.Create("PORT", pi.InPort)
            };
            ADDR.Add(TdfStruct.Create("INIP", INIP));
            return TdfStruct.Create("ADDR", ADDR, true);
        }

        public static Tdf CreateNQOSField(Player pi, string label)
        {
            List<Tdf> NQOS = new List<Tdf>
            {
                TdfInteger.Create("DBPS", 0),
                TdfInteger.Create("NATT", pi.Nat),
                TdfInteger.Create("UBPS", 0)
            };
            return TdfStruct.Create(label, NQOS);
        }

        public static TdfStruct CreateUserStruct(Player pi)
        {
            List<Tdf> USER = new List<Tdf>
            {
                TdfInteger.Create("AID\0", pi.UserId),
                TdfInteger.Create("ALOC", pi.Loc),
                TdfInteger.Create("EXID\0", pi.UserId),
                TdfInteger.Create("ID\0\0", pi.UserId),
                TdfString.Create("NAME", pi.Profile.Name)
            };
            return TdfStruct.Create("USER", USER);
        }

        public static Tdf CreateUserDataStruct(Player pi, string name = "DATA")
        {
            List<Tdf> DATA = new List<Tdf>
            {
                BlazeHelper.CreateNETFieldUnion(pi, "ADDR"),
                BlazeHelper.CreateNQOSField(pi, "QDAT")
            };
            return TdfStruct.Create(name, DATA);
        }

    }
}
