using System;

namespace CNCEmu.Constants
{
    public static class General
    {
        public static string ProfileFoler { get; private set; } = "Backend\\Profiles";

        public static string ProfileFileExtension { get; private set; } = "profile";

        public static string ServerAccountName { get; private set; } = "rts.server";

        public static string ServerAccountEmail { get; internal set; } = "rts.server.pc@ea.com";


        public static int BlazerMaximumPlayers { get; private set; } = 100;

        public static TimeSpan BlazerMaximumTimeout { get; private set; } = TimeSpan.FromMinutes(5); // todo: isn't that loo long?
    }
}
