using System.Diagnostics;

namespace CNCEmu
{
    public static class Helper
    {
        /// <summary>
        /// Open a url in the default browser
        /// </summary>
        /// <param name="url"></param>
        public static void OpenUrl(string url)=>
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // This is necessary to use the default browser
            });

        /// <summary>
        /// Run a shell command
        /// </summary>
        /// <param name="file"></param>
        /// <param name="command"></param>
        public static void RunShell(string file, string command) =>
            Process.Start(new ProcessStartInfo { FileName = file, Arguments = command });
    }
}
