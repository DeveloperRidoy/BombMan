using System;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay
{
    internal class GameBackground: StaticObject
    {
        public GameBackground(Vector2 position, int width, int height)
            : base(position, width, height)
        {
        }

        public override void LoadContent()
        {
            // Load the background image
            Texture = Art.GameBackground;
        }
    }
}
