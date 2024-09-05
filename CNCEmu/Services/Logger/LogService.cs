using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace CNCEmu.Services.Logger
{
    public class LogService
    {
        private const int MAX_LOGS_PER_TYPE = 50;
        private readonly ObservableCollection<Log> Logs;

        /// <summary>
        /// Get or set the name of the logger
        /// </summary>
        public string LoggerName { get; private set; } = "";

        /// <summary>
        /// Get the logs
        /// </summary>
        public ObservableCollection<Log> PublicLogs => new ObservableCollection<Log>(Logs);

        // Define a delegate that represents the method signature for the event handler
        public delegate void LogEventHandler(LogEventArgs e);

        /// <summary>
        /// Event based on the delegate for new log
        /// </summary>
        public event LogEventHandler NewLogEvent;

        /// <summary>
        /// Event based on the delegate for removed log
        /// </summary>
        public event LogEventHandler RemovedLogEvent;

        /// <summary>
        /// Method to raise the event of new log
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNewLogEvent(LogEventArgs e) => NewLogEvent?.Invoke(e);

        /// <summary>
        /// Method to raise the event of removed log
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRemovedLogEvent(LogEventArgs e) => NewLogEvent?.Invoke(e);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the logger</param>
        public LogService(string name)
        {
            LoggerName = name;
            Logs = new ObservableCollection<Log>();
            Logs.CollectionChanged += Logs_CollectionChanged;
        }

        /// <summary>
        /// Handle the collection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Log log in e.OldItems)
                OnRemovedLogEvent(new LogEventArgs(log));
            foreach (Log log in e.NewItems)
                OnNewLogEvent(new LogEventArgs(log));
        }

        /// <summary>
        /// Save the log and raise the event
        /// </summary>
        /// <param name="log"></param>
        private void AddToLogList(Log log)
        {
            if (Logs.Count(l => l.Type == log.Type) >= MAX_LOGS_PER_TYPE)
                Logs.RemoveAt(0);
            Logs.Add(log);
        }

        public void Write(Exception exception) => 
            Write("", exception);

        public void Write(string name, Exception exception) =>
            Write(name, $"Error: {exception.Message}{(string.IsNullOrEmpty(exception.InnerException.Message) ? "" : $"{Environment.NewLine}{exception.InnerException.Message}")}", LogType.ERRR);

        public void Write(string message) => 
            Write(message, LogType.INFO);

        public void Write(string message, LogType type) => 
            Write("", message, type);

        public void Write(string name, string message) =>
           Write(name, message, LogType.INFO);

        public void Write(string name, string message, LogType type)
        {
            var log = new Log()
            {
                Name = name,
                Message = message,
                Type = type,
            };

            AddToLogList(log);
        }
    }
}
