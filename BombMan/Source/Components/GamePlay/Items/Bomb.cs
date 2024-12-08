using System;
using Microsoft.Xna.Framework;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace BombMan.Source.Components.GamePlay.Items
{
    public class Bomb : StaticObject
    {
        private TimeSpan _timer;
        private readonly TimeSpan _fuseDuration = TimeSpan.FromSeconds(3); // Time before explosion
        private readonly TimeSpan _explosionDuration = TimeSpan.FromSeconds(1); // Duration of the explosion effect

        public bool HasExploded { get; private set; }
        private bool _isExplosionActive;

        public bool ExplosionHandled { get; set; }

        public Bomb(Vector2 position, int width, int height)
            : base(position, width, height)
        {
            _timer = TimeSpan.Zero;
            HasExploded = false;
            _isExplosionActive = false;
            IsActive = true;
            ExplosionHandled = false;
        }

        public override void LoadContent()
        {
            Texture = Art.Bomb;
        }

        public override void Update()
        {
            _timer += Resource.UpdateGameTime.ElapsedGameTime;

            if (!HasExploded)
            {
                // Bomb is not exploded
                if (_timer >= _fuseDuration)
                {
                    Explode();
                }
            }
            else if (_isExplosionActive)
            {
                // Explosion is active
                if (_timer >= _explosionDuration)
                {
                    // Explosion finished
                    _isExplosionActive = false;
                    IsActive = false; // Bomb is inactive after explosion
                }
            }
        }

        private void Explode()
        {
            HasExploded = true;
            _isExplosionActive = true;
            Texture = Art.Explosion;
            Art.ExplosionSound?.Play();
            _timer = TimeSpan.Zero; // Reset timer for explosion duration
        }

        public override void Draw()
        {
            if (IsActive && Texture != null)
            {
                Rectangle destinationRectangle;

                if (_isExplosionActive)
                {
                    int explosionWidth = Width * 2;
                    int explosionHeight = Height * 2;

                    // Use the explosion's destination rectangle
                    destinationRectangle = new Rectangle(
                        (int)(Position.X - (explosionWidth - Width) / 2),
                        (int)(Position.Y - (explosionHeight - Height) / 2),
                        explosionWidth,
                        explosionHeight
                    );
                }
                else
                {
                    // Use the bomb's destination rectangle
                    destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                }

                Resource.SpriteBatch.Draw(Texture, destinationRectangle, Color.White);
            }
        }

        public override Rectangle GetBoundingRectangle()
        {
            // Return the explosion's bounding rectangle if the explosion is active
            if (_isExplosionActive)
            {
                int explosionWidth = Width * 2;
                int explosionHeight = Height * 2;
                return new Rectangle(
                    (int)(Position.X - (explosionWidth - Width) / 2),
                    (int)(Position.Y - (explosionHeight - Height) / 2),
                    explosionWidth,
                    explosionHeight
                );
            }
            // Return the bomb's bounding rectangle if the explosion is not active
            else
            {
                return base.GetBoundingRectangle();
            }
        }
    }
}
