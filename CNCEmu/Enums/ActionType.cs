using System;

namespace CNCEmu.Enums
{
    /// <summary>
    /// Enumeration for different action types.
    /// </summary>
    [Flags]
    public enum ActionType
    {
        StartGame,
        MovePlayer,
        
        GameService,
        UiService,
        OtherService
    }
}
