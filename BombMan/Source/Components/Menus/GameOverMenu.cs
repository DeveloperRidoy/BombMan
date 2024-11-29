using BombMan.Source.Core.Shared;
using System;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    internal class GameOverMenu : BaseMenu
    {
        public event Action OnRestartRequest;
        public event Action OnMainMenuRequest;

        public GameOverMenu(int score, int highScore, bool isNewHighScore) : base(
            "Game Over",
            new List<string>
            {
                $"Your score: {score}",
                isNewHighScore ? "Congratulations, new high score!" : $"High score: {highScore}"
            },
            new List<MenuItem> {
                new ("Restart", null),
                new ("Main Menu", null),
            },
            false
        )
        {
            _menuItems[0].Action = () => OnRestartRequest();
            _menuItems[1].Action = () => OnMainMenuRequest();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Art.GameOverSound.Play();
            // Load additional content
        }
    }
}
