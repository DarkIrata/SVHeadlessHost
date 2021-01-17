using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using SVHeadlessHost.Data;
using SVHeadlessHost.Utilities;

namespace SVHeadlessHost.Handler
{
    public class RunPathHandler : HandlerBase
    {
        private readonly ActiveSaveData activeSaveData;

        public RunPathHandler(ModConfig config, IModHelper helper, IMonitor monitor, ActiveSaveData activeSaveData)
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
            else if (!this.activeSaveData.PetReceived)
            {
                this.ReceivePet();
            }
            else
            {
                this.SelectPath();
            }

            if (this.activeSaveData.InputRequiredSetupCompleted)
            {
                this.monitor.Log("Input required setup completed", LogLevel.Info);
                Game1.player.CanMove = true;
            }
        }

        private void ReceivePet()
        {
            if (Game1.activeClickableMenu == null && !Game1.player.hasPet() && !this.activeSaveData.PetReceived)
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

            if (Game1.activeClickableMenu == null &&
                !Game1.player.eventsSeen.Contains(caveEventId) &&
                !this.activeSaveData.CaveSelected &&
                Game1.activeClickableMenu == null)
            {
                const string mushroomsResponseKey = "Mushrooms";
                const string batssResponseKey = "Bats";
                var answerChoices = new Response[2]
                {
                    new Response(mushroomsResponseKey, "Mushroom Cave"),
                    new Response(batssResponseKey, "Bat Cave")
                };

                Game1.currentLocation.createQuestionDialogue(
                    "Select cave type",
                    answerChoices,
                    (who, answer) =>
                    {
                        if (answer != batssResponseKey)
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

        private void SelectPath()
        {
            if (Game1.activeClickableMenu == null && !this.activeSaveData.CommunityCenterFadeSelected)
            {
                const string ccBundleResponseKey = "CommunityCenter";
                const string JojaBundleResponseKey = "Joja";

                var answerChoices = new Response[2]
                {
                    new Response(ccBundleResponseKey, "Repair it yourself (Community Center Bundles)"),
                    new Response(JojaBundleResponseKey, "Go with Joja (Joja Market Bundles)")
                };

                Game1.currentLocation.createQuestionDialogue(
                    "Which path should be followed",
                    answerChoices,
                    (who, answer) =>
                    {
                        if (answer == JojaBundleResponseKey)
                        {
                            this.activeSaveData.CommunityCenterFade = Enums.CommunityCenterFade.JojaBundles;
                        }

                        this.activeSaveData.CommunityCenterFadeSelected = true;
                    },
                    null);
            }
        }

        internal void EnforceRunPath()
        {
            if (!this.activeSaveData.CommunityCenterFadeSelected &&
                this.activeSaveData.SetupCompleted)
            {
                return;
            }

            if (this.activeSaveData.CommunityCenterFade == Enums.CommunityCenterFade.CCBundles)
            {
                this.EnforceCommunityCenterPath();
            }
            else
            {
                this.EnforceJojaPath();
            }
        }

        private void EnforceCommunityCenterPath()
        {
            const int eventId = 191393; // Todo: Should be named by ID and not just eventId
            if (!Game1.player.eventsSeen.Contains(eventId)
                && Game1.player.mailReceived.Contains("ccCraftsRoom")
                && Game1.player.mailReceived.Contains("ccVault")
                && Game1.player.mailReceived.Contains("ccFishTank")
                && Game1.player.mailReceived.Contains("ccBoilerRoom")
                && Game1.player.mailReceived.Contains("ccPantry")
                && Game1.player.mailReceived.Contains("ccBulletin"))
            {
                if (Game1.getLocationFromName(nameof(CommunityCenter)) is CommunityCenter ccLocation)
                {
                    for (int index = 0; index < ccLocation.areasComplete.Count; ++index)
                    {
                        ccLocation.areasComplete[index] = true;
                    }

                    Game1.player.eventsSeen.Add(eventId);
                }
                else
                {
                    this.monitor.Log("Tried getting CommunityCenter Location by name. Got something else...");
                }
            }
        }

        private void EnforceJojaPath()
        {
            const int eventId = 502261; // Todo: Should be named by ID and not just eventId
            if (Game1.player.eventsSeen.Contains(eventId))
            {
                return;
            }

            if (this.HasMoneyAndMailNotReceived(10000, "JojaMember"))
            {
                Game1.player.Money -= 5000;
                Game1.player.mailReceived.Add("JojaMember");
                GameUtils.SendChatMessage("Bought Joja Membership");
            }
            else if (this.HasMoneyAndMailNotReceived(30000, "jojaBoilerRoom"))
            {
                Game1.player.Money -= 15000;
                Game1.player.mailReceived.Add("ccBoilerRoom");
                Game1.player.mailReceived.Add("jojaBoilerRoom");
                GameUtils.SendChatMessage("Bought Joja Minecarts");
            }
            else if (this.HasMoneyAndMailNotReceived(40000, "jojaFishTank"))
            {
                Game1.player.Money -= 20000;
                Game1.player.mailReceived.Add("ccFishTank");
                Game1.player.mailReceived.Add("jojaFishTank");
                GameUtils.SendChatMessage("Bought Joja Panning");
            }
            else if (this.HasMoneyAndMailNotReceived(50000, "jojaCraftsRoom"))
            {
                Game1.player.Money -= 25000;
                Game1.player.mailReceived.Add("ccCraftsRoom");
                Game1.player.mailReceived.Add("jojaCraftsRoom");
                GameUtils.SendChatMessage("Bought Joja Bridge");
            }
            else if (this.HasMoneyAndMailNotReceived(70000, "jojaPantry"))
            {
                Game1.player.Money -= 35000;
                Game1.player.mailReceived.Add("ccPantry");
                Game1.player.mailReceived.Add("jojaPantry");
                GameUtils.SendChatMessage("Bought Joja Greenhouse");
            }
            else if (this.HasMoneyAndMailNotReceived(80000, "jojaVault"))
            {
                Game1.player.Money -= 40000;
                Game1.player.mailReceived.Add("ccVault");
                Game1.player.mailReceived.Add("jojaVault");
                GameUtils.SendChatMessage("Bought Joja Bus");
                Game1.player.eventsSeen.Add(eventId);
            }
        }

        private bool HasMoneyAndMailNotReceived(int money, string mail) => Game1.player.Money >= money && !Game1.player.mailReceived.Contains(mail);

        public override void Dispose()
        {
        }
    }
}
