using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.Menus
{
    internal class MenuManager : BaseComponent
    {
        public event Action ExitRequested;

        private readonly List<BaseMenu> _menus;
        private BaseMenu _currentMenu;
        private BaseMenu _previousMenu;
        private Texture2D _posterImg;

        public MenuManager()
        {
            _menus = new List<BaseMenu>
            {
                new MainMenu(),
                new LevelSelectMenu(), 
                new AboutMenu(),
                new MainMenu(),
                new HelpMenu(),
                new HighScoreMenu(),
            };
            _currentMenu = _menus.First();

            // React to menu changes
            foreach (var menu in _menus)
            {
                menu.MenuChanged += (Type menuType) => SwitchToMenu(menuType);
                menu.BackRequested += GoBack;
                menu.ExitRequested += () => ExitRequested?.Invoke();
            }
        }

        public override void LoadContent()
        {
            _posterImg = Resource.ContentManager.Load<Texture2D>("Images/BomberManPoster");
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

            _previousMenu = _currentMenu;
            _currentMenu = _menus.FirstOrDefault(m => m.GetType() == menuType);

            if (_currentMenu == null)
            {
                throw new InvalidOperationException($"No menu of type {menuType} exists in the menu manager.");
            }

            // Load content for the newly switched menu
            _currentMenu.LoadContent();
        }

        private void GoBack()
        {
            if (_previousMenu != null)
            {
                (_previousMenu, _currentMenu) = (_currentMenu, _previousMenu);

                // Load content for the menu being switched to
                _currentMenu.LoadContent();
            }
        }
    }
}
