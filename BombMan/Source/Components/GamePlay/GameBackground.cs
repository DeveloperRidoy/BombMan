using System;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay
{
    internal class GameBackground(Vector2 position, int width, int height) : StaticObject(position, width, height)
    {
        public override void LoadContent()
        {
            // Load the background image
            Texture = Art.GameBackground;
        }
    }
}
