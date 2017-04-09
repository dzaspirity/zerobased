using System;
using System.Collections.Generic;
using Zerobased.Extensions;

namespace Zerobased
{
    /// <summary>
    ///     Class provides ability to measure performance
    /// </summary>
    /// <example>
    ///     This sample shows how to use the <see cref="MonitoredScope"/> with console output.
    ///     <code>
    ///     using(var scope = MonitoredScope.Console("Send data"))
    ///     {
    ///         // make some actions
    ///         scope.Poke("Step 1 done");
    ///         // make other actions
    ///     }
    ///     </code>
    /// </example>
    public class MonitoredScope : IDisposable
    {
        /// <summary>
        ///     Delegate for printing message. <see cref="TimeSpan"/> parameter - time passed from creating the scope.
        ///     <see cref="string"/> parameter - message to print.
        /// </summary>
        public Action<TimeSpan, string> OnMessage { get; }

        /// <summary>
        ///     Name of scope (will be included to message)
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     UTC time of creating the scope
        /// </summary>
        public DateTime CtorTime { get; } = DateTime.UtcNow;

        /// <summary>
        ///     Creates an instance of <see cref="MonitoredScope"/>
        /// </summary>
        /// <param name="name">Name of new scope</param>
        /// <param name="onMessage">Delegate for printing message</param>
        public MonitoredScope(string name, Action<TimeSpan, string> onMessage)
        {
            onMessage = Check.NotNull(onMessage, nameof(onMessage));
            Name = name.IsNullOrWhiteSpace() ? DateTime.UtcNow.ToString("HH:mm:ss.ff") : name;
            OnMessage = onMessage;
            RaiseMessage("Enter scope");
        }

        /// <summary>
        ///     Send new message
        /// </summary>
        /// <param name="msg">Message to send</param>
        /// <returns>Current instance of <see cref="MonitoredScope"/></returns>
        public MonitoredScope Poke(string msg = null)
        {
            RaiseMessage(msg.IsNullOrEmpty() ? "poke" : msg);
            return this;
        }

        private void RaiseMessage(string message)
        {
            OnMessage(DateTime.UtcNow - CtorTime, $"{DateTime.UtcNow:G}'{Name}': {message}");
        }

        /// <summary>
        ///     Sends "Leave scope" message
        /// </summary>
        public void Dispose()
        {
            RaiseMessage("Leave scope");
        }

        /// <summary>
        ///     Creates new instance of <see cref="MonitoredScope"/> to write messages
        ///     to standard output (<see cref="System.Console.WriteLine(string)"/>)
        /// </summary>
        /// <param name="name">Name of new scope</param>
        public static MonitoredScope Console(string name = null)
        {
            var scope = new MonitoredScope(name, (ts, msg) => System.Console.WriteLine($"{msg} ({ts})"));
            return scope;
        }

        /// <summary>
        ///     Creates new instance of <see cref="MonitoredScope"/> to write messages
        ///     to Debug output (<see cref="System.Diagnostics.Debug.WriteLine(string)"/>)
        /// </summary>
        /// <param name="name">Name of new scope</param>
        public static MonitoredScope Debug(string name = null)
        {
            var scope = new MonitoredScope(name, (ts, msg) => System.Diagnostics.Debug.WriteLine($"{msg} ({ts})"));
            return scope;
        }

        /// <summary>
        ///     Creates new instance of <see cref="MonitoredScope"/> to write messages
        ///     to standard output (<see cref="System.IO.File.AppendAllLines(string, IEnumerable{string})"/>)
        /// </summary>
        /// <param name="filePath">Path of output file</param>
        /// <param name="name">Name of new scope</param>
        public static MonitoredScope File(string filePath, string name = null)
        {
            var scope = new MonitoredScope(name, (ts, msg) => System.IO.File.AppendAllLines(filePath, new[] { $"{msg} ({ts})" }));
            return scope;
        }
    }
}
