using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CNCEmu.Utils.Logger
{
    /// <summary>
    /// Log service
    /// </summary>
    public class LogService
    {
        #region Fields
        private readonly object _IOFlag = new object();
        #endregion

        #region Properties
        /// <summary>
        /// Logs collection
        /// </summary>
        public ObservableCollection<Log> Logs { get; } = new ObservableCollection<Log>();

        /// <summary>
        /// Get or set the name of the logger
        /// </summary>
        public string LoggerName { get; private set; } = "";

        public int MaxLogsPerType { get; private set; }

        public bool SaveToFile { get; private set; } = false;

        public string LogFileName { get; private set; } = string.Empty;
        #endregion

        #region Events
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
        protected virtual void OnNewLogEvent(LogEventArgs e) => 
            NewLogEvent?.Invoke(e);

        /// <summary>
        /// Method to raise the event of removed log
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRemovedLogEvent(LogEventArgs e) => 
            NewLogEvent?.Invoke(e);
        #endregion

        #region Methods
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the logger</param>
        /// <param name="maxLogsPerType">Maximum number of logs per type</param>
        /// <param name="logFile">Log file name, if empty will ignored</param>
        public LogService(string name, int maxLogsPerType = 50, bool saveToFile = false)
        {
            MaxLogsPerType = maxLogsPerType;
            LoggerName = name;
            if (saveToFile)
            {
                SaveToFile = true;
                LogFileName = Path.GetFullPath($"logs\\{name}_{UnixTimeNow()}.log");
                TryCreateLogFile();
            }
            Logs.CollectionChanged += Logs_CollectionChanged;
            Write($"Logger started {name}");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the number of seconds that have elapsed since 1970-01-01T00:00:00Z.
        /// </summary>
        /// <returns>The number of seconds that have elapsed since 1970-01-01T00:00:00Z.</returns>
        public static long UnixTimeNow() => 
            DateTimeOffset.UtcNow.ToUnixTimeSeconds();

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

            if (SaveToFile && e.NewItems != null && e.NewItems.Count > 0)
                TryWriteToFile(e.NewItems.Cast<string>().ToArray());
        }

        /// <summary>
        /// Try create the log file
        /// </summary>
        private void TryCreateLogFile()
        {
            lock (_IOFlag)
                try
                {
                    if (File.Exists(LogFileName))
                        return;

                    Directory.CreateDirectory(Path.GetDirectoryName(LogFileName));
                    File.Create(LogFileName).Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
        }

        /// <summary>
        /// Try write to the log file
        /// </summary>
        /// <param name="lines"></param>
        private void TryWriteToFile(string[] lines)
        {
            try
            {
                File.AppendAllLines(LogFileName, lines);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Save the log and raise the event
        /// </summary>
        /// <param name="log"></param>
        private void AddToLogList(Log log)
        {
            if (Logs.Count(l => l.Type == log.Type) >= MaxLogsPerType)
                Logs.RemoveAt(0);
            Logs.Add(log);
        }
        #endregion

        #region Public Methods
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

        #region Log Packet
        private static int PacketCounter = 0;
        private static readonly string PacketFolder = "logs\\packets";

        /// <summary>
        /// Log a packet
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handlerId"></param>
        /// <param name="data"></param>
        public static void LogPacket(string name, int handlerId, byte[] data)
        {
            try
            {
                if(!Directory.Exists(PacketFolder))
                    Directory.CreateDirectory(PacketFolder);
                File.WriteAllBytes($"{PacketFolder}\\PacketId_{++PacketCounter:d6}_HandlerId{handlerId}_Time{UnixTimeNow()}_{name}.bin", data);
            }
            catch
            {
                Debug.WriteLine("Failed to save packet");
            }
        }

        public static void ClearPackets() =>
            Directory.GetFiles(PacketFolder)
                .ToList()
                .ForEach(f => File.Delete(f));
        #endregion
        #endregion
        #endregion
    }
}
