using System;

namespace CNCEmu.Services.Logger
{
    public class LogEventArgs : EventArgs
    {
        public Log Log { get; private set; }

        public LogEventArgs(Log log)
        {
            Log = log;
        }
    }
}
