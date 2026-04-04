using CNCEmu.Structs;
using System.Threading.Tasks;

namespace CNCEmu.Interfaces.Services
{
    public interface IUiService
    {
        Task HandleActionAsync(GameAction action);
    }
}
