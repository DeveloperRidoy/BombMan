using BombMan.Source.Core;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BombMan.Source.Core.IO;

namespace BombMan
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private GameManager _gameManager;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1280; 
            _graphics.PreferredBackBufferHeight = 1080;

            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
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
