namespace BombMan.Source.Components.Menus
{
    public class MainMenu : BaseMenu
    {
        public MainMenu() : base("Main Menu")
        {
            AddMenuItem("New Game", StartNewGame);
            AddMenuItem("Load Game", LoadGame);
            AddMenuItem("About", ShowAbout);
            AddMenuItem("Help", ShowHelp);
            AddMenuItem("High Score", ShowHighScore);
            AddMenuItem("Exit", ExitGame);
        }

        private void StartNewGame()
        {
            InvokeStartGameRequestedtedEvent(false);
        }

        private void LoadGame()
        {
            InvokeStartGameRequestedtedEvent(true);
        }

        private void ShowAbout()
        {
            InvokeMenuChangedEvent(new AboutMenu());
        }

        private void ShowHelp()
        {
            InvokeMenuChangedEvent(new HelpMenu());
        }

        private void ShowHighScore()
        {
            InvokeMenuChangedEvent(new HighScoreMenu());
        }

        private  void ExitGame()
        {
            InvokeExitRequestedEvent();
        }
    }
}
