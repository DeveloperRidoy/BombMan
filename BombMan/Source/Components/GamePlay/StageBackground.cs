using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay
{
    public class StageBackground(Vector2 position, int width, int height, ELvl lvel) : StaticObject(position, width, height)
    {

        private readonly ELvl _level = lvel;

        public override void LoadContent()
        {
            // Load the background image
            Texture = _level switch
            {
                ELvl.Lvl1 => Art.StageBackgroundLvl1,
                ELvl.Lvl2 => Art.StageBackgroundLvl2,
                ELvl.Lvl3 => Art.StageBackgroundLvl3,
                _ => Art.StageBackgroundLvl1
            };
        }
    }
}