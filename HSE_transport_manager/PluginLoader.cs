using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Interfaces;

namespace HSE_transport_manager
{
    class PluginLoader
    {
        private const string PluginFolder = "plugins";
        public ITaxiService LoadTaxiService()
        {
            var files = Directory.GetFiles(PluginFolder, "*.dll");
            foreach (var file in files)
            {
                Assembly pluginAssembly = Assembly.LoadFrom(file);
                try
                {
                    var type = pluginAssembly.GetTypes().First(t => typeof (ITaxiService).IsAssignableFrom(t));
                    var service = Activator.CreateInstance(type) as ITaxiService;
                    return service;
                }
                catch
                {
                    // ignored
                }
            }
            return null;
        }
    }
}
