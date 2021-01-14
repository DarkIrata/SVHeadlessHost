using StardewModdingAPI;

namespace SVHeadlessHost.Handler
{
    public abstract class HandlerBase : BaseModClass
    {
        protected HandlerBase(ModConfig config, IModHelper helper, IMonitor monitor)
            : base(config, helper, monitor)
        {
        }

        public abstract void Handle();
    }
}
