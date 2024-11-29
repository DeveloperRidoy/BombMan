using Microsoft.Xna.Framework;
using BombMan.Source.Core.Shared;
using BombMan.Source.Components.GamePlay.Characters.Heroes;

namespace BombMan.Source.Components.GamePlay
{
    public class HUD : BaseComponent
    {
        private readonly Hero _hero;
        private readonly GameWorld _gameWorld;
        private readonly int _healthIconSize;

        public HUD(Hero hero, GameWorld gameWorld)
        {
            _hero = hero;
            _gameWorld = gameWorld;
            _healthIconSize = 32;
        }

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
            // Draw the "Lives:" text
            Vector2 livesTextPosition = new(10, 10);
            Resource.SpriteBatch.DrawString(Art.DefaultFont, "Lives:", livesTextPosition, Color.White);

            // Draw the health icons directly
            for (int i = 0; i < _hero.Health; i++)
            {
                Vector2 heartPosition = new(80 + i * (_healthIconSize + 5), 10);
                Rectangle destinationRectangle = new((int)heartPosition.X, (int)heartPosition.Y, _healthIconSize, _healthIconSize);
                Resource.SpriteBatch.Draw(Art.HealthIcon, destinationRectangle, Color.White);
            }

            // Draw the level, score, and high score
            Vector2 levelTextPosition = new(10, 50);
            Resource.SpriteBatch.DrawString(Art.DefaultFont, $"Level: {_gameWorld.Level}", levelTextPosition, Color.White);

            Vector2 scoreTextPosition = new(10, 70);
            Resource.SpriteBatch.DrawString(Art.DefaultFont, $"Score: {_gameWorld.Score}", scoreTextPosition, Color.White);

            // Safely draw the high score
            string highScoreText = _gameWorld.HighScores != null && _gameWorld.HighScores.Count > 0
                ? $"High Score: {_gameWorld.HighScores[0]}"
                : "High Score: 0";
            Vector2 highScoreTextPosition = new(10, 90);
            Resource.SpriteBatch.DrawString(Art.DefaultFont, highScoreText, highScoreTextPosition, Color.White);
        }

    }
}

