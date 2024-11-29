using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay
{
    public abstract class DynamicObject : GameObject
    {
        // Movement-specific properties
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public float Speed { get; set; }

        // Constructor
        public DynamicObject(Vector2 initialPosition, int width, int height, float speed)
            : base(initialPosition, width, height)
        {
            Speed = speed * 100;
        }

        // Update logic for movement
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
