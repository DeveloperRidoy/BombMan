using BombMan.Source.Components.GamePlay.Worlds;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    public class HighScoreMenu : BaseMenu
    {
        private readonly List<int> _highScores;

        public HighScoreMenu() : base(
            "High Scores:",
            true // Indicates this is a submenu
        )
        {
            _highScores = GameWorld.LoadHighScores() ?? [];
        }

        public override void LoadContent()
        {
            base.LoadContent();
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Art.HighScoresBgm);
            SetMenuItemPositionsAfterHighScores();
        }

        private void SetMenuItemPositionsAfterHighScores()
        {
            Vector2 titleSize = Art.DefaultFont.MeasureString(_title);
            float lineHeight = Art.DefaultFont.MeasureString("A").Y;
            float padding = 20f;

            // Calculate vertical starting position after the high scores
            float menuStartY = (Resource.GraphicsDevice.Viewport.Height - (titleSize.Y + _highScores.Count * (lineHeight + padding) + _menuItems.Count * (lineHeight + padding))) / 2
                               + titleSize.Y
                               + _highScores.Count * (lineHeight + padding)
                               + padding;

            // Set each menu item's position and size
            for (int i = 0; i < _menuItems.Count; i++)
            {
                MenuItem item = _menuItems[i];
                Vector2 size = Art.DefaultFont.MeasureString(item.Name);
                Vector2 position = new(
                    (Resource.GraphicsDevice.Viewport.Width - size.X) / 2,
                    menuStartY + i * (lineHeight + padding)
                );

                item.SetPosition(position, size);
            }
        }

        public override void DrawBackground()
        {
            // Calculate the bounds of the menu area
            Vector2 titleSize = Art.DefaultFont.MeasureString(_title);
            float padding = 20f;
            float menuWidth = 400f;
            float menuHeight = titleSize.Y;

            // Add height for high scores
            foreach (var score in _highScores)
            {
                menuHeight += Art.DefaultFont.MeasureString(score.ToString()).Y + padding;
            }

            // Add height for menu items
            if (_menuItems != null && _menuItems.Count > 0)
            {
                foreach (var item in _menuItems)
                {
                    menuHeight += item.BoundingRectangle.Height + padding;
                }
            }

            // Add padding for the top, bottom, and between elements
            menuHeight += padding * 2;

            // Calculate the position for the menu background
            Vector2 menuPosition = new(
                (Resource.GraphicsDevice.Viewport.Width - menuWidth) / 2,
                (Resource.GraphicsDevice.Viewport.Height - menuHeight) / 2
            );

            // Draw the background
            Rectangle backgroundRectangle = new(
                (int)(menuPosition.X - padding),
                (int)(menuPosition.Y - padding),
                (int)(menuWidth + padding * 2),
                (int)(menuHeight + padding * 2)
            );

            Resource.SpriteBatch.Draw(_backgroundTexture, backgroundRectangle, Color.White * 0.95f);
        }

        public override void DrawMenu()
        {
            Vector2 titleSize = Art.DefaultFont.MeasureString(_title);
            float padding = 20f;

            // Calculate the position for the title
            Vector2 titlePosition = new(
                (Resource.GraphicsDevice.Viewport.Width - titleSize.X) / 2,
                (Resource.GraphicsDevice.Viewport.Height - (titleSize.Y + _highScores.Count * (titleSize.Y + padding) + _menuItems.Count * (titleSize.Y + padding))) / 2
            );

            // Draw the title
            Resource.SpriteBatch.DrawString(Art.DefaultFont, _title, titlePosition, Color.Black);

            // Draw high scores
            Vector2 highScorePosition = titlePosition + new Vector2(0, titleSize.Y + padding);
            foreach (var score in _highScores)
            {
                Resource.SpriteBatch.DrawString(Art.DefaultFont, $"Score: {score}", highScorePosition, Color.DarkSlateGray);
                highScorePosition.Y += Art.DefaultFont.MeasureString(score.ToString()).Y + padding;
            }

            // Draw menu items
            foreach (var item in _menuItems)
            {
                item.Draw();
            }
        }
    }
}
