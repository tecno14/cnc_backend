using System;

namespace CNCEmu.DTOs
{
    /// <summary>
    /// Data Transfer Object for player information.
    /// </summary>
    public class PlayerDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        // Other properties
    }
}
