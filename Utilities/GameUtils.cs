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
    }
}
