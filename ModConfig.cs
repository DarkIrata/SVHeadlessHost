using StardewModdingAPI;

namespace SVHeadlessHost
{
    public class ModConfig
    {
        public SButton ServerToggleKey { get; set; } = SButton.F9;

        public int MaxSkillLevel { get; set; } = 10; // Bypass for Skill select screen

        public int HostHouseUpgradeLevel { get; set; } = 3; // House progress bypass
    }
}
