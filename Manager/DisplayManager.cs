using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using SVHeadlessHost.Enums;

namespace SVHeadlessHost.Manager
{
    public class DisplayManager : ManagerBase
    {
        private readonly ServerManager serverManager;

        public DisplayManager(ModConfig config, IModHelper helper, IMonitor monitor, ServerManager serverManager)
            : base(config, helper, monitor)
        {
            this.serverManager = serverManager;
        }

        public override void RegisterEvents()
        {
            this.helper.Events.Display.Rendered += this.OnRendered;
        }

        internal void OnRendered(object sender, RenderedEventArgs e)
        {
            if (this.serverManager.ServerEnabledWithHostServerMode)
            {
                //this.DrawTextBox(Game1.spriteBatch, 5, 100, Game1.dialogueFont, $"Server Mode On ({this.config.ServerToggleKey})");
                //this.DrawTextBox(Game1.spriteBatch, 5, 260, Game1.dialogueFont, $"Profit Margin: {Game1.player.difficultyModifier}%");
                //this.DrawTextBox(Game1.spriteBatch, 5, 340, Game1.dialogueFont, $"{Game1.server.connectionsCount} Clients");

                //var inviteCode = Game1.server.getInviteCode();
                //if (inviteCode != null)
                //{
                //    this.DrawTextBox(5, 420, Game1.dialogueFont, $"Invite Code: {inviteCode}");
                //}
            }
            var rect = new Rectangle(0, 256, 60, 60);
            this.DrawTextBox(Game1.spriteBatch, rect, new Vector2(0, 0), new Vector2(28, 17), Game1.dialogueFont, $"AAAAA", Color.White, TextBoxAlignment.TopLeft);
            this.DrawTextBox(Game1.spriteBatch, rect, new Vector2(0, 0), new Vector2(60, 17), Game1.dialogueFont, $"AAAAA", Color.White, TextBoxAlignment.Top);
            this.DrawTextBox(Game1.spriteBatch, rect, new Vector2(0, 0), new Vector2(28, 17), Game1.dialogueFont, $"AAAAA", Color.White, TextBoxAlignment.TopRight);
            this.DrawTextBox(Game1.spriteBatch, rect, new Vector2(0, 0), new Vector2(28, 17), Game1.dialogueFont, $"AAAAA", Color.White, TextBoxAlignment.MidLeft);
            this.DrawTextBox(Game1.spriteBatch, rect, new Vector2(0, 0), new Vector2(28, 17), Game1.dialogueFont, $"AAAAA", Color.White, TextBoxAlignment.Mid);
            this.DrawTextBox(Game1.spriteBatch, rect, new Vector2(0, 0), new Vector2(28, 17), Game1.dialogueFont, $"AAAAA", Color.White, TextBoxAlignment.MidRight);
            this.DrawTextBox(Game1.spriteBatch, rect, new Vector2(0, 0), new Vector2(28, 17), Game1.dialogueFont, $"AAAAA", Color.White, TextBoxAlignment.BottomLeft);
            this.DrawTextBox(Game1.spriteBatch, rect, new Vector2(0, 0), new Vector2(28, 17), Game1.dialogueFont, $"AAAAA", Color.White, TextBoxAlignment.Bottom);
            this.DrawTextBox(Game1.spriteBatch, rect, new Vector2(0, 0), new Vector2(28, 17), Game1.dialogueFont, $"AAAAA", Color.White, TextBoxAlignment.BottomRight);
        }

        public void DrawTextBox(
            SpriteBatch spriteBatch,
            Rectangle spriteSourceRect,
            Vector2 position,
            Vector2 padding,
            SpriteFont font,
            string message,
            Color textureColor,
            TextBoxAlignment align = TextBoxAlignment.TopLeft,
            float colorIntensity = 1f)
        {
            var fontWidth = (int)font.MeasureString(message).X;
            var fontHeight = (int)font.MeasureString(message).Y;
            var windowWidth = (int)(fontWidth + padding.X);
            var windowHeight = (int)(fontHeight + padding.Y);

            var x = (int)position.X;
            var y = (int)position.Y;


            // - 4 for shadow (8 / 2)
            switch (align)
            {
                case TextBoxAlignment.TopLeft:
                    break;
                case TextBoxAlignment.Top:
                    x = ((Game1.options.preferredResolutionX / 2) - (windowWidth / 2) - windowWidth) - x - 4 + (int)(padding.X / 2);
                    break;
                case TextBoxAlignment.TopRight:
                    x = Game1.options.preferredResolutionX - windowWidth - x;
                    break;
                case TextBoxAlignment.MidLeft:
                    y = ((Game1.options.preferredResolutionY / 2) - (windowHeight / 2) - windowHeight) - y - 4 + (int)(padding.Y / 2);
                    break;
                case TextBoxAlignment.Mid:
                    x = ((Game1.options.preferredResolutionX / 2) - (windowWidth / 2) - windowWidth) - x - 4 + (int)(padding.X / 2);
                    y = ((Game1.options.preferredResolutionY / 2) - (windowHeight / 2) - windowHeight) - y - 4 + (int)(padding.Y / 2);
                    break;
                case TextBoxAlignment.MidRight:
                    x = Game1.options.preferredResolutionX - windowWidth - x;
                    y = ((Game1.options.preferredResolutionY - windowHeight) / 2) - y;
                    break;
                case TextBoxAlignment.BottomLeft:
                    break;
                case TextBoxAlignment.Bottom:
                    x = ((Game1.options.preferredResolutionX / 2) - (windowWidth / 2) - windowWidth) - x - 4 + (int)(padding.X / 2);
                    break;
                case TextBoxAlignment.BottomRight:
                    break;
            }

            IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, spriteSourceRect, x, y, windowWidth, windowHeight, textureColor * colorIntensity);
            Utility.drawTextWithShadow(spriteBatch, message, font, new Vector2(x + (windowWidth / 2) - (fontWidth / 2), y + (windowHeight / 2) - (fontHeight / 2)), Game1.textColor);

            //new Rectangle(0, 256, 60, 60), x - width / 2, y, width, height + 4, textureColor * colorIntensity);
            //new Rectangle(0, 256, 60, 60), x - width, y, width, height + 4, textureColor * colorIntensity);
            //new Vector2(x + 16 - width / 2, y + 16), Game1.textColor);
            //new Vector2(x + 16 - width, y + 16), Game1.textColor);

            //switch (aligsn)
            //{
            //    case 0:
            //        IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height + 4, textureColor * colorIntensity);
            //        Utility.drawTextWithShadow(spriteBatch, message, font, new Vector2(x + 16, y + 16), Game1.textColor);
            //        break;
            //    case 1:
            //        IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x - width / 2, y, width, height + 4, textureColor * colorIntensity);
            //        Utility.drawTextWithShadow(spriteBatch, message, font, new Vector2(x + 16 - width / 2, y + 16), Game1.textColor);
            //        break;
            //    case 2:
            //        IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x - width, y, width, height + 4, textureColor * colorIntensity);
            //        Utility.drawTextWithShadow(spriteBatch, message, font, new Vector2(x + 16 - width, y + 16), Game1.textColor);
            //        break;
            //}
        }

        public override void Dispose()
        {
        }
    }
}
