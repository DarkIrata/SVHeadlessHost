using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using SVHeadlessHost.Data;

namespace SVHeadlessHost.Manager
{
    public class ServerManager : ManagerBase
    {
        public static string DataPath => Path.Combine("data", Constants.SaveFolderName);

        public bool IsActive { get; private set; }

        public bool ServerEnabledWithHostServerMode => this.IsActive && Game1.options.enableServer;

        public ServerManager(ModConfig config, IModHelper helper, IMonitor monitor)
            : base(config, helper, monitor)
        {
        }

        public override void RegisterConsoleCommands()
        {
            this.helper.ConsoleCommands.Add("toggle_server", "Manuel toggle headless mode on / off", this.ToggleServer);
        }

        public override void RegisterEvents()
        {
            this.helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }

        private void ToggleServer(string cmd, string[] args)
        {
            this.ToggleServer(!this.IsActive);
        }

        public void ToggleServer(bool newState)
        {
            if (newState)
            {
                if (!Context.IsWorldReady)
                {
                    this.monitor.Log("No save is loaded!", LogLevel.Warn);
                    return;
                }

                if (Context.IsSplitScreen)
                {
                    this.monitor.Log("Split screen is not supported currently!", LogLevel.Warn);
                    return;
                }

                if (!Game1.IsServer)
                {
                    this.monitor.Log("Game was not started in Coop mode!", LogLevel.Warn);
                    return;
                }

                if (!Game1.options.enableServer)
                {
                    this.monitor.Log("Server is not enabled in options!", LogLevel.Warn);
                    return;
                }
            }

            if (newState)
            {
                this.IsActive = true;
                this.monitor.Log("Starting Headless Mode");
                this.StartServerMode();
            }
            else
            {
                this.IsActive = false;
                this.monitor.Log("Stopping Headless Mode");
                this.StopServerMode();
            }
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == this.config.ServerToggleKey)
            {
                this.ToggleServer(!this.IsActive);
            }
        }

        private void StartServerMode()
        {
            this.monitor.Log($"Backuping {nameof(HostPlayerData)}", LogLevel.Info);
            HostPlayerData.BackupHostData(this.helper);

            this.monitor.Log($"Backuping {nameof(HostGameOptions)}", LogLevel.Info);
            HostGameOptions.BackupHostOptions(this.helper);

            this.monitor.Log("Headless mode is now activated!", LogLevel.Info);
            Game1.chatBox.addInfoMessage($"[{nameof(SVHeadlessHost)}] The Host ACTIVATED headless mode!");
        }

        private void StopServerMode()
        {
            this.monitor.Log($"Restoring {nameof(HostPlayerData)}", LogLevel.Info);
            HostPlayerData.RestoreHostData(this.helper);

            this.monitor.Log($"Restoring {nameof(HostGameOptions)}", LogLevel.Info);
            HostGameOptions.RestoreHostOptions(this.helper);

            this.monitor.Log("Headless mode is now deactivated!", LogLevel.Info);
            Game1.chatBox.addInfoMessage($"[{nameof(SVHeadlessHost)}] The Host DEACTIVATED headless mode!");
        }

        private void DisplayHudMessage(string message)
        {
            Game1.displayHUD = true;
            Game1.addHUDMessage(new HUDMessage(message));
        }

        public override void Dispose()
        {
            if (this.IsActive)
            {
                this.StopServerMode();
            }
        }
    }
}
