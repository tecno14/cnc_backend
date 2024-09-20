using BlazeLibWV.Models;
using CNCEmu.Models;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class AccountsComponent
    {
        public static void HandlePacket(Packet p, Player pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x0:
                    //AuthLogin(p, pi, ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.UserId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [ACCOUNTS] " + " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void AuthLogin(Packet p, Player pi, NetworkStream ns)
        {

        }

    }
}
