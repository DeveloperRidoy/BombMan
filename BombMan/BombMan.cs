using BombMan.Source.Core;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BombMan.Source.Core.IO;
using Microsoft.Xna.Framework.Input.Touch;

namespace BombMan
{
    public class BombMan : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private GameManager _gameManager;

        public BombMan(Platform platform = Platform.Windows)
        {
            Resource.Platform = platform;

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (Resource.Platform == Platform.Android || Resource.Platform == Platform.iOS)
            {
                IsMouseVisible = false;
            }
            else
            {
                SetWindowSize();

                IsMouseVisible = true;
            }

            _graphics.ApplyChanges();
        }

        private void SetWindowSize()
        {
            int screenWidth = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.55);
            int screenHeight = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.7);

            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            
            if (Resource.Platform == Platform.Android || Resource.Platform == Platform.iOS)
            {
                // Enable touch input
                TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap
                    | GestureType.Flick | GestureType.FreeDrag;
            }

            Resource.GraphicsDevice = GraphicsDevice;
            Resource.ContentManager = Content;
            Resource.InputManager = new InputManager();

            // Initialize screen properties
            Resource.ScreenWidth = _graphics.PreferredBackBufferWidth;
            Resource.ScreenHeight = _graphics.PreferredBackBufferHeight;

            _gameManager = new(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Resource.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Art.LoadContent();
            _gameManager.LoadContent();
           
        }

        protected override void Update(GameTime gameTime)
        {
            Resource.UpdateGameTime = gameTime;
            Resource.InputManager.Update();

            _gameManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Resource.DrawGameTime = gameTime;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            Resource.SpriteBatch.Begin();
            _gameManager.Draw();
            Resource.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
