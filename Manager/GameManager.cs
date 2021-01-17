using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using SVHeadlessHost.Data;
using SVHeadlessHost.Enums;
using SVHeadlessHost.Handler;
using SVHeadlessHost.Utilities;

namespace SVHeadlessHost.Manager
{
    public class GameManager : ManagerBase
    {
        private const int CommunityCenterEventId = 611439;

        private readonly ServerManager serverManager;

        private ActiveSaveData ActiveSaveData { get; set; }

        private RunPathHandler RunPathHandler { get; set; }

        public GameManager(ModConfig config, IModHelper helper, IMonitor monitor, ServerManager serverManager)
            : base(config, helper, monitor)
        {
            this.serverManager = serverManager;
        }

        public override void RegisterEvents()
        {
            this.helper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;
            this.helper.Events.GameLoop.Saving += this.OnSaving;
            this.helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.ActiveSaveData = ActiveSaveData.Load(this.helper);

            this.RunPathHandler?.Dispose();
            this.RunPathHandler = new RunPathHandler(this.config, this.helper, this.monitor, this.ActiveSaveData);
        }

        public void OnSaving(object sender, SavingEventArgs e)
        {
            if (this.serverManager.IsActive && this.ActiveSaveData != null)
            {
                // Restore Data so when someone uses it with his normal character, it doesn't get saved
                // This could possible get the headless client suck at perk selection
                this.monitor.Log($"Restoring {nameof(HostPlayerData)}", LogLevel.Info);
                HostPlayerData.RestoreHostData(this.helper);
                this.monitor.Log($"Backuping {nameof(HostPlayerData)}", LogLevel.Info);
                HostPlayerData.BackupHostData(this.helper);

                if (Game1.activeClickableMenu is ShippingMenu menu)
                {
                    var totalString = "----";
                    var totals = this.helper.Reflection.GetFieldValueEx<List<int>>(menu, "categoryTotals", null, this.monitor);
                    var totalIndex = (int)ShippingMenuTotal.Total;
                    if (totals != null && totals.Count >= totalIndex)
                    {
                        totalString = totals[totalIndex].ToString();
                    }

                    this.monitor.Log($"Skipping {nameof(ShippingMenu)}. Total {totalString}G received for {Game1.dayOfMonth - 1} {Game1.CurrentSeasonDisplayName}", LogLevel.Info);

                    // okClicked is getting invoked by reflection since ShippingMenu checks for CanReceiveInput and so blocks leftClicks
                    this.helper.Reflection.GetMethod(menu, "okClicked").Invoke();
                }
            }
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!this.serverManager.IsActive)
            {
                return;
            }

            // Maybe handle it by the ServerManager? mhmmm not sure
            // Also this could cause problems when reactivating Headless mode.
            // Needs to be further investigated
            if (Game1.activeClickableMenu is TitleMenu)
            {
                this.serverManager.ToggleServer(false);
                return;
            }

            if (!this.ActiveSaveData.SetupCompleted)
            {
                this.SetupWorld();
                return;
            }

            this.RunPathHandler.EnforceRunPath();

            //lockPlayerChests
            //if (this.Config.lockPlayerChests)
            //{
            //    foreach (Farmer farmer in Game1.getOnlineFarmers())
            //    {
            //        if (farmer.currentLocation is Cabin cabin && farmer != cabin.owner)
            //        {
            //            //locks player inventories
            //            NetMutex playerinventory = this.Helper.Reflection.GetField<NetMutex>(cabin, "inventoryMutex").GetValue();
            //            playerinventory.RequestLock();

            //            //locks all chests
            //            foreach (SObject x in cabin.objects.Values)
            //            {
            //                if (x is Chest chest)
            //                {
            //                    //removed, the game stores color id's strangely, other colored chests randomly unlocking
            //                    /*if (chest.playerChoiceColor.Value.Equals(unlockedChestColor)) 
            //                    {
            //                        return;
            //                    }*/
            //                    //else
            //                    {
            //                        chest.mutex.RequestLock();
            //                    }
            //                }
            //            }
            //            //locks fridge
            //            cabin.fridge.Value.mutex.RequestLock();
            //        }
            //    }

            //}
        }

        private void SetupWorld()
        {
            if (!this.ActiveSaveData.InputRequiredSetupCompleted)
            {
                this.RunPathHandler.Handle();
                return;
            }

            // Checks and if needed unlocks the Community Center Event
            if (!Game1.player.eventsSeen.Contains(CommunityCenterEventId))
            {
                Game1.player.eventsSeen.Add(CommunityCenterEventId);
                Game1.MasterPlayer.mailReceived.Add("ccDoorUnlock");
                this.monitor.Log("Community Center unlocked!", LogLevel.Info);
            }

            if (this.config.HostHouseUpgradeLevel != 0 &&
                Game1.player.HouseUpgradeLevel != this.config.HostHouseUpgradeLevel)
            {
                // Since this only gets called on setup, there should be a possiblity to recall the upgrade (Command)
                this.UpgradeHouse();
            }

            // This isn't called in a else block so we can insure every setup step is completed
            if (this.ActiveSaveData.InputRequiredSetupCompleted)
            {
                this.ActiveSaveData.SetupCompleted = true;
                this.ActiveSaveData.Save(this.helper);
            }
        }

        private void UpgradeHouse()
        {
            Game1.player.HouseUpgradeLevel = this.config.HostHouseUpgradeLevel;
        }

        public override void Dispose()
        {
            this.ActiveSaveData.Save(this.helper);
        }
    }
}
