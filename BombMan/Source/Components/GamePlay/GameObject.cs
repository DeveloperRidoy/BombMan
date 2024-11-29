using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BombMan.Source.Core.Shared;

namespace BombMan.Source.Components.GamePlay
{
    public abstract class GameObject : BaseComponent
    {
        // Common properties for all game objects
        public Vector2 Position { get; set; } = Vector2.Zero;
        public int Width { get; set; }
        public int Height { get; set; }
        public Texture2D Texture { get; protected set; }
        public bool IsActive { get; set; } = true;

        // Constructor
        public GameObject(Vector2 initialPosition, int width, int height)
        {
            Position = initialPosition;
            Width = width;
            Height = height;
        }

        // Load content (to be implemented by derived classes)
        public override void LoadContent()
        {
        }

        // Update logic (optional for static objects)
        public override void Update()
        {
        }

        // Draw logic
        public override void Draw()
        {
            if (IsActive && Texture != null)
            {
                Rectangle sourceRectangle = GetSourceRectangle();
                Rectangle destinationRectangle = new ((int)Position.X, (int)Position.Y, Width, Height);
                Resource.SpriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            }
        }

        // Get Source Rectangle
        public virtual Rectangle GetSourceRectangle()
        {
            return new Rectangle(0, 0, Texture.Width, Texture.Height);
        }

        // Get Bounding Rectangle for collision detection
        public virtual Rectangle GetBoundingRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }
    }
}
