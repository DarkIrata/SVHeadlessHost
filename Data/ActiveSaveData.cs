using System.IO;
using StardewModdingAPI;
using SVHeadlessHost.Enums;
using SVHeadlessHost.Manager;

namespace SVHeadlessHost.Data
{
    public class ActiveSaveData
    {
        private static string FilePath => Path.Combine(ServerManager.DataPath, $"{nameof(ActiveSaveData)}.json");

        public bool PetReceived { get; set; } = false;

        public bool CaveSelected { get; set; } = false;

        public bool CommunityCenterFadeSelected { get; set; } = false;

        public CommunityCenterFade CommunityCenterFade { get; set; } = CommunityCenterFade.CCBundles;

        public bool InputRequiredSetupCompleted => this.PetReceived && this.CaveSelected && this.CommunityCenterFadeSelected;

        public bool SetupCompleted { get; set; }

        public void Save(IModHelper helper) => helper.Data.WriteJsonFile(FilePath, this);

        public static ActiveSaveData Load(IModHelper helper)
        {
            return helper.Data.ReadJsonFile<ActiveSaveData>(FilePath) ?? new ActiveSaveData(); ;
        }
    }
}
