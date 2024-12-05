using Microsoft.Xna.Framework;
using BombMan.Source.Core.Shared;
using BombMan.Source.Components.GamePlay.Characters.Heroes;
using BombMan.Source.Components.GamePlay.Worlds;
using Microsoft.Xna.Framework.Graphics;

namespace BombMan.Source.Components.GamePlay
{
    public class HUD(Hero hero, GameWorld gameWorld) : BaseComponent
    {
        private readonly Hero _hero = hero;
        private readonly GameWorld _gameWorld = gameWorld;
        private readonly int _healthIconSize = 32;

        public override void LoadContent()
        {
        }

        public override void Update()
        {
        }

        public void Update(Hero hero)
        {
            _hero.Health = hero.Health;
        }

        public override void Draw()
        {
            // Text scale for smaller text
            float headingTextScale = 1.3f;
            float textScale = 1f;

            // Calculate the horizontal center of the screen
            int screenWidth = Resource.ScreenWidth;

            // Calculate positions for "Lives:" text
            Vector2 livesTextSize = Art.DefaultFont.MeasureString("Lives:") * headingTextScale;
            float totalHealthIconsWidth = _hero.Health * (_healthIconSize + 5) - 5; // Total width of all icons

            // Combined width of "Lives:" text and icons
            float totalLivesWidth = livesTextSize.X + 10 + totalHealthIconsWidth;

            // Center the entire "Lives:" section horizontally
            float livesStartX = (screenWidth - totalLivesWidth) / 2;

            // Draw the "Lives:" text
            Vector2 livesTextPosition = new(livesStartX, 10);
            Resource.SpriteBatch.DrawString(Art.DefaultFont, "Lives:", livesTextPosition, Color.OrangeRed, 0f, Vector2.Zero, headingTextScale, SpriteEffects.None, 0f);

            // Draw the health icons next to "Lives:" text
            float healthIconsStartX = livesTextPosition.X + livesTextSize.X + 10; // 10px gap between text and icons
            for (int i = 0; i < _hero.Health; i++)
            {
                Vector2 heartPosition = new(healthIconsStartX + i * (_healthIconSize + 5), 10);
                Rectangle destinationRectangle = new((int)heartPosition.X, (int)heartPosition.Y, _healthIconSize, _healthIconSize);
                Resource.SpriteBatch.Draw(Art.HealthIcon, destinationRectangle, Color.OrangeRed);
            }

            // Adjust spacing for the next section
            float sectionSpacing = 20;

            // Draw Level, Score, and High Score closer together with smaller text
            string levelText = $"Level: {_gameWorld.Level}";
            Vector2 levelTextSize = Art.DefaultFont.MeasureString(levelText) * textScale;
            Vector2 levelTextPosition = new((screenWidth - levelTextSize.X) / 2, 10 + _healthIconSize + sectionSpacing);
            Resource.SpriteBatch.DrawString(Art.DefaultFont, levelText, levelTextPosition, Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 0f);

            string scoreText = $"Score: {_gameWorld.Score}";
            Vector2 scoreTextSize = Art.DefaultFont.MeasureString(scoreText) * textScale;
            Vector2 scoreTextPosition = new((screenWidth - scoreTextSize.X) / 2, levelTextPosition.Y + levelTextSize.Y + 5); // Smaller gap
            Resource.SpriteBatch.DrawString(Art.DefaultFont, scoreText, scoreTextPosition, Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 0f);

            string highScoreText = _gameWorld.HighScores != null && _gameWorld.HighScores.Count > 0
                ? $"High Score: {_gameWorld.HighScores[0]}"
                : "High Score: 0";
            Vector2 highScoreTextSize = Art.DefaultFont.MeasureString(highScoreText) * textScale;
            Vector2 highScoreTextPosition = new((screenWidth - highScoreTextSize.X) / 2, scoreTextPosition.Y + scoreTextSize.Y + 5); // Smaller gap
            Resource.SpriteBatch.DrawString(Art.DefaultFont, highScoreText, highScoreTextPosition, Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 0f);
        }

    }
}

