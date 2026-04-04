using BlazeLibWV.Models;
using System;

namespace CNCEmu.Models
{
    /// <summary>
    /// Represents the game state.
    /// </summary>
    public class Game
    {
        public Guid Id { get; } = Guid.NewGuid();

        public User[] Players { get; } = new User[32];
        // Other properties

        [Obsolete]
        public int id;
        public bool isRunning;

        public TdfDoubleList ATTR;
        public uint GSTA;
        public long GSET;
        public long VOIP;
        public string VSTR;
        public string GNAM;
        public int[] slotUse;

        [Obsolete]
        public User[] players;

        public Game()
        {
            players = new User[32];
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
