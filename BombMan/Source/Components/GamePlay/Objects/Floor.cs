using Microsoft.Xna.Framework;
using BombMan.Source.Core.Shared;

namespace BombMan.Source.Components.GamePlay.Objects
{

    public class Floor(Vector2 position, int width, int height) : StaticObject(position, width, height)
    {
        public override void LoadContent()
        {
            Texture = Art.Floor;
        }

    }
}
