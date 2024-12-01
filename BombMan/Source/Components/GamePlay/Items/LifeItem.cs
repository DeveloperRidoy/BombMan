using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BombMan.Source.Core.Shared;

namespace BombMan.Source.Components.GamePlay.Items
{
    internal class LifeItem: StaticObject
    {

        public LifeItem(Vector2 initialPosition, int width, int height) : base(initialPosition, width, height)
        {
        }
        public override void LoadContent()
        {
            Texture = Art.HealthIcon;
        }
    }
}
