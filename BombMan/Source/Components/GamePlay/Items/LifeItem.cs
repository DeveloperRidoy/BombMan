using Microsoft.Xna.Framework;
using BombMan.Source.Core.Shared;

namespace BombMan.Source.Components.GamePlay.Items
{
    internal class LifeItem(Vector2 initialPosition, int width, int height) : StaticObject(initialPosition, width, height)
    {
        public override void LoadContent()
        {
            Texture = Art.HealthIcon;
        }
    }
}
