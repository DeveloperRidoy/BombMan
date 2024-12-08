using System;
using System.Collections.Generic;
using BombMan.Source.Core.IO;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BombMan.Source.Components.Menus
{
    public class BaseMenu : BaseComponent
    {
        public event Action<BaseMenu> MenuChanged;
        public event Action BackRequested;
        public event Action ExitRequested;
        public event Action<bool> StartGameRequested;

        protected Texture2D _backgroundTexture;
        protected List<MenuItem> _menuItems;
        protected int _selectedIndex;
        protected bool _isSubMenu;
        protected string _title;

        private readonly float _horizontalPadding;
        private readonly float _verticalPadding;

        public BaseMenu(string title, bool isSubMenu = false, float horizontalPadding = 120f, float verticalPadding = 30f)
        {
            _title = title;
            _menuItems = [];
            _isSubMenu = isSubMenu;
            _selectedIndex = 0;
            _horizontalPadding = horizontalPadding;
            _verticalPadding = verticalPadding;

            if (_isSubMenu)
            {
                _menuItems.Insert(0, new MenuItem("Back", () => BackRequested?.Invoke()));
            }
        }

        public void AddMenuItem(string name, Action action)
        {
            _menuItems.Add(new MenuItem(name, action));
        }

        public override void LoadContent()
        {
            _backgroundTexture = new Texture2D(Resource.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData([Color.White]);

            SetMenuItemPositions();
        }

        private void SetMenuItemPositions()
        {
            Vector2 titleSize = Art.DefaultFont.MeasureString(_title);
            float lineHeight = Art.DefaultFont.MeasureString("A").Y;

            // Calculate starting Y position after the title
            float menuStartY = (Resource.GraphicsDevice.Viewport.Height - (titleSize.Y + _menuItems.Count * (lineHeight + _verticalPadding))) / 2
                               + titleSize.Y
                               + _verticalPadding;

            // Set each menu item's position and size
            for (int i = 0; i < _menuItems.Count; i++)
            {
                MenuItem item = _menuItems[i];
                Vector2 size = Art.DefaultFont.MeasureString(item.Name);
                Vector2 position = new(
                    (Resource.GraphicsDevice.Viewport.Width - size.X) / 2,
                    menuStartY + i * (lineHeight + _verticalPadding)
                );

                item.SetPosition(position, size);
            }
        }

        public override void Update()
        {
            if (Resource.InputManager.IsEscapePressed())
            {
                if (_isSubMenu)
                {
                    InvokeBackRequestedEvent();
                }
                else
                {
                    InvokeExitRequestedEvent();
                }
            }

            if (Resource.InputManager.IsMoveUpPressed())
            {
                _selectedIndex = (_selectedIndex - 1 + _menuItems.Count) % _menuItems.Count;
            }

            if (Resource.InputManager.IsMoveDownPressed())
            {
                _selectedIndex = (_selectedIndex + 1) % _menuItems.Count;
            }

            // Update menu items
            for (int i = 0; i < _menuItems.Count; i++)
            {
                var item = _menuItems[i];
                item.IsSelected = i == _selectedIndex;

                if (item.IsMouseClicked() || item.IsTapped())
                {
                    _selectedIndex = i;
                    item.Action?.Invoke();
                }
            }

            if (Resource.InputManager.IsEnterPressed())
            {
                _menuItems[_selectedIndex]?.Action.Invoke();
            }
        }

        public override void Draw()
        {
            DrawBackground();
            DrawMenu();
        }

        public virtual void DrawBackground()
        {
            // Calculate the bounds of the menu area
            Vector2 titleSize = Art.DefaultFont.MeasureString(_title);
            float lineHeight = Art.DefaultFont.MeasureString("A").Y;

            float menuWidth = 0f;
            foreach (var item in _menuItems)
            {
                float itemWidth = Art.DefaultFont.MeasureString(item.Name).X;
                if (itemWidth > menuWidth) menuWidth = itemWidth;
            }
            menuWidth += _horizontalPadding * 2;

            float menuHeight = titleSize.Y + (_menuItems.Count * (lineHeight + _verticalPadding)) + _verticalPadding * 2;

            // Calculate the position for the menu background
            Vector2 menuPosition = new(
                (Resource.GraphicsDevice.Viewport.Width - menuWidth) / 2,
                (Resource.GraphicsDevice.Viewport.Height - menuHeight) / 2
            );

            // Draw the background
            Rectangle backgroundRectangle = new(
                (int)(menuPosition.X),
                (int)(menuPosition.Y),
                (int)(menuWidth),
                (int)(menuHeight)
            );

            Resource.SpriteBatch.Draw(_backgroundTexture, backgroundRectangle, Color.White * 0.95f);
        }

        public virtual void DrawMenu()
        {
            Vector2 titleSize = Art.DefaultFont.MeasureString(_title);

            // Calculate the position for the title
            Vector2 titlePosition = new(
                (Resource.GraphicsDevice.Viewport.Width - titleSize.X) / 2,
                (Resource.GraphicsDevice.Viewport.Height - (titleSize.Y + _menuItems.Count * (titleSize.Y + _verticalPadding))) / 2
            );

            // Draw the title
            Resource.SpriteBatch.DrawString(Art.DefaultFont, _title, titlePosition, Color.Black);

            // Draw each menu item
            foreach (var item in _menuItems)
            {
                item.Draw();
            }
        }

        protected void InvokeMenuChangedEvent(BaseMenu menu)
        {
            MenuChanged?.Invoke(menu);
        }

        protected void InvokeExitRequestedEvent()
        {
            ExitRequested?.Invoke();
        }

        protected void InvokeBackRequestedEvent()
        {
            BackRequested?.Invoke();
        }

        protected void InvokeStartGameRequestedtedEvent(bool loadGame)
        {
            StartGameRequested?.Invoke(loadGame);
        }
    }
}
