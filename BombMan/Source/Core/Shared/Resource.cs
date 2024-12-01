using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombMan.Source.Core.Shared
{
    public static class Resource
    {
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static ContentManager ContentManager { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static GameTime DrawGameTime { get; set; }
        public static GameTime UpdateGameTime { get;set; }
        public static InputManager InputManager { get; set; }
        // Screen properties
        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }
    }
}
