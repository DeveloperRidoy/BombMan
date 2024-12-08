using BombMan.Source.Components;
using BombMan.Source.Components.GamePlay;
using BombMan.Source.Components.Menus;

namespace BombMan.Source.Core
{

    public enum GameState
    {
        Menu, InGame, GameExit
    }

    internal class GameManager: BaseComponent
    {
        private readonly BombMan _game;
        private readonly MenuManager _menuManager;
        private GamePlayManager _gamePlayManager;

        public GameState CurrentGameState { get; private set; }

        public GameManager(BombMan game)
        {
            _game = game;
            _menuManager = new ();
            _menuManager.ExitRequested += ExitGame;
            _menuManager.StartGameRequested += StartGame;
            CurrentGameState = GameState.Menu;
        }

        private void StartGame(bool loadGame)
        {
            _gamePlayManager = new GamePlayManager(loadGame);
            _gamePlayManager.MainMenuRequested += () =>
            {
                CurrentGameState = GameState.Menu;
                _menuManager.LoadContent();
            };
            CurrentGameState = GameState.InGame;
            _gamePlayManager.LoadContent();
        }

        public void ExitGame () => CurrentGameState = GameState.GameExit;

        public override void LoadContent()
        {
            switch (CurrentGameState)
            {
                case GameState.Menu:
                    _menuManager.LoadContent();
                    break;
                case GameState.InGame:
                    _gamePlayManager.LoadContent();
                    break;
            }

        }

        public override void Update()
        {
            // Update game state
            switch (CurrentGameState)
            {
                case GameState.Menu:
                    _menuManager.Update();
                    break;
                case GameState.InGame:
                    _gamePlayManager.Update();
                    break;
                case GameState.GameExit:
                    // Exit game
                    _game.Exit();
                    break;
            }
        }

        public override void Draw()
        {

            switch (CurrentGameState)
            {
                case GameState.Menu:
                    _menuManager.Draw();
                    break;
                case GameState.InGame:
                    _gamePlayManager.Draw();
                    break;
            }

        }
    }
}
