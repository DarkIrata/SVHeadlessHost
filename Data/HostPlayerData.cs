using System.IO;
using StardewModdingAPI;
using StardewValley;
using SVHeadlessHost.Manager;

namespace SVHeadlessHost.Data
{
    public class HostPlayerData
    {
        private static string FilePath => Path.Combine(ServerManager.DataPath, $"{nameof(HostPlayerData)}.json");

        public int NewSkillPointsToSpend { get; set; }

        public int FarmingLevel { get; set; }

        public int MiningLevel { get; set; }

        public int CombatLevel { get; set; }

        public int ForagingLevel { get; set; }

        public int FishingLevel { get; set; }

        public int LuckLevel { get; set; }

        public static HostPlayerData BackupHostData(IModHelper helper)
        {
            var data = new HostPlayerData()
            {
                NewSkillPointsToSpend = Game1.player.NewSkillPointsToSpend,
                FarmingLevel = Game1.player.FarmingLevel,
                MiningLevel = Game1.player.MiningLevel,
                CombatLevel = Game1.player.CombatLevel,
                ForagingLevel = Game1.player.ForagingLevel,
                FishingLevel = Game1.player.FishingLevel,
                LuckLevel = Game1.player.LuckLevel,
            };

            helper.Data.WriteJsonFile(FilePath, data);

            return data;
        }

        public static HostPlayerData RestoreHostData(IModHelper helper)
        {
            var data = helper.Data.ReadJsonFile<HostPlayerData>(FilePath) ?? new HostPlayerData();

            Game1.player.NewSkillPointsToSpend = data.NewSkillPointsToSpend;
            Game1.player.FarmingLevel = data.FarmingLevel;
            Game1.player.MiningLevel = data.MiningLevel;
            Game1.player.CombatLevel = data.CombatLevel;
            Game1.player.ForagingLevel = data.ForagingLevel;
            Game1.player.FishingLevel = data.FishingLevel;
            Game1.player.LuckLevel = data.LuckLevel;

            return data;
        }
    }
}
