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
                        new ("Start Game", null),
                        new ("About", null),
                        new ("Help", null),
                        new ("High Score", null),
                        new ("Exit", null),
            }
        )
        {
            _menuItems[0].Action = StartGame;
            _menuItems[1].Action = ShowAbout;
            _menuItems[2].Action = ShowHelp;
            _menuItems[3].Action = ShowHighScore;
            _menuItems[4].Action = ExitGame;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        private void StartGame()
        {
            InvokeMenuChangedEvent(typeof(LevelSelectMenu));
        }

        private void ShowAbout()
        {
            InvokeMenuChangedEvent(typeof(AboutMenu));
        }

        private void ShowHelp()
        {
            InvokeMenuChangedEvent(typeof(HelpMenu));
        }

        private void ShowHighScore()
        {
            InvokeMenuChangedEvent(typeof(HighScoreMenu));
        }

        private  void ExitGame()
        {
            InvokeExitRequestedEvent();
        }
    }
}
