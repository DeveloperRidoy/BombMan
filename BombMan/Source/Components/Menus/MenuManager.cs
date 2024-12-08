using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using BombMan.Source.Components.GamePlay;

namespace BombMan.Source.Components.Menus
{
    internal class MenuManager : BaseComponent
    {
        public event Action ExitRequested;
        public event Action<bool> StartGameRequested;

        private readonly Stack<BaseMenu> _menuStack;
        private BaseMenu _currentMenu;
        private Song _menuBackgroundMusic;
        private readonly BomberManPoster _bomberManPoster;

        public MenuManager()
        {
            _bomberManPoster = new BomberManPoster(Vector2.Zero, Resource.ScreenWidth, Resource.ScreenHeight);
            _menuStack = new Stack<BaseMenu>();
            var mainMenu = new MainMenu();
            _menuStack.Push(mainMenu);
            _currentMenu = mainMenu;

            // handle menu changes
            foreach (var menu in _menuStack)
            {
                menu.MenuChanged += SwitchToMenu;
                menu.BackRequested += GoBack;
                menu.ExitRequested += () => ExitRequested();
                menu.StartGameRequested += (loadGame) => StartGameRequested(loadGame);
            }
        }

        public override void LoadContent()
        {
            _bomberManPoster.LoadContent();
            _menuBackgroundMusic = Resource.ContentManager.Load<Song>("Audio/Menus/menuBackgroundMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_menuBackgroundMusic);
            _currentMenu.LoadContent();
        }

        public override void Update()
        {
            _currentMenu.Update();
        }

        public override void Draw()
        {
            _bomberManPoster.Draw();
            _currentMenu.Draw();
        }

        public void SwitchToMenu(BaseMenu newMenu)
        {
            if (_currentMenu.GetType() == newMenu.GetType())
                return; // stop if the menu is already active

            _menuStack.Push(newMenu);
            _currentMenu = newMenu;

            // Subscribe to events
            _currentMenu.MenuChanged += SwitchToMenu;
            _currentMenu.BackRequested += GoBack;
            _currentMenu.ExitRequested += ExitRequested;

            // Load content for the newly switched menu
            _currentMenu.LoadContent();
        }

        private void GoBack()
        {
            if (_menuStack.Count > 1)
            {
                _menuStack.Pop();
                _currentMenu = _menuStack.Peek();
                MediaPlayer.Stop(); // Stop current menu music

                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(_menuBackgroundMusic);
                // Load content for the menu being switched to
                _currentMenu.LoadContent();
            }
        }
    }
}
