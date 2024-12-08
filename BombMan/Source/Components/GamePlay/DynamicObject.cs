using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay
{
    public abstract class DynamicObject(Vector2 initialPosition, int width, int height, float speed) : GameObject(initialPosition, width, height)
    {
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public float Speed { get; set; } = speed * 100;

        public override void Update()
        {
            if (!IsActive) return;

            Position += Velocity * Speed * (float)Resource.UpdateGameTime.ElapsedGameTime.TotalSeconds;
        }

        // Set velocity for movement
        public void SetVelocity(Vector2 direction)
        {
            Velocity = direction;
        }

        // Stop movement
        public void Stop()
        {
            Velocity = Vector2.Zero;
        }
    }
}
