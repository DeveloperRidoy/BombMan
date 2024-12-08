using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BombMan.Source.Core.Shared;

namespace BombMan.Source.Components.GamePlay
{
    public abstract class GameObject(Vector2 initialPosition, int width, int height) : BaseComponent
    {
        // properties for game objects
        public Vector2 Position { get; set; } = initialPosition;
        public int Width { get; set; } = width;
        public int Height { get; set; } = height;
        public Texture2D Texture { get; protected set; }
        public bool IsActive { get; set; } = true;

        public override void LoadContent()
        {
        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            // only draw if active and texture is not null
            if (IsActive && Texture != null)
            {
                Rectangle sourceRectangle = GetSourceRectangle();
                Rectangle destinationRectangle = new ((int)Position.X, (int)Position.Y, Width, Height);
                Resource.SpriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            }
        }

        // get texture's source rectangle
        public virtual Rectangle GetSourceRectangle()
        {
            return new Rectangle(0, 0, Texture.Width, Texture.Height);
        }

        // get bounding rectangle for collision detection
        public virtual Rectangle GetBoundingRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }
    }
}
