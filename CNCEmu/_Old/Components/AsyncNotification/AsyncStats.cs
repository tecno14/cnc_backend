using BlazeLibWV;
using BlazeLibWV.Models;
using BlazeLibWV.Struct;
using CNCEmu.Models;
using CNCEmu.Services.Network;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class AsyncStats
    {
        public static void GetStatsAsyncNotification(Packet p, Player pi, NetworkStream ns)
        {
            var input = Blaze.ReadPacketContent(p);
            string statSpace = ((TdfString)input[2]).Value;
            long vid = ((TdfInteger)input[7]).Value;
            long eid = ((List<long>)((TdfList)input[1]).List)[0];
            var Result = new List<Tdf>
            {
                TdfString.Create("GRNM", statSpace),
                TdfString.Create("KEY\0", "No_Scope_Defined"),
                TdfInteger.Create("LAST", 1)
            };
            var STS = new List<Tdf>();
            List<TdfStruct> STAT = new List<TdfStruct>();
            var e0 = new List<Tdf>
            {
                TdfInteger.Create("EID\0", eid),
                TdfDoubleVal.Create("ETYP", new DoubleVal(30722, 1)),
                TdfInteger.Create("POFF", 0)
            };
            List<string> values = new List<string>();
            //if (statSpace == "crit")
            //    values.AddRange(new string[] { pi.profile.level.ToString(),
            //                                           pi.profile.xp.ToString(),
            //                                           "10000" });
            //else
            //    values.AddRange(new string[] { pi.profile.kit.ToString(),
            //                                           pi.profile.head.ToString(),
            //                                           pi.profile.face.ToString(),
            //                                           pi.profile.shirt.ToString()});
            e0.Add(TdfList.Create("STAT", 1, values.Count, values));
            STAT.Add(TdfStruct.Create("0", e0));
            STS.Add(TdfList.Create("STAT", 3, STAT.Count, STAT));
            Result.Add(TdfStruct.Create("STS\0", STS));
            Result.Add(TdfInteger.Create("VID\0", vid));
            byte[] buff = Blaze.CreatePacket(7, 0x32, 0, 0x2000, 0, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            BlazeServer.Log("[CLNT] #" + pi.UserId + " [0007:0032] GetStatsAsyncNotification");
        }
    }
}
