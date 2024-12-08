using BombMan.Source.Components.GamePlay.Characters.Heroes;
using BombMan.Source.Components.GamePlay.Objects;
using BombMan.Source.Components.GamePlay.Items; // Add this using directive
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BombMan.Source.Components.GamePlay.Characters.Enemies
{
    public abstract class Enemy(Vector2 initialPosition, int width, int height, float speed, int hudHeight, int tileSize) : DynamicObject(initialPosition, width, height, speed)
    {
        protected static readonly Random Random = new();
        protected readonly int HudHeight = hudHeight;
        protected readonly int TileSize = tileSize;

        private float _floatingOffset = 0;
        private const float FloatingSpeed = 10f; // floating speed
        private const float FloatingAmplitude = 5f; // floating range

        public override void LoadContent()
        {
            Texture = Art.EnemyLvl1;
        }

        public override void Update()
        {
            base.Update();
            // update floating offset
            _floatingOffset += FloatingSpeed * (float)Resource.UpdateGameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw()
        {
            if (IsActive && Texture != null)
            {
                // Calculate floating offset
                float floatOffsetY = (float)Math.Sin(_floatingOffset) * FloatingAmplitude;

                Rectangle sourceRectangle = GetSourceRectangle();
                Rectangle destinationRectangle = new(
                    (int)Position.X,
                    (int)(Position.Y + floatOffsetY), // Add floating effect to Y-axis
                    Width,
                    Height
                );

                Resource.SpriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            }
        }

        public virtual void HandleCollisionWithHero(Hero hero)
        {
            if (GetBoundingRectangle().Intersects(hero.GetBoundingRectangle()))
            {
                hero.TakeDamage();
                // Reverse direction
                Velocity = -Velocity;
            }
        }

        public virtual void HandleCollisionWithBlocks(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (GetBoundingRectangle().Intersects(block.GetBoundingRectangle()))
                {
                    // Reverse direction
                    Velocity = -Velocity;
                    break;
                }
            }
        }

        public virtual void HandleCollisionWithBombs(List<Bomb> bombs)
        {
            foreach (var bomb in bombs)
            {
                if (GetBoundingRectangle().Intersects(bomb.GetBoundingRectangle()))
                {
                    // Reverse direction
                    Velocity = -Velocity;
                    break;
                }
            }
        }
    }
}
