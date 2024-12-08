using BombMan.Source.Core.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BombMan.Source.Core.Shared
{
    public static class Resource
    {
        public static Platform Platform { get; set; }
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
