using CNCEmu.Models;
using System.Threading.Tasks;

namespace CNCEmu.Interfaces.Services
{
    public interface IUiService
    {
        Task HandleActionAsync(Action action);
    }
}
