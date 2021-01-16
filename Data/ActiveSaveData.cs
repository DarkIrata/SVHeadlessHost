using System.IO;
using StardewModdingAPI;
using SVHeadlessHost.Manager;

namespace SVHeadlessHost.Data
{
    public class ActiveSaveData
    {
        private static string FilePath => Path.Combine(ServerManager.DataPath, $"{nameof(ActiveSaveData)}.json");

        public bool PetReceived { get; set; } = false;

        public bool CaveSelected { get; set; } = false;

        public bool InputRequiredSetupCompleted => this.PetReceived && this.CaveSelected;

        public bool SetupCompleted { get; set; }

        public void Save(IModHelper helper) => helper.Data.WriteJsonFile(FilePath, this);

        public static ActiveSaveData Load(IModHelper helper)
        {
            return helper.Data.ReadJsonFile<ActiveSaveData>(FilePath) ?? new ActiveSaveData(); ;
        }
    }
}
