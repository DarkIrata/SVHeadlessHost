using StardewModdingAPI;
using SVHeadlessHost.Manager;

namespace SVHeadlessHost
{
    public class ModEntry : Mod
    {
        public ModConfig Config { get; private set; }

        public ServerManager ServerManager { get; private set; }

        public GameManager GameManager { get; private set; }

        public DisplayManager DisplayManager { get; private set; }

        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();
            this.ServerManager = new ServerManager(this.Config, helper, this.Monitor);
            this.GameManager = new GameManager(this.Config, helper, this.Monitor, this.ServerManager);
            this.DisplayManager = new DisplayManager(this.Config, helper, this.Monitor, this.ServerManager);

            this.RegisterConsoleCommands(helper);
            this.RegisterEvents(helper);
        }

        private void RegisterConsoleCommands(IModHelper helper)
        {
            this.ServerManager.RegisterConsoleCommands();
            this.GameManager.RegisterConsoleCommands();
            this.DisplayManager.RegisterConsoleCommands();
        }

        // In theory it would make more sense to register to events when going into HeadlessMode
        private void RegisterEvents(IModHelper helper)
        {
            this.ServerManager.RegisterEvents();
            this.GameManager.RegisterEvents();
            this.DisplayManager.RegisterEvents();
        }
    }
}
