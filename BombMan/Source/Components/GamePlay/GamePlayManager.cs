using System;
using BombMan.Source.Core.Shared;
using BombMan.Source.Components.Menus;
using BombMan.Source.Components.GamePlay.Worlds;
using Microsoft.Xna.Framework.Media;

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

        public GamePlayManager(bool loadGame)
        {
            _gameWorld = new GameWorld(loadGame); // Automatically load or initialize the game world
            _gameWorld.OnGameOver += HandleGameOver;
        }

        public override void LoadContent()
        {
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
                _gameWorld.LoadContent();
            };

            _pauseMenu.OnSaveProgressRequest += () =>
            {
                _gameWorld.SaveToFile();
                Art.PauseSound.Play(); // mimics save sound
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


