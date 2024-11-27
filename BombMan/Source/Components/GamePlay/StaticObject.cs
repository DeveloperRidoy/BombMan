using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay
{
    public class StaticObject : GameObject
    {
        // Constructor
        public StaticObject(Vector2 position, int width, int height)
            : base(position, width, height)
        {
        }

        public override void LoadContent()
        {
        }
    }
}
