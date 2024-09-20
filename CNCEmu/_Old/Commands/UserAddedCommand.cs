using BlazeLibWV.Models;
using CNCEmu.Extensions;
using CNCEmu.Models;
using System;
using System.Collections.Generic;

namespace CNCEmu
{
    public static class UserAddedCommand
    {
        public static List<Tdf> UserAdded(Player pi)
        {
            List<Tdf> Result = new List<Tdf>();
            List<Tdf> DATA = new List<Tdf>();
            List<Tdf> USER = new List<Tdf>();
            List<Tdf> QDAT = new List<Tdf>();
            DATA.Add(TdfString.Create("BPS", "ams")); //Best PingSite
            DATA.Add(TdfString.Create("CTY", "")); //Country
            DATA.Add(TdfInteger.Create("HWFG", 0)); //Hardware Flags
            List<string> t = "{354} {376} {241} {177} {206} {37}".ConvertToStringList();
            List<long> t2 = new List<long>();
            foreach (string v in t)
                t2.Add(Convert.ToInt64(v));
            DATA.Add(TdfList.Create("PSLM", 0, t2.Count, t2)); //PingSite list # in ms
            DATA.Add(TdfStruct.Create("QDAT", QDAT)); //Quality of Service Data
            DATA.Add(TdfInteger.Create("UATT", 0)); //UserInfoAttribute
            Result.Add(TdfStruct.Create("DATA", DATA));

            USER.Add(TdfInteger.Create("AID", pi.UserId));
            USER.Add(TdfInteger.Create("ALOC", 1701729619));
            USER.Add(TdfInteger.Create("ID", pi.UserId));
            USER.Add(TdfString.Create("NAME", pi.Profile.Name));
            USER.Add(TdfInteger.Create("ORIG", pi.UserId));
            USER.Add(TdfInteger.Create("PIDI", 0));
            Result.Add(TdfStruct.Create("USER", USER));

            return Result;
        }
    }
}
