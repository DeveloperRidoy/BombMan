using Microsoft.Xna.Framework;
using BombMan.Source.Core.Shared;

namespace BombMan.Source.Components.GamePlay.Objects
{

    public class Floor : StaticObject
    {

        public Floor(Vector2 position, int width, int height)
            : base(position, width, height)
        {
        }

        public override void LoadContent()
        {
            Texture = Art.Floor;
        }

    }
}
