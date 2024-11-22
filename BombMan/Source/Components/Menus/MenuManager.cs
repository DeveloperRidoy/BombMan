using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace BombMan.Source.Components.Menus
{
    internal class MenuManager : BaseComponent
    {
        public event Action ExitRequested;

        private readonly Stack<BaseMenu> _menuStack;
        private BaseMenu _currentMenu;
        private Texture2D _posterImg;
        private Song _menuBackgroundMusic;

        public MenuManager()
        {
            _menuStack = new Stack<BaseMenu>();
            var mainMenu = new MainMenu();
            _menuStack.Push(mainMenu);
            _currentMenu = mainMenu;

            // React to menu changes
            foreach (var menu in _menuStack)
            {
                menu.MenuChanged += (Type menuType) => SwitchToMenu(menuType);
                menu.BackRequested += GoBack;
                menu.ExitRequested += () => ExitRequested?.Invoke();
            }
        }

        public override void LoadContent()
        {
            _posterImg = Resource.ContentManager.Load<Texture2D>("Images/BomberManPoster");
            _menuBackgroundMusic = Resource.ContentManager.Load<Song>("Audio/Menus/menuBackgroundMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_menuBackgroundMusic);
            _currentMenu.LoadContent();
        }

        private void DrawPosterImage()
        {
            Resource.SpriteBatch.Draw(_posterImg, new Vector2(0, 0), Color.White);
        }

        public override void Update()
        {
            _currentMenu.Update();
        }

        public override void Draw()
        {
            DrawPosterImage();
            _currentMenu.Draw();
        }

        public void SwitchToMenu(Type menuType)
        {
            if (_currentMenu.GetType() == menuType)
                return; // Avoid redundant menu switching

            var newMenu = (BaseMenu)Activator.CreateInstance(menuType);
            if (newMenu == null)
            {
                throw new InvalidOperationException($"No menu of type {menuType} exists in the menu manager.");
            }

            _menuStack.Push(newMenu);
            _currentMenu = newMenu;

            // Subscribe to events
            _currentMenu.MenuChanged += (Type type) => SwitchToMenu(type);
            _currentMenu.BackRequested += GoBack;
            _currentMenu.ExitRequested += () => ExitRequested?.Invoke();

            // Load content for the newly switched menu
            _currentMenu.LoadContent();
        }

        private void GoBack()
        {
            if (_menuStack.Count > 1)
            {
                _menuStack.Pop();
                _currentMenu = _menuStack.Peek();

                // Load content for the menu being switched to
                _currentMenu.LoadContent();
            }
        }
    }
}
