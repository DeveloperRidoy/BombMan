using BombMan.Source.Core;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

            _gameManager = new(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Resource.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Resource.DefaultFont = Content.Load<SpriteFont>("Fonts/DefaultFont");
            _gameManager.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Resource.UpdateGameTime = gameTime;
            Resource.InputManager.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
