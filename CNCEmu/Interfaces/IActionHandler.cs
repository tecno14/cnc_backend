using CNCEmu.Enums;
using CNCEmu.Models;
using System.Threading.Tasks;

namespace CNCEmu.Interfaces
{
    /// <summary>
    /// Interface for action handlers.
    /// </summary>
    public interface IActionHandler
    {
        Task HandleActionAsync(Action action);

        bool CanHandle(ActionType actionType);
    }
}
