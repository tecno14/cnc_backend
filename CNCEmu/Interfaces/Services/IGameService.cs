using CNCEmu.DTOs;
using CNCEmu.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CNCEmu.Interfaces.Services
{
    /// <summary>
    /// Interface for game service.
    /// </summary>
    public interface IGameService
    {
        Task StartGameAsync(GameStateDto gameStateDto);

        Task MovePlayerAsync(PlayerDto playerDto);

        Task<GameState> GetGameStateAsync(Guid gameId);

        Task<IEnumerable<GameState>> GetAllGameStatesAsync();

        Task DeleteGameAsync(Guid gameId);
    }
}
