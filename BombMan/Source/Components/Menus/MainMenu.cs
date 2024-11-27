using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{

    public class MainMenu : BaseMenu
    {
        public MainMenu() : base(
            "Main Menu",
            new List<MenuItem> {
                        new ("New Game", null),
                        new ("Load Game", null),
                        new ("About", null),
                        new ("Help", null),
                        new ("High Score", null),
                        new ("Exit", null),
            }
        )
        {
            _menuItems[0].Action = StartNewGame;
            _menuItems[1].Action = LoadGame;
            _menuItems[2].Action = ShowAbout;
            _menuItems[3].Action = ShowHelp;
            _menuItems[4].Action = ShowHighScore;
            _menuItems[5].Action = ExitGame;
        }

        public override void LoadContent()
        {
            base.LoadContent();
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
