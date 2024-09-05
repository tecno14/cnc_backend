using System;

namespace CNCEmu.Services.Logger
{
    public class Log
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public LogType Type { get; set; } = LogType.INFO;

        public override string ToString() =>
            $"{DateTime.ToShortTimeString()} - {(Type == LogType.None ? "" : $"[{Type}] - ")}({(string.IsNullOrEmpty(Name) ? "" : $"{Name} : ")}){Message}";
    }
}
