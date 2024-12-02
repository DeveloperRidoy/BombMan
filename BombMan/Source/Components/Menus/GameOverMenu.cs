using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BombMan.Source.Components.Menus
{
    internal class GameOverMenu : BaseMenu
    {
        public event Action OnRestartRequest;
        public event Action OnMainMenuRequest;

        private readonly int _score;
        private readonly int _highScore;
        private readonly bool _isNewHighScore;

        public GameOverMenu(int score, int highScore, bool isNewHighScore) : base(
            "Game Over"
        )
        {
            _score = score;
            _highScore = highScore;
            _isNewHighScore = isNewHighScore;

            // Add menu items
            AddMenuItem("Restart", () => OnRestartRequest?.Invoke());
            AddMenuItem("Main Menu", () => OnMainMenuRequest?.Invoke());
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Art.GameOverSound.Play();
            SetMenuItemPositionsAfterScore();
        }

        private void SetMenuItemPositionsAfterScore()
        {
            Vector2 titleSize = Art.DefaultFont.MeasureString(_title);
            float lineHeight = Art.DefaultFont.MeasureString("A").Y;
            float padding = 20f;

            // Calculate starting Y position after the score and high score messages
            float menuStartY = (Resource.GraphicsDevice.Viewport.Height - (titleSize.Y + 2 * (lineHeight + padding) + _menuItems.Count * (lineHeight + padding))) / 2
                               + titleSize.Y
                               + 2 * (lineHeight + padding) // Space for "Score" and high score/congratulations message
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
            float menuWidth = 400f; // Fixed width for simplicity
            float menuHeight = titleSize.Y;

            // Add height for score and high score/congratulations message
            menuHeight += Art.DefaultFont.MeasureString("Score: ").Y * 2 + padding;

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

            // Draw the semi-transparent background
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
                (Resource.GraphicsDevice.Viewport.Height - (titleSize.Y + 2 * (titleSize.Y + padding) + _menuItems.Count * (titleSize.Y + padding))) / 2
            );

            // Draw the title
            Resource.SpriteBatch.DrawString(Art.DefaultFont, _title, titlePosition, Color.Black);

            // Draw the score
            Vector2 scorePosition = titlePosition + new Vector2(0, titleSize.Y + padding);
            Resource.SpriteBatch.DrawString(Art.DefaultFont, $"Score: {_score}", scorePosition, Color.DarkSlateGray);

            // Draw the high score or congratulations message
            Vector2 highScorePosition = scorePosition + new Vector2(0, Art.DefaultFont.MeasureString("Score: ").Y + padding);
            if (_isNewHighScore)
            {
                Resource.SpriteBatch.DrawString(Art.DefaultFont, "Congratulations! New High Score", highScorePosition, Color.Green);
            }
            else
            {
                Resource.SpriteBatch.DrawString(Art.DefaultFont, $"High Score: {_highScore}", highScorePosition, Color.Gray);
            }

            // Draw each menu item
            foreach (var item in _menuItems)
            {
                item.Draw();
            }
        }
    }
}
