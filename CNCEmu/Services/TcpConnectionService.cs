using CNCEmu.Interfaces;
using CNCEmu.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CNCEmu.Services
{
    /// <summary>
    /// Processes incoming TCP messages and routes them to the appropriate action handler.
    /// </summary>
    public class TcpConnectionService
    {
        private readonly IEnumerable<IActionHandler> _actionHandlers;

        public TcpConnectionService(IEnumerable<IActionHandler> actionHandlers)
        {
            _actionHandlers = actionHandlers;
        }

        public async Task ProcessMessageAsync(string message, TcpClient client)
        {
            // Parse the message and determine the action
            var action = ParseMessage(message);

            // Find the appropriate action handler
            var handler = _actionHandlers.FirstOrDefault(h => h.CanHandle(action.Type));
            if (handler != null)
            {
                await handler.HandleActionAsync(action);
            }

            // Send response back to client if needed
            var response = CreateResponse(action);
            var buffer = Encoding.UTF8.GetBytes(response);
            await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
        }

        private Action ParseMessage(string message)
        {
            // Implement message parsing logic
            return JsonConvert.DeserializeObject<Action>(message);
        }

        private string CreateResponse(Action action)
        {
            // Implement response creation logic
            return "Action processed";
        }
    }
}
