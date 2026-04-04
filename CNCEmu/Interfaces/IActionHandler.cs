using CNCEmu.Enums;
using CNCEmu.Structs;
using System.Threading.Tasks;

namespace CNCEmu.Interfaces
{
    /// <summary>
    /// Interface for action handlers.
    /// </summary>
    public interface IActionHandler
    {
        Task HandleActionAsync(GameAction action);

        bool CanHandle(ActionType actionType);
    }
}
