using System;
using System.Collections.Generic;

namespace CNCEmu.DTOs
{
    /// <summary>
    /// Data Transfer Object for game state.
    /// </summary>
    public struct GameStateDto
    {
        public Guid Id { get; set; }
        public List<UserDto> Players { get; set; }
        // Other properties
    }
}
