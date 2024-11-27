using System;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    internal class PauseMenu: BaseMenu
    {
        public event Action OnResumeRequest;
        public event Action OnRestartRequest;
        public event Action OnMainMenuRequest;

        public PauseMenu() : base(
            "Pause Menu",
            new List<MenuItem> {
                new ("Resume", null),
                new ("Restart", null),
                new ("Main Menu", null),
            },
            false
        )
        {
            _menuItems[0].Action = () => OnResumeRequest();
            _menuItems[1].Action = () => OnRestartRequest();
            _menuItems[2].Action = () => OnMainMenuRequest();
        }
    }
}
