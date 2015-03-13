using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan
{
    /// <summary>
    /// Simple logging using the ingame console (F7)
    /// Usage:
    /// Log.Message("Hello World!");
    /// Log.Error("foobar");
    /// Log.Warning("Crap!");
    /// </summary>
    public static class Log
    {
        public static void Message(string s)
        {
            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, s);
        }

        public static void Error(string s)
        {
            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Error, s);
        }

        public static void Warning(string s)
        {
            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Warning, s);
        }
    }
}
