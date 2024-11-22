using BombMan.Source.Components;
using BombMan.Source.Components.Menus;

namespace BombMan.Source.Core
{

    public enum GameState
    {
        MainMenu, InGame, PausedMenu, GameOver, GameWon, GameLost, GameExit
    }

    internal class GameManager: BaseComponent
    {
        private readonly Game _game;
        private readonly MenuManager _menuManager;

        public GameState CurrentGameState { get; private set; }

        public GameManager(Game game)
        {
            _game = game;
            _menuManager = new ();
            _menuManager.ExitRequested += ExitGame;
            CurrentGameState = GameState.MainMenu;
        }

        public void ExitGame () => _game.Exit();

        public override void LoadContent()
        {
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    _menuManager.LoadContent();
                    break;
                case GameState.InGame:
                    // Load game
                    break;
                case GameState.PausedMenu:
                    _menuManager.LoadContent();
                    // Load pause menu
                    break;
                case GameState.GameOver:
                    // Load game over screen
                    break;
                case GameState.GameWon:
                    // Load game won screen
                    break;
                case GameState.GameLost:
                    // Load game lost screen
                    break;
            }

        }

        public override void Update()
        {
            // Update game state
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    _menuManager.Update();
                    break;
                case GameState.InGame:
                    // Load game
                    break;
                case GameState.PausedMenu:
                    _menuManager.Update();
                    break;
                case GameState.GameOver:
                    // Load game over screen
                    break;
                case GameState.GameWon:
                    // Load game won screen
                    break;
                case GameState.GameLost:
                    // Load game lost screen
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
                case GameState.MainMenu:
                    _menuManager.Draw();
                    break;
                case GameState.InGame:
                    // Draw game
                    break;
                case GameState.PausedMenu:
                    _menuManager.Draw();
                    break;
                case GameState.GameOver:
                    // Draw game over screen
                    break;
                case GameState.GameWon:
                    // Draw game won screen
                    break;
                case GameState.GameLost:
                    // Draw game lost screen
                    break;
            }

        }


    }
}
