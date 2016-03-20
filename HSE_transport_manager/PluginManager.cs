using System;
using System.IO;
using System.Linq;
using System.Reflection;
using HSE_transport_manager.Common.Interfaces;

namespace HSE_transport_manager
{
    class PluginManager
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

        public ITransportMonitoringService LoadMonitoringService()
        {
            var files = Directory.GetFiles(PluginFolder, "*.dll");
            foreach (var file in files)
            {
                Assembly pluginAssembly = Assembly.LoadFrom(file);
                try
                {
                    var type = pluginAssembly.GetTypes().First(t => typeof(ITransportMonitoringService).IsAssignableFrom(t));
                    var service = Activator.CreateInstance(type) as ITransportMonitoringService;
                    return service;
                }
                catch
                {
                    // ignored
                }
            }
            return null;
        }

        public ITransportSchedulerService LoadScheduleService()
        {
            var files = Directory.GetFiles(PluginFolder, "*.dll");
            foreach (var file in files)
            {
                Assembly pluginAssembly = Assembly.LoadFrom(file);
                try
                {
                    var type = pluginAssembly.GetTypes().First(t => typeof(ITransportSchedulerService).IsAssignableFrom(t));
                    var service = Activator.CreateInstance(type) as ITransportSchedulerService;
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
