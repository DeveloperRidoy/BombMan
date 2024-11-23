using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    public class AboutMenu : BaseMenu
    {

        private Texture2D _image;
        public AboutMenu() : base(
            "About",
            _image, true
        )
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _image = Resource.ContentManager.Load<Texture2D>("Images/About");
        }
    }
}

