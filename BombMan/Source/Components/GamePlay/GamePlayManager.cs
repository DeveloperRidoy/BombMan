using BombMan.Source.Components.Menus;
using BombMan.Source.Components.GamePlay.Characters.Heroes;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using System;

namespace BombMan.Source.Components.GamePlay
{
    internal class GamePlayManager : BaseComponent
    {
        private enum GameState
        {
            Playing,
            Paused
        }

        public event Action MainMenuRequested;
        private GameState _gameState = GameState.Playing;
        private PauseMenu _pausemenu;
        private readonly Hero _hero;

        public GamePlayManager(bool _)
        {
            _hero = new(new Vector2(100, 100), 72, 104, 100, 100);
        }
        public override void LoadContent()
        {
            switch (_gameState)
            {
                case GameState.Playing:
                    _hero.LoadContent();
                    break;
                case GameState.Paused:
                    _pausemenu.LoadContent();
                    break;
            }
        }

        public override void Update()
        {
            ListenForEscapeClick();
            switch (_gameState)
            {
                case GameState.Playing:
                    _hero.Update();
                    break;
                case GameState.Paused:
                    _pausemenu.Update();
                    break;
            }
        }
        public override void Draw()
        {
            switch (_gameState)
            {
                case GameState.Playing:
                    _hero.Draw();
                    break;
                case GameState.Paused:
                    _pausemenu.Draw();
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
                    InitializePauseMenu();
                }
                else if (_gameState == GameState.Paused)
                {
                    _gameState = GameState.Playing;
                    _pausemenu = null;
                }
            }
        }

        private void InitializePauseMenu()
        {
            _pausemenu = new();
            _pausemenu.LoadContent();
            _pausemenu.OnResumeRequest += () =>
            {
                _gameState = GameState.Playing;
                _pausemenu = null;
            };
            _pausemenu.OnRestartRequest += () =>
            {
                _gameState = GameState.Playing;
                _pausemenu = null;
            };
            _pausemenu.OnMainMenuRequest += () => MainMenuRequested();
        }

       
    }
}