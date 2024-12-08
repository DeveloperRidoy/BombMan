using BombMan.Source.Components.GamePlay.Worlds;
using BombMan.Source.Components.Menus;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework.Media;
using System;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay
{
    internal class GamePlayManager : BaseComponent
    {
        private enum GameState
        {
            Playing,
            Paused,
            GameOver
        }

        public event Action MainMenuRequested;

        private GameState _gameState = GameState.Playing;
        private PauseMenu _pauseMenu;
        private GameOverMenu _gameOverMenu;
        private GameWorld _gameWorld;
        private readonly GameBackground _gameBackground;

        public GamePlayManager(bool loadGame)
        {
            _gameWorld = new GameWorld(loadGame);
            _gameWorld.OnGameOver += HandleGameOver;
            _gameWorld.OnPauseMenuRequest += () => {
                _gameState = GameState.Paused;
                MediaPlayer.Pause(); // Pause the game music
                InitializePauseMenu();
            };
            _gameBackground = new GameBackground(Vector2.Zero, Resource.ScreenWidth, Resource.ScreenHeight);
        }

        public override void LoadContent()
        {
            _gameBackground?.LoadContent();

            switch (_gameState)
            {
                case GameState.Playing:
                    _gameWorld?.LoadContent();
                    break;
                case GameState.Paused:
                    _pauseMenu?.LoadContent();
                    break;
                case GameState.GameOver:
                    _gameOverMenu?.LoadContent();
                    break;
            }
        }

        public override void Update()
        {
            ListenForEscapeClick();

            switch (_gameState)
            {
                case GameState.Playing:
                    _gameWorld?.Update();
                    break;
                case GameState.Paused:
                    _pauseMenu?.Update();
                    break;
                case GameState.GameOver:
                    _gameOverMenu?.Update();
                    break;
            }
        }

        public override void Draw()
        {
            _gameBackground?.Draw();

            switch (_gameState)
            {
                case GameState.Playing:
                    _gameWorld?.Draw();
                    break;
                case GameState.Paused:
                    _pauseMenu?.Draw();
                    break;
                case GameState.GameOver:
                    _gameOverMenu?.Draw();
                    break;
            }
        }

        private void ListenForEscapeClick()
        {
            if (Resource.InputManager.IsEscapePressed())
            {
                if (_gameState == GameState.Playing)
                {
                    _gameState = GameState.Paused;
                    MediaPlayer.Pause(); // Pause the game music
                    InitializePauseMenu();
                }
                else if (_gameState == GameState.Paused)
                {
                    _gameState = GameState.Playing;
                    _pauseMenu = null;
                }
            }
        }

        private void InitializePauseMenu()
        {
            _pauseMenu = new PauseMenu();
            _pauseMenu.LoadContent();
            _pauseMenu.OnResumeRequest += () =>
            {
                _gameState = GameState.Playing;
                _pauseMenu = null;
                MediaPlayer.Resume(); // Resume the game music
            };
            _pauseMenu.OnRestartRequest += () =>
            {
                _gameState = GameState.Playing;
                _pauseMenu = null;
                _gameWorld = new GameWorld(false); // Reset to default world on restart
                _gameWorld.OnGameOver += HandleGameOver;
                _gameWorld.OnPauseMenuRequest += () =>
                {
                    _gameState = GameState.Paused;
                    MediaPlayer.Pause(); // Pause the game music
                    InitializePauseMenu();
                };
                _gameWorld.LoadContent();
            };

            _pauseMenu.OnSaveProgressRequest += () =>
            {
                _gameWorld.SaveToFile();
                Art.PauseSound.Play(); // Mimics save sound
            };
            _pauseMenu.OnMainMenuRequest += () => MainMenuRequested?.Invoke();
        }

        private void HandleGameOver(int score, int highScore, bool isNewHighScore)
        {
            _gameState = GameState.GameOver;
            _gameOverMenu = new GameOverMenu(score, highScore, isNewHighScore);
            _gameOverMenu.OnRestartRequest += RestartGame;
            _gameOverMenu.OnMainMenuRequest += ReturnToMainMenu;
            _gameOverMenu.LoadContent();
        }

        private void RestartGame()
        {
            _gameState = GameState.Playing;
            _gameWorld = new GameWorld(false);
            _gameWorld.OnGameOver += HandleGameOver;
            _gameWorld.LoadContent();
        }

        private void ReturnToMainMenu()
        {
            MainMenuRequested?.Invoke();
        }
    }
}
