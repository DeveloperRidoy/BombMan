using System;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay
{
    internal class BomberManPoster : StaticObject
    {
        public BomberManPoster(Vector2 position, int width, int height)
            : base(position, width, height)
        {
        }

        public override void LoadContent()
        {
            // Load the background image
            Texture = Art.BomberManPoster;
        }
    }
}
