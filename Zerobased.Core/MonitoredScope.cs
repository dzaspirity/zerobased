using System;
using Zerobased.Extensions;

namespace Zerobased
{
    public class MonitoredScope : IDisposable
    {
        private const char LevelChar = ' ';
        private static int _instancesCount;
        private readonly Action<TimeSpan, string> _onMessage;
        private readonly string _name;
        private readonly DateTime _ctorTime = DateTime.UtcNow;
        private readonly int _level;

        public MonitoredScope(string name, Action<TimeSpan, string> onMessage, bool messageOnEnter = true)
        {
            _level = ++_instancesCount;
            if (onMessage == null)
            {
                throw new ArgumentNullException(nameof(onMessage));
            }

            _name = name.IsNullOrWhiteSpace() ? DateTime.UtcNow.ToString("hh:mm:ss.ff") : name;
            _onMessage = onMessage;

            if (messageOnEnter)
            {
                RaiseMessage("Enter scope");
            }
        }

        public void Poke(string msg = null)
        {
            RaiseMessage(msg.IsNullOrEmpty() ? "poke" : msg);
        }

        private void RaiseMessage(string message)
        {
            _onMessage(DateTime.UtcNow - _ctorTime, "{0:G}{3}'{1}': {2}".FormatWith(DateTime.UtcNow, _name, message, string.Empty.PadLeft(_level*2, LevelChar)));
        }

        public void Dispose()
        {
            RaiseMessage("Leave scope");
            _instancesCount--;
        }

        public static MonitoredScope Console(string name = null, bool messageOnEnter = true)
        {
            var scope = new MonitoredScope(name, (ts, msg) => System.Console.WriteLine("{0} ({1})", msg, ts), messageOnEnter);
            return scope;
        }

        public static MonitoredScope Debug(string name = null, bool messageOnEnter = true)
        {
            var scope = new MonitoredScope(name, (ts, msg) => System.Diagnostics.Debug.WriteLine("{0} ({1})", msg, ts), messageOnEnter);
            return scope;
        }

        public static MonitoredScope File(string filePath, string name = null, bool messageOnEnter = true)
        {
            var scope = new MonitoredScope(name, (ts, msg) => System.IO.File.AppendAllLines(filePath, new[] { "{0} ({1})".FormatWith(msg, ts) }), messageOnEnter);
            return scope;
        }
    }
}
