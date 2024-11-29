using BombMan.Source.Components.GamePlay.Characters.Heroes;
using BombMan.Source.Components.GamePlay.Objects;
using BombMan.Source.Components.GamePlay.Items; // Add this using directive
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BombMan.Source.Components.GamePlay.Characters.Enemies
{
    public abstract class Enemy : DynamicObject
    {
        protected static readonly Random Random = new();
        protected readonly int WorldWidth;
        protected readonly int WorldHeight;
        protected readonly int HudHeight;
        protected readonly int TileSize;

        protected Enemy(Vector2 initialPosition, int width, int height, float speed, int worldWidth, int worldHeight, int hudHeight, int tileSize)
            : base(initialPosition, width, height, speed)
        {
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
            HudHeight = hudHeight;
            TileSize = tileSize;
        }

        public override void LoadContent()
        {
            Texture = Art.EnemyLvl1; // Adjust as needed
        }

        public override void Update()
        {
            base.Update();

            // Ensure enemy does not move outside the grid
            Position = new Vector2(
                Math.Clamp(Position.X, 0, WorldWidth * TileSize - Width),
                Math.Clamp(Position.Y, HudHeight, WorldHeight * TileSize - Height + HudHeight)
            );
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

        // **Add this method**
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
