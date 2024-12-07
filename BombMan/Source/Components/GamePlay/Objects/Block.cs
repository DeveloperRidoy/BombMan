using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BombMan.Source.Core.Shared;

namespace BombMan.Source.Components.GamePlay.Objects
{

    public class Block(Vector2 position, int width, int height, ELvl lvl) : StaticObject(position, width, height)
    {
        public ELvl Lvl = lvl;

        public override void LoadContent()
        {
            Texture = Lvl switch
            {
                ELvl.Lvl1 => Art.BlockLvl1,
                ELvl.Lvl2 => Art.BlockLvl2,
                ELvl.Lvl3 => Art.BlockLvl3,
                _ => Art.BlockLvl1
            };
        }

    }
}
