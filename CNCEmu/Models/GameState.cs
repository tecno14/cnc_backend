using System;
using System.Collections.Generic;

namespace CNCEmu.Models
{
    /// <summary>
    /// Represents the game state.
    /// </summary>
    public class GameState
    {
        public Guid Id { get; set; }
        
        public List<Player> Players { get; set; }
        // Other properties
    }
}
