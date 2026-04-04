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

        Task MovePlayerAsync(UserDto playerDto);

        Task<Game> GetGameStateAsync(Guid gameId);

        Task<IEnumerable<Game>> GetAllGameStatesAsync();

        Task DeleteGameAsync(Guid gameId);
    }
}
