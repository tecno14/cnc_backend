using BlazeLibWV;
using BlazeLibWV.Models;
using CNCEmu.Constants;
using CNCEmu.Extensions;
using CNCEmu.Models;
using CNCEmu.Services;
using CNCEmu.Services.Network;
using CNCEmu.Utils.Logger;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class UtilComponent
    {
        public static void HandlePacket(Packet p, Player pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x02:
                    Ping(p, pi, ns);
                    break;
                case 0x05:
                    GetTelemetryServer(p, pi,ns);
                    break;
                case 0x07:
                    PreAuth(p, pi, ns);
                    break;
                case 0x08:
                    PostAuth(p,pi,ns);
                    break;
                case 0x16:
                    SetClientMetrics(p, pi,ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.UserId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [UTIL] " +  " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void Ping(Packet p, Player pi, NetworkStream ns)
        {
            pi.Timeout.Restart();
            List<Tdf> Result = new List<Tdf>
            {
                TdfInteger.Create("STIM", Blaze.GetUnixTimeStamp())
            };
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void GetTelemetryServer(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> Result = new List<Tdf>();
            List<Tdf> TELE = new List<Tdf>
            {
                TdfString.Create("ADRS", "http://river.data.ea.com"),
                TdfInteger.Create("ANON", 0),
                TdfString.Create("DISA", "AD,AF,AG,AI,AL,AM,AN,AO,AQ,AR,AS,AW,AX,AZ,BA,BB,BD,BF,BH,BI,BJ,BM,BN,BO,BR,BS,BT,BV,BW,BY,BZ,CC,CD,CF,CG,CI,CK,CL,CM,CN,CO,CR,CU,CV,CX,DJ,DM,DO,DZ,EC,EG,EH,ER,ET,FJ,FK,FM,FO,GA,GD,GE,GF,GG,GH,GI,GL,GM,GN,GP,GQ,GS,GT,GU,GW,GY,HM,HN,HT,ID,IL,IM,IN,IO,IQ,IR,IS,JE,JM,JO,KE,KG,KH,KI,KM,KN,KP,KR,KW,KY,KZ,LA,LB,LC,LI,LK,LR,LS,LY,MA,MC,MD,ME,MG,MH,ML,MM,MN,MO,MP,MQ,MR,MS,MU,MV,MW,MY,MZ,NA,NC,NE,NF,NG,NI,NP,NR,NU,OM,PA,PE,PF,PG,PH,PK,PM,PN,PS,PW,PY,QA,RE,RS,RW,SA,SB,SC,SD,SG,SH,SJ,SL,SM,SN,SO,SR,ST,SV,SY,SZ,TC,TD,TF,TG,TH,TJ,TK,TL,TM,TN,TO,TT,TV,TZ,UA,UG,UM,UY,UZ,VA,VC,VE,VG,VN,VU,WF,WS,YE,YT,ZM,ZW,ZZ"),
                TdfInteger.Create("EDCT", 1),
                TdfString.Create("FILT", "-GAME/COMM/EXPD"),
                TdfInteger.Create("LOC", 1701729619),
                TdfInteger.Create("MINR", 0),
                TdfString.Create("NOOK", "US, CA, MX"),
                TdfInteger.Create("PORT", 0x1BB),
                TdfInteger.Create("SDLY", 15000),
                TdfString.Create("SESS", "session_key_telemetry"),
                TdfString.Create("SKEY", ""),
                TdfInteger.Create("SPCT", 0),
                TdfString.Create("STIM", "Default"),
                TdfString.Create("SVNM", "telemetry - 3 - common")
            };

            Result.Add(TdfStruct.Create("TELE", TELE));

            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void PreAuth(Packet p, Player pi, NetworkStream ns)
        {
            uint utime = Blaze.GetUnixTimeStamp();

            List<Tdf> input = Blaze.ReadPacketContent(p);
            TdfStruct CDAT = (TdfStruct)input[0];
            TdfInteger TYPE = (TdfInteger)CDAT.Values[3];
            pi.IsServer = TYPE.Value != 0;

            if (pi.IsServer)  //Make as a Server !
            {
                pi.Game = new GameInfo();
                pi.Profile = ProfileService.Instance.ServerProfile;
                pi.UserId = 999;
            }

            TdfStruct CINF = (TdfStruct)input[1];
            TdfString CVER = (TdfString)CINF.Values[5];
            TdfInteger LOC = (TdfInteger)CINF.Values[8];
            pi.Loc = LOC.Value;
            pi.Version = CVER.Value;
            BlazeServer.Log("[CLNT] #" + pi.UserId + " is a " + (pi.IsServer ? "server" : "client"), System.Drawing.Color.Blue);

            List<Tdf> Result = new List<Tdf>
            {
                TdfString.Create("ASRC", "302123") //Authentication Source 300294
            };
            List<string> t = ("{30728} {1} {30729} {25} {30730} {27} {4} {28} {6} {7} {9} {10} {63490} {35} {15} {30720} {30722} {30723} {30724} {30726} {2000} {30727}").ConvertToStringList(); // Component ID's
            List<long> t2 = new List<long>();
            foreach (string v in t)
                t2.Add(Convert.ToInt64(v));
            Result.Add(TdfList.Create("CIDS", 0, t2.Count, t2));
            t = new List<string>();
            List<string> t3 = new List<string>();
            "{associationListSkipInitialSet ; 1} {blazeServerClientId ; GOS-BlazeServer-CNC-PC} {bytevaultHostname ; bytevault.gameservices.ea.com} {bytevaultPort ; 42210} {bytevaultSecure ; true} {capsStringValidationUri ; client-strings.xboxlive.com} {connIdleTimeout ; 90s} {defaultRequestTimeout ; 60s} {identityDisplayUri ; console2/welcome} {identityRedirectUri ; http://127.0.0.1/success} {nucleusConnect ; https://accounts.ea.com} {nucleusProxy ; https://gateway.ea.com} {pingPeriod ; 30s} {userManagerMaxCachedUsers ; 0} {voipHeadsetUpdateRate ; 1000} {xblTokenUrn ; accounts.ea.com} {xlspConnectionIdleTimeout ; 300}".ConvertDoubleStringList(out t, out t3);
            TdfDoubleList conf2 = TdfDoubleList.Create("CONF", 1, 1, t, t3, t.Count);
            List<Tdf> t4 = new List<Tdf>
            {
                conf2
            };
            Result.Add(TdfStruct.Create("CONF", t4));
            Result.Add(TdfString.Create("ESRC", "302123"));
            Result.Add(TdfString.Create("INST", "rts-client-pc"));
            Result.Add(TdfInteger.Create("MINR", 0));
            Result.Add(TdfString.Create("NASP", "cem_ea_id"));
            Result.Add(TdfString.Create("PILD", ""));
            Result.Add(TdfString.Create("PLAT", "pc"));


            List<Tdf> QOSS = new List<Tdf>();
            List<Tdf> BWPS = new List<Tdf>
            {
                TdfString.Create("PSA\0", ProviderInfo.ams_psa),
                TdfInteger.Create("PSP\0", ProviderInfo.ams_psp),
                TdfString.Create("SNA\0", ProviderInfo.ams_sna)
            };
            QOSS.Add(TdfStruct.Create("BWPS", BWPS));
            QOSS.Add(TdfInteger.Create("LNP\0", 0xA));

            List<TdfStruct> LTPS = new List<TdfStruct>();

            List<Tdf> LTPS1 = new List<Tdf>
            {
                TdfString.Create("PSA\0", ProviderInfo.ams_psa),
                TdfInteger.Create("PSP\0", ProviderInfo.ams_psp),
                TdfString.Create("SNA\0", ProviderInfo.ams_sna)
            };
            LTPS.Add(Blaze.CreateStructStub(LTPS1));

            List<Tdf> LTPS2 = new List<Tdf>
            {
                TdfString.Create("PSA\0", ProviderInfo.gru_psa),
                TdfInteger.Create("PSP\0", ProviderInfo.gru_psp),
                TdfString.Create("SNA\0", ProviderInfo.gru_sna)
            };
            LTPS.Add(Blaze.CreateStructStub(LTPS2));

            List<Tdf> LTPS3 = new List<Tdf>
            {
                TdfString.Create("PSA\0", ProviderInfo.iad_psa),
                TdfInteger.Create("PSP\0", ProviderInfo.iad_psp),
                TdfString.Create("SNA\0", ProviderInfo.iad_sna)
            };
            LTPS.Add(Blaze.CreateStructStub(LTPS3));

            List<Tdf> LTPS4 = new List<Tdf>
            {
                TdfString.Create("PSA\0", ProviderInfo.lax_psa),
                TdfInteger.Create("PSP\0", ProviderInfo.lax_psp),
                TdfString.Create("SNA\0", ProviderInfo.lax_sna)
            };
            LTPS.Add(Blaze.CreateStructStub(LTPS4));

            List<Tdf> LTPS5 = new List<Tdf>
            {
                TdfString.Create("PSA\0", ProviderInfo.nrt_psa),
                TdfInteger.Create("PSP\0", ProviderInfo.nrt_psp),
                TdfString.Create("SNA\0", ProviderInfo.nrt_sna)
            };
            LTPS.Add(Blaze.CreateStructStub(LTPS5));

            List<Tdf> LTPS6 = new List<Tdf>
            {
                TdfString.Create("PSA\0", ProviderInfo.syd_psa),
                TdfInteger.Create("PSP\0", ProviderInfo.syd_psp),
                TdfString.Create("SNA\0", ProviderInfo.syd_sna)
            };
            LTPS.Add(Blaze.CreateStructStub(LTPS6));

            t = ("{" + ProviderInfo.ams + "}" + "{" + ProviderInfo.gru + "}" + "{" + ProviderInfo.iad + "}" + "{" + ProviderInfo.lax + "}" + "{" + ProviderInfo.nrt + "}" + "{" + ProviderInfo.syd + "}").ConvertToStringList();
            QOSS.Add(TdfDoubleList.Create("LTPS", 1, 3, t, LTPS, LTPS.Count));

            QOSS.Add(TdfInteger.Create("SVID", 0x45410805)); // ServerID
            QOSS.Add(TdfInteger.Create("TIME", utime));

            Result.Add(TdfStruct.Create("QOSS", QOSS));
            Result.Add(TdfString.Create("RSRC", "302123"));
            Result.Add(TdfString.Create("SVER", "Blaze 13.3.1.8.0 (CL# 1148269)")); // Blaze Server Version 13.15.08.0 (CL# 9442625)
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);

            LogService.LogPacket("PreAuth", Convert.ToInt32(pi.UserId), buff); //Test
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void PostAuth(Packet p, Player pi, NetworkStream ns)
        {
            List<Tdf> Result = new List<Tdf>();
            List<Tdf> PSSList = new List<Tdf>
            {
                TdfString.Create("ADRS", "127.0.0.1"), //playersyncservice.ea.com
                TdfString.Create("PJID", "123071"),
                TdfInteger.Create("PORT", 80),
                TdfInteger.Create("RPRT", 9)
            };
            Result.Add(TdfStruct.Create("PSS\0", PSSList));
            List<Tdf> TELEList = new List<Tdf>
            {
                TdfString.Create("ADRS", "127.0.0.1"), //river.data.ea.com
                TdfInteger.Create("ANON", 0),
                TdfString.Create("DISA", "AD,AF,AG,AI,AL,AM,AN,AO,AQ,AR,AS,AW,AX,AZ,BA,BB,BD,BF,BH,BI,BJ,BM,BN,BO,BR,BS,BT,BV,BW,BY,BZ,CC,CD,CF,CG,CI,CK,CL,CM,CN,CO,CR,CU,CV,CX,DJ,DM,DO,DZ,EC,EG,EH,ER,ET,FJ,FK,FM,FO,GA,GD,GE,GF,GG,GH,GI,GL,GM,GN,GP,GQ,GS,GT,GU,GW,GY,HM,HN,HT,ID,IL,IM,IN,IO,IQ,IR,IS,JE,JM,JO,KE,KG,KH,KI,KM,KN,KP,KR,KW,KY,KZ,LA,LB,LC,LI,LK,LR,LS,LY,MA,MC,MD,ME,MG,MH,ML,MM,MN,MO,MP,MQ,MR,MS,MU,MV,MW,MY,MZ,NA,NC,NE,NF,NG,NI,NP,NR,NU,OM,PA,PE,PF,PG,PH,PK,PM,PN,PS,PW,PY,QA,RE,RS,RW,SA,SB,SC,SD,SG,SH,SJ,SL,SM,SN,SO,SR,ST,SV,SY,SZ,TC,TD,TF,TG,TH,TJ,TK,TL,TM,TN,TO,TT,TV,TZ,UA,UG,UM,UY,UZ,VA,VC,VE,VG,VN,VU,WF,WS,YE,YT,ZM,ZW,ZZ"),
                TdfString.Create("FILT", "-GAME/COMM/EXPD"),
                TdfInteger.Create("LOC\0", pi.Loc),
                TdfString.Create("NOOK", "US, CA, MX"),
                TdfInteger.Create("PORT", 80),
                TdfInteger.Create("SDLY", 0x3A98),
                TdfString.Create("SESS", "tele_sess"),
                TdfString.Create("SKEY", "some_tele_key"),
                TdfInteger.Create("SPCT", 0x4B),
                TdfString.Create("STIM", "Default")
            };
            Result.Add(TdfStruct.Create("TELE", TELEList));
            List<Tdf> TICKList = new List<Tdf>
            {
                TdfString.Create("ADRS", "127.0.0.1"), //ticker.ea.com
                TdfInteger.Create("PORT", 8999),
                TdfString.Create("SKEY", pi.UserId + ",127.0.0.1:80,rts-client-pc,10,50,50,50,50,0,0")
            };
            Result.Add(TdfStruct.Create("TICK", TICKList));
            List<Tdf> UROPList = new List<Tdf>
            {
                TdfInteger.Create("TMOP", 1),
                TdfInteger.Create("UID\0", pi.UserId)
            };
            Result.Add(TdfStruct.Create("UROP", UROPList));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void SetClientMetrics(Packet p, Player pi, NetworkStream ns)
        {
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Tdf>());
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

    }
}
