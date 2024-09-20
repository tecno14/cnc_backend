using CNCEmu.Enums;
using CNCEmu.Interfaces;
using CNCEmu.Interfaces.Repositories;
using CNCEmu.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CNCEmu.ActionHandlers
{
    /// <summary>
    /// Handles game-related actions.
    /// Purpose: The GameActionHandler class is responsible for handling specific game-related actions received from clients. 
    ///     It acts as a mediator that processes incoming actions and delegates the actual game logic to the GameService.
    /// Responsibilities:
    ///     Parsing and handling game-related actions.
    ///     Delegating the execution of these actions to the GameService.
    ///     Ensuring that the correct service is called based on the action type.
    /// </summary>
    public class GameActionHandler : IActionHandler
    {
        private readonly IGameRepository _gameRepository;

        public GameActionHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task HandleActionAsync(Action action)
        {
            // Handle game-related actions
            var gameState = JsonConvert.DeserializeObject<GameState>(action.Data);
            _gameRepository.Save(gameState);
            await NotifyClientsAsync(gameState);
        }

        public bool CanHandle(ActionType actionType)
        {
            return actionType == ActionType.GameService;
        }

        private async Task NotifyClientsAsync(GameState gameState)
        {
            // Implement client notification logic
        }
    }

}
