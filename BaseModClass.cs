using System;
using StardewModdingAPI;

namespace SVHeadlessHost
{
    public abstract class BaseModClass : IDisposable
    {
        public readonly ModConfig config;
        public readonly IModHelper helper;
        public readonly IMonitor monitor;

        public BaseModClass(ModConfig config, IModHelper helper, IMonitor monitor)
        {
            this.config = config;
            this.helper = helper;
            this.monitor = monitor;
        }

        public abstract void Dispose();
    }
}
