using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using SVHeadlessHost.Data;

namespace SVHeadlessHost.Handler
{
    public class PetCaveHandler : HandlerBase
    {
        private readonly ActiveSaveData activeSaveData;

        public PetCaveHandler(ModConfig config, IModHelper helper, IMonitor monitor, ActiveSaveData activeSaveData)
            : base(config, helper, monitor)
        {
            this.activeSaveData = activeSaveData;
        }

        public override void Handle()
        {
            if (!this.activeSaveData.CaveSelected)
            {
                this.SelectCave();
            }
            else
            {
                this.ReceivePet();
            }

            if (this.activeSaveData.InputRequiredSetupCompleted)
            {
                this.monitor.Log("Rquired setup completed", LogLevel.Info);
                Game1.player.CanMove = true;
            }
        }

        private void ReceivePet()
        {
            if (!Game1.player.hasPet() && !this.activeSaveData.PetReceived)
            {
                var gameEvent = new Event();
                const string PetEventDialogQuestion = "pet";
                gameEvent.answerDialogue(PetEventDialogQuestion, 0);

                this.activeSaveData.PetReceived = true;
            }
        }

        private void SelectCave()
        {
            const int caveEventId = 65;

            if (!Game1.player.eventsSeen.Contains(caveEventId) &&
                !this.activeSaveData.CaveSelected &&
                Game1.activeClickableMenu == null)
            {
                var gameEvent = new Event();

                var answerChoices = new Response[2]
                {
                    new Response("Mushrooms", "Mushroom Cave"),
                    new Response("Bats", "Bat Cave")
                };

                Game1.currentLocation.createQuestionDialogue(
                    "Select cave type",
                    answerChoices,
                    (who, answer) =>
                    {
                        if (answer != "Bats")
                        {
                            Game1.MasterPlayer.caveChoice.Value = 2;
                            (Game1.getLocationFromName(nameof(FarmCave)) as FarmCave).setUpMushroomHouse();
                        }
                        else
                        {
                            Game1.MasterPlayer.caveChoice.Value = 1;
                        }

                        Game1.player.eventsSeen.Add(caveEventId);
                        this.activeSaveData.CaveSelected = true;
                    },
                    null);
            }
        }

        public override void Dispose()
        {
        }
    }
}
