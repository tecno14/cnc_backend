using CNCEmu.DTOs;
using CNCEmu.Interfaces.Repositories;
using CNCEmu.Interfaces.Services;
using CNCEmu.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNCEmu.Services
{
    /// <summary>
    /// Contains business logic for game-related actions
    /// Purpose: The GameService class contains the core business logic related to game operations. It handles tasks such as starting a game, updating game states, and notifying clients about changes.
    /// Responsibilities:
    ///     Managing game state.
    ///     Interacting with the game repository to save and retrieve game data.
    ///     Performing game-related operations like starting a game or moving a player.
    ///     Notifying clients about game state changes.
    /// </summary>
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Task StartGameAsync(GameStateDto gameStateDto) // or (string gameStateData) 
        {
            // Deserialize game state data
            //var gameState = JsonConvert.DeserializeObject<GameState>(gameStateData);

            var gameState = new GameState
            {
                Id = gameStateDto.Id,
                Players = gameStateDto.Players,
                // Other properties
            };

            // Save game state
            _gameRepository.Save(gameState);

            // Notify clients about the new game state
            await NotifyClientsAsync(gameState);
        }

        public async Task MovePlayerAsync(string playerData) // or (PlayerDto playerDto)
        {
            // Deserialize player data
            var player = JsonConvert.DeserializeObject<Player>(playerData);

            // Update player state
            var gameState = _gameRepository.GetById(player.GameId);
            var existingPlayer = gameState.Players.FirstOrDefault(p => p.Id == player.Id);
            if (existingPlayer != null)
            {
                existingPlayer.Position = player.Position;
                _gameRepository.Save(gameState);

                // Notify clients about the updated player state
                await NotifyClientsAsync(gameState);
            }
        }

        public async Task<GameState> GetGameStateAsync(Guid gameId)
        {
            return _gameRepository.GetById(gameId);
        }

        public async Task<IEnumerable<GameState>> GetAllGameStatesAsync()
        {
            return _gameRepository.GetAll();
        }

        public async Task DeleteGameAsync(Guid gameId)
        {
            _gameRepository.Delete(gameId);
        }

        private async Task NotifyClientsAsync(GameState gameState)
        {
            // Implement client notification logic
        }

        public Task StartGameAsync(GameStateDto gameStateDto)
        {
            throw new NotImplementedException();
        }

        public Task MovePlayerAsync(PlayerDto playerDto)
        {
            throw new NotImplementedException();
        }
    }
}
