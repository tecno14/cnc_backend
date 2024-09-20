using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using BlazeLibWV.Models;
using CNCEmu.Models;

namespace CNCEmu
{
    public class GameInfo
    {
        public int id;
        public bool isRunning;

        public TdfDoubleList ATTR;
        public uint GSTA;
        public long GSET;
        public long VOIP;
        public string VSTR;
        public string GNAM;
        public int[] slotUse;
        public Player[] players;

        public GameInfo()
        {
            players = new Player[32];
            slotUse = new int[32];
            for (int i = 0; i < 32; i++)
                slotUse[i] = -1;
        }

        public byte GetNextSlot()
        {
            for (byte i = 0; i < 32; i++)
                if (slotUse[i] == -1)
                    return i;
            return 255;
        }

        public void SetNextSlot(int id)
        {
            for (byte i = 0; i < 32; i++)
                if (slotUse[i] == -1)
                {
                    slotUse[i] = id;
                    return;
                }
        }

        public void RemovePlayer(int id)
        {
            for (byte i = 0; i < 32; i++)
                if (slotUse[i] == id)
                {
                    slotUse[i] = -1;
                    players[i] = null;
                    return;
                }
        }
    }
}
