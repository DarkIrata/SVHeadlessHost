using StardewModdingAPI;

namespace SVHeadlessHost.Manager
{
    public abstract class ManagerBase : BaseModClass
    {
        public ManagerBase(ModConfig config, IModHelper helper, IMonitor monitor)
            : base(config, helper, monitor)
        {
        }

        public virtual void RegisterConsoleCommands()
        {
        }

        public virtual void RegisterEvents()
        {
        }
    }
}
