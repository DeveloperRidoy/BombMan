using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    public class AboutMenu : BaseMenu
    {
        public AboutMenu() : base(
            "About",
            new List<string> {
                "This game is a Bomberman-inspired game developed by XYZ.",
                "Version: 1.0.0",
                "Thank you for playing!"
            },           
             
            true
        )
        {
        }
    }
}

