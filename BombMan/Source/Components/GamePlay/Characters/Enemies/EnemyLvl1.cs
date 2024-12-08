using BombMan.Source.Components.GamePlay.Characters.Heroes;
using BombMan.Source.Components.GamePlay.Objects;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BombMan.Source.Components.GamePlay.Characters.Enemies
{
    internal class EnemyLvl1(Vector2 initialPosition, int width, int height, float speed, int hudHeight, int tileSize) : Enemy(initialPosition, width, height, speed, hudHeight, tileSize)
    {
        public override void Update()
        {
            base.Update();

            // Random movement
            if (Random.NextDouble() < 0.01)
            {
                SetRandomDirection();
            }
        }

        private void SetRandomDirection()
        {
            Vector2 direction = new(
                (float)(Random.NextDouble() * 2 - 1),
                (float)(Random.NextDouble() * 2 - 1)
            );

            if (direction.Length() > 0)
            {
                direction.Normalize();
                SetVelocity(direction);
            }
        }
    }
}
