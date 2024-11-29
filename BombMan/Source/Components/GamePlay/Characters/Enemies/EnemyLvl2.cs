using BombMan.Source.Components.GamePlay.Characters.Heroes;
using BombMan.Source.Components.GamePlay.Objects;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BombMan.Source.Components.GamePlay.Characters.Enemies
{
    internal class EnemyLvl2 : Enemy
    {
        private readonly Hero _hero;
        private Vector2 _previousPosition;
        private TimeSpan _cooldownTimer;
        private const float CooldownDuration = 5f; // 5 seconds cooldown

        public EnemyLvl2(Vector2 initialPosition, int width, int height, float speed, int worldWidth, int worldHeight, int hudHeight, int tileSize, Hero hero)
            : base(initialPosition, width, height, speed, worldWidth, worldHeight, hudHeight, tileSize)
        {
            _hero = hero;
            _previousPosition = initialPosition;
            _cooldownTimer = TimeSpan.Zero;
        }

        public override void LoadContent()
        {
            Texture = Art.EnemyLvl2;
        }

        public override void Update()
        {
            _previousPosition = Position;
            base.Update();

            if (_cooldownTimer > TimeSpan.Zero)
            {
                _cooldownTimer -= Resource.UpdateGameTime.ElapsedGameTime;
            }
            else
            {
                // Follow the hero
                Vector2 direction = _hero.Position - Position;
                if (direction.Length() > 0)
                {
                    direction.Normalize();
                    SetVelocity(direction);
                }
            }
        }

        public override void HandleCollisionWithHero(Hero hero)
        {
            if (GetBoundingRectangle().Intersects(hero.GetBoundingRectangle()))
            {
                hero.TakeDamage();
                // Reverse direction and start cooldown
                Velocity = -Velocity;
                _cooldownTimer = TimeSpan.FromSeconds(CooldownDuration);
            }
        }

        public override void HandleCollisionWithBlocks(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (GetBoundingRectangle().Intersects(block.GetBoundingRectangle()))
                {
                    // Revert to previous position to avoid teleporting
                    Position = _previousPosition;
                    // Change direction to avoid getting stuck
                    Vector2 direction = _hero.Position - Position;
                    direction = new Vector2(-direction.Y, direction.X); // Rotate direction by 90 degrees
                    SetVelocity(direction);
                    break;
                }
            }
        }
    }
}
