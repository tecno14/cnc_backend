using CNCEmu.Interfaces.Repositories;
using CNCEmu.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CNCEmu.Repositories
{
    /// <summary>
    /// Handles data access for game states.
    /// </summary>
    public class GameRepository : IGameRepository
    {
        private readonly List<GameState> _gameStates = new List<GameState>();

        public void Save(GameState gameState)
        {
            var existingGameState = _gameStates.FirstOrDefault(g => g.Id == gameState.Id);
            if (existingGameState != null)
            {
                // Update existing game state
                existingGameState.Players = gameState.Players;
                // Update other properties as needed
            }
            else
            {
                // Add new game state
                _gameStates.Add(gameState);
            }
        }

        public GameState GetById(Guid id)
        {
            return _gameStates.FirstOrDefault(g => g.Id == id);
        }

        public void Delete(Guid id)
        {
            var gameState = _gameStates.FirstOrDefault(g => g.Id == id);
            if (gameState != null)
            {
                _gameStates.Remove(gameState);
            }
        }

        public IEnumerable<GameState> GetAll()
        {
            return _gameStates;
        }

        // Other data access methods
    }
}
