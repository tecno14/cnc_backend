using System;
using System.IO;

namespace CNCEmu.Services.Logger
{
    public class LogIO
    {
    }

    [Obsolete]
    public static class BackendLog
    {
        public static string logFile = "BackendLog.txt";
        public static void Clear()
        {
            if (File.Exists(logFile))
                File.Delete(logFile);
        }

        public static void Write(string s)
        {
            File.AppendAllText(logFile, s);
        }
    }

    [Obsolete]
    public static class CleanPackets
    {
        public static void Clean()
        {
            if (Directory.Exists("logs\\packets"))
                Directory.Delete("logs\\packets", true);
            if (Directory.Exists("logs"))
                Directory.Delete("logs", true);
        }
    }

    [Obsolete]
    public static class GenFiles
    {
        public static void CreatePackets()
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
            if (!Directory.Exists("logs\\packets"))
                Directory.CreateDirectory("logs\\packets");
        }
    }
}
