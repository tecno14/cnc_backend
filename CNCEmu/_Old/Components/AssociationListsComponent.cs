using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class AssociationListsComponent
    {
        public static void HandlePacket(Packet p, Player pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x00:
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.UserId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [ASSOCIATIONLISTCOMP] " + " not found.", System.Drawing.Color.Red);
                    break;
            }
        }
    }
}
