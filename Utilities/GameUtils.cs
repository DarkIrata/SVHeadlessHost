using StardewModdingAPI;
using StardewValley;

namespace SVHeadlessHost.Utilities
{
    public static class GameUtils
    {
        public static void SendChatMessage(string message, IMonitor monitor = null)
        {
            Game1.chatBox.activate();
            Game1.chatBox.setText(message);
            if (monitor != null)
            {
                monitor.Log($"System to chat: {message}");
            }

            Game1.chatBox.chatBox.RecieveCommandInput('\r');
        }

        public static bool HasMoneyAndMailNotReceived(this Farmer farmer, int money, string mail) => farmer.Money >= money && !farmer.mailReceived.Contains(mail);

        public static void AddedMailReceivedWithCosts(this Farmer farmer, int money, params string[] events)
        {
            farmer.Money -= money;

            foreach (var gevent in events)
            {
                farmer.mailReceived.Add(gevent);
            }
        }
    }
}
