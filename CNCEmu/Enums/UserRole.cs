using System;

namespace CNCEmu.Enums
{
    [Flags]
    public enum UserRole
    {
        Guest = 1 << 0,
        Player = 1 << 1,
        Spectator = 1 << 2,
        Host = 1 << 3
    }
}
