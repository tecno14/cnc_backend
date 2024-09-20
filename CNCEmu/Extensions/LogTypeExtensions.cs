using System.Drawing;

namespace CNCEmu.Extensions
{
    public static class LogTypeExtensions
    {
        public static Color GetColor(this Utils.Logger.LogType logType)
        {
            switch (logType)
            {
                case Utils.Logger.LogType.None: return Color.LightGray;  // Neutral
                case Utils.Logger.LogType.INFO: return Color.Blue;       // Informational
                case Utils.Logger.LogType.ERRR: return Color.Red;        // Error
                case Utils.Logger.LogType.WARN: return Color.Yellow;     // Warning
                case Utils.Logger.LogType.DEBG: return Color.Cyan;       // Debug
                case Utils.Logger.LogType.TRCE: return Color.Green;      // Trace
                case Utils.Logger.LogType.AUDT: return Color.Purple;     // Audit
                case Utils.Logger.LogType.TRAN: return Color.Magenta;    // Transaction
                case Utils.Logger.LogType.EVNT: return Color.Orange;     // Event
                case Utils.Logger.LogType.SECU: return Color.DarkGreen;  // Security
                case Utils.Logger.LogType.APPL: return Color.DarkBlue;   // Application
                case Utils.Logger.LogType.SYST: return Color.DarkGray;   // System
                default: return Color.Black;                             // Default
            }
        }
    }
}
