using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BombMan.Source.Core.Shared;

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
        protected List<string> _description;

        protected Texture2D _image;
        public BaseMenu(string title, List<MenuItem> items, bool isSubMenu = false)
        {
            _title = title;
            _menuItems = new List<MenuItem>(items);
            _isSubMenu = isSubMenu;
            _selectedIndex = 0; // Default to the first menu option
            _description = new List<string>();

            if (_isSubMenu)
            {
                _menuItems.Insert(0, new MenuItem("Back", () => BackRequested?.Invoke()));
            }
        }

        public BaseMenu(string title, List<string> description, bool isSubMenu = false)
        {
            _title = title;
            _description = description;
            _menuItems = new List<MenuItem>();
            _isSubMenu = isSubMenu;

            if (_isSubMenu)
            {
                _menuItems.Insert(0, new MenuItem("Back", () => BackRequested?.Invoke()));
            }
        }

        public BaseMenu(string title, Texture2D image, bool isSubMenu = false)
        {
            _title = title;
            _description = new List<string>();
            _menuItems = new List<MenuItem>();
            _isSubMenu = isSubMenu;
            _image = image;

            if (_isSubMenu)
            {
                _menuItems.Insert(0, new MenuItem("Back", () => BackRequested?.Invoke()));
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

        public override void LoadContent()
        {
            // Create a 1x1 white texture for drawing the background
            _backgroundTexture = new Texture2D(Resource.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { Color.White });
        }

        public override void Update()
        {
            // Update logic for the menu
            if (Resource.InputManager.IsMoveUpPressed())
            {
                _selectedIndex = (_selectedIndex - 1 + _menuItems.Count) % _menuItems.Count;
            }
            if (Resource.InputManager.IsMoveDownPressed())
            {
                _selectedIndex = (_selectedIndex + 1) % _menuItems.Count;
            }
            if (Resource.InputManager.IsEnterPressed())
            {
                _menuItems[_selectedIndex]?.Action.Invoke();
            }
            if (Resource.InputManager.IsEscapePressed())
            {
                if (_isSubMenu)
                {
                    BackRequested?.Invoke();
                } else
                {
                    ExitRequested?.Invoke();
                }
            }
        }

        public override void Draw()
        {
            DrawBackground();
            DrawMenu();
        }

        protected void DrawBackground()
        {
            Vector2 titleSize = Art.DefaultFont.MeasureString(_title);
            float padding = 20f;

            // Determine menu width based on whether description exists
            float menuWidth = (_description != null && _description.Count > 0) ? 800f : 300f;

            // Calculate the total description height for all lines
            float descriptionHeight = 0;
            if (_description != null && _description.Count > 0)
            {
                foreach (string line in _description)
                {
                    descriptionHeight += MeasureWrappedTextHeight(line, menuWidth); // Adjust wrapping width based on menu width
                }
                descriptionHeight += padding;
            }

            // Calculate total height of menu: title + menu items + description
            float menuHeight = titleSize.Y + (_menuItems.Count * (titleSize.Y + padding)) + padding * 2 + descriptionHeight;

            // Calculate the position to center the menu on the screen
            Vector2 menuPosition = new(
                (Resource.GraphicsDevice.Viewport.Width - menuWidth) / 2,
                (Resource.GraphicsDevice.Viewport.Height - menuHeight) / 2
            );

            // Draw semi-transparent background for the menu with padding
            Rectangle backgroundRectangle = new(
                (int)menuPosition.X - (int)padding,
                (int)menuPosition.Y - (int)padding,
                (int)menuWidth + (int)padding * 2,
                (int)menuHeight + (int)padding * 2
            );

            Resource.SpriteBatch.Draw(_backgroundTexture, backgroundRectangle, Color.White * 0.95f);
        }

        protected void DrawMenu()
        {
            SpriteFont titleFont = Art.DefaultFont;
            Vector2 titleSize = titleFont.MeasureString(_title);
            float padding = 20f;

            // Determine menu width based on whether description exists
            float menuWidth = (_description != null && _description.Count > 0) ? 800f : 300f;

            // Calculate the total description height for all lines
            float descriptionHeight = 0;
            if (_description != null && _description.Count > 0)
            {
                foreach (string line in _description)
                {
                    descriptionHeight += MeasureWrappedTextHeight(line, menuWidth); // Adjust wrapping width based on menu width
                }
                descriptionHeight += padding;
            }

            float menuHeight = titleSize.Y + (_menuItems.Count * (titleSize.Y + padding)) + padding * 2 + descriptionHeight;

            Vector2 menuPosition = new(
                (Resource.GraphicsDevice.Viewport.Width - menuWidth) / 2,
                (Resource.GraphicsDevice.Viewport.Height - menuHeight) / 2
            );

            // Draw title
            Vector2 titlePosition = new(
                (Resource.GraphicsDevice.Viewport.Width - titleSize.X) / 2,
                menuPosition.Y + padding
            );
            Resource.SpriteBatch.DrawString(titleFont, _title, titlePosition, Color.Black);

            // Draw menu options
            float startY = titlePosition.Y + titleSize.Y + padding;
            for (int i = 0; i < _menuItems.Count; i++)
            {
                string option = _menuItems[i]?.Name;
                Vector2 optionSize = Art.DefaultFont.MeasureString(option);
                Vector2 optionPosition = new(
                    (Resource.GraphicsDevice.Viewport.Width - optionSize.X) / 2,
                    startY + i * (optionSize.Y + padding)
                );

                // Draw background for selected option
                if (i == _selectedIndex)
                {
                    Resource.SpriteBatch.Draw(_backgroundTexture, new Rectangle((int)optionPosition.X - 10, (int)optionPosition.Y - 5, (int)optionSize.X + 20, (int)optionSize.Y + 10), Color.Orange);
                    Resource.SpriteBatch.DrawString(Art.DefaultFont, option, optionPosition, Color.White);
                }
                else
                {
                    Resource.SpriteBatch.DrawString(Art.DefaultFont, option, optionPosition, Color.Black);
                }
            }

            // Draw description text
            if (_description != null && _description.Count > 0)
            {
                Vector2 descriptionPosition = new(
                    (Resource.GraphicsDevice.Viewport.Width - menuWidth) / 2,
                    startY + _menuItems.Count * (titleSize.Y + padding) + padding
                );

                foreach (string line in _description)
                {
                    DrawWrappedText(line, descriptionPosition, menuWidth, Color.Black);
                    descriptionPosition.Y += MeasureWrappedTextHeight(line, menuWidth);
                }

                // Draw image after the description
                if (_image != null)
                {
                    Vector2 imagePosition = new(
                        (Resource.GraphicsDevice.Viewport.Width - _image.Width) / 2,
                        descriptionPosition.Y + padding
                    );
                    Resource.SpriteBatch.Draw(_image, imagePosition, Color.White);
                }
            }
        }




        private static float MeasureWrappedTextHeight(string text, float maxWidth)
        {
            string[] words = text.Split(' ');
            float lineHeight = Art.DefaultFont.MeasureString("A").Y;
            float currentLineWidth = 0f;
            float totalHeight = lineHeight;

            foreach (string word in words)
            {
                Vector2 wordSize = Art.DefaultFont.MeasureString(word + " ");
                if (currentLineWidth + wordSize.X > maxWidth)
                {
                    totalHeight += lineHeight;
                    currentLineWidth = wordSize.X;
                }
                else
                {
                    currentLineWidth += wordSize.X;
                }
            }

            return totalHeight;
        }

        private static void DrawWrappedText(string text, Vector2 position, float maxWidth, Color color)
        {
            string[] words = text.Split(' ');
            float lineHeight = Art.DefaultFont.MeasureString("A").Y;
            float currentLineWidth = 0f;
            Vector2 currentPosition = position;

            foreach (string word in words)
            {
                Vector2 wordSize = Art.DefaultFont.MeasureString(word + " ");
                if (currentLineWidth + wordSize.X > maxWidth)
                {
                    currentPosition.Y += lineHeight;
                    currentPosition.X = position.X;
                    currentLineWidth = wordSize.X;
                }
                else
                {
                    currentLineWidth += wordSize.X;
                }

                Resource.SpriteBatch.DrawString(Art.DefaultFont, word + " ", currentPosition, color);
                currentPosition.X += wordSize.X;
            }
        }

    }
}

