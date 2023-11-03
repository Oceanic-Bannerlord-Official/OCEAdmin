using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Core.Plugin
{
    public class PluginManager
    {
        protected List<IPluginBase> _plugins;

        public List<IPluginBase> GetPlugins() => _plugins;

        public async Task RegisterPlugin(IPluginBase[] plugins)
        {
            foreach(IPluginBase plugin in plugins)
            {
                await RegisterPlugin(plugin);
            }
        }

        public async Task RegisterPlugin(IPluginBase plugin)
        {
            var existingPlugin = _plugins.FirstOrDefault(p => p.GetType() == plugin.GetType());

            if (existingPlugin != null)
            {
                _plugins.Remove(existingPlugin);
            }

            _plugins.Add(plugin);
            await plugin.Load();
        }

        public T GetPlugin<T>() where T : IPluginBase
        {
            return _plugins.OfType<T>().FirstOrDefault();
        }
    }
}
