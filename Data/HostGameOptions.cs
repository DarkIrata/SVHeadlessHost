using System.IO;
using StardewModdingAPI;
using StardewValley;
using SVHeadlessHost.Manager;

namespace SVHeadlessHost.Data
{
    public class HostGameOptions
    {
        private static string FilePath => Path.Combine(ServerManager.DataPath, $"{nameof(HostGameOptions)}.json");

        public bool PauseOnFocusLost { get; set; }

        public static HostGameOptions BackupHostOptions(IModHelper helper)
        {
            var data = new HostGameOptions()
            {
                PauseOnFocusLost = Game1.options.pauseWhenOutOfFocus,
            };

            helper.Data.WriteJsonFile(FilePath, data);

            return data;
        }

        public static HostGameOptions RestoreHostOptions(IModHelper helper)
        {
            var data = helper.Data.ReadJsonFile<HostGameOptions>(FilePath) ?? new HostGameOptions();

            Game1.options.pauseWhenOutOfFocus = data.PauseOnFocusLost;

            return data;
        }
    }
}
