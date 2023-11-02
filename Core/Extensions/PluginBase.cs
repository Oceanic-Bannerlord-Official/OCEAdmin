using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace OCEAdmin.Core.Extensions
{
    public class PluginBase : IPluginBase
    {
        public virtual string Name => "Plugin";

        public virtual async Task Load()
        {
            MPUtil.WriteToConsole(string.Format("Plugin '{0}' has been loaded.", Name));
        }

        public virtual void OnMultiplayerGameStart(Game game, object starterObject) { }

        public virtual void OnGameEnd(Game game) { }

        public virtual void OnPatch(Harmony harmony) { }
    }
}
