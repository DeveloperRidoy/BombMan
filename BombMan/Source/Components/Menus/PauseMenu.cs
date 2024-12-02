using BombMan.Source.Core.Shared;
using System;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    internal class PauseMenu: BaseMenu
    {
        public event Action OnResumeRequest;
        public event Action OnRestartRequest;
        public event Action OnSaveProgressRequest;
        public event Action OnMainMenuRequest;

        public PauseMenu() : base("Pause Menu")
        {
            AddMenuItem("Resume", () => OnResumeRequest?.Invoke());
            AddMenuItem("Restart", () => OnRestartRequest?.Invoke());
            AddMenuItem("Save Progress", () => OnSaveProgressRequest?.Invoke());
            AddMenuItem("Main Menu", () => OnMainMenuRequest?.Invoke());
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Art.PauseSound.Play();
        }
    }
}
