using System;
using System.Collections.Generic;

namespace CNCEmu.DTOs
{
    /// <summary>
    /// Data Transfer Object for game state.
    /// </summary>
    public class GameStateDto
    {
        public Guid Id { get; set; }
        public List<PlayerDto> Players { get; set; }
        // Other properties
    }
}
