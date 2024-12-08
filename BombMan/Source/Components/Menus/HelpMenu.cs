using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace BombMan.Source.Components.Menus
{
    public class HelpMenu : BaseMenu
    {
        private Texture2D _image;
        private readonly float _maxImageHeightPercentage = 0.5f; // Image height should not exceed 20% of the screen height

        public HelpMenu() : base(
            "Help",
            true // Indicates this is a submenu
        )
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _image = Art.HelpImage;
            SetMenuItemAndImagePositions();
        }

        private void SetMenuItemAndImagePositions()
        {
            Vector2 titleSize = Art.DefaultFont.MeasureString(_title);
            float lineHeight = Art.DefaultFont.MeasureString("A").Y;
            float padding = 30f;

            // Calculate total height required
            float imageHeight = Math.Min(_image.Height, Resource.GraphicsDevice.Viewport.Height * _maxImageHeightPercentage);
            float imageWidth = _image.Width * (imageHeight / _image.Height); // Maintain aspect ratio
            float totalMenuHeight = titleSize.Y + (_menuItems.Count * (lineHeight + padding)) + imageHeight + (padding * 3);

            // Center the content vertically
            float startY = (Resource.GraphicsDevice.Viewport.Height - totalMenuHeight) / 2;

            // Position the title
            float currentY = startY;
            _titlePosition = new Vector2(
                (Resource.GraphicsDevice.Viewport.Width - titleSize.X) / 2,
                currentY
            );
            currentY += titleSize.Y + padding;

            // Position the menu items
            for (int i = 0; i < _menuItems.Count; i++)
            {
                MenuItem item = _menuItems[i];
                Vector2 size = Art.DefaultFont.MeasureString(item.Name);
                Vector2 position = new(
                    (Resource.GraphicsDevice.Viewport.Width - size.X) / 2,
                    currentY
                );
                item.SetPosition(position, size);
                currentY += lineHeight + padding;
            }

            // Position the image
            _imagePosition = new Vector2(
                (Resource.GraphicsDevice.Viewport.Width - imageWidth) / 2,
                currentY + padding
            );
            _imageSize = new Vector2(imageWidth, imageHeight);
        }

        private Vector2 _titlePosition;
        private Vector2 _imagePosition;
        private Vector2 _imageSize;

        public override void DrawBackground()
        {
            float verticalPadding = 20f; // Padding for top and bottom
            float horizontalPadding = 30f; // Padding for left and right

            // Calculate the width and height of the menu area
            float menuWidth = Math.Max(400f, _imageSize.X) + horizontalPadding * 2; // Add horizontal padding
            float menuHeight = _titlePosition.Y + _imageSize.Y + (_menuItems.Count * (Art.DefaultFont.MeasureString("A").Y + verticalPadding)) + (verticalPadding * 2);

            // Center the background
            Vector2 menuPosition = new(
                (Resource.GraphicsDevice.Viewport.Width - menuWidth) / 2,
                (Resource.GraphicsDevice.Viewport.Height - menuHeight) / 2
            );

            // Draw the background
            Rectangle backgroundRectangle = new(
                (int)(menuPosition.X),
                (int)(menuPosition.Y),
                (int)menuWidth,
                (int)menuHeight
            );

            Resource.SpriteBatch.Draw(_backgroundTexture, backgroundRectangle, Color.White * 0.9f);
        }


        public override void DrawMenu()
        {
            // Draw the title
            Resource.SpriteBatch.DrawString(Art.DefaultFont, _title, _titlePosition, Color.Black);

            // Draw menu items
            foreach (var item in _menuItems)
            {
                item.Draw();
            }

            // Draw the image
            if (_image != null)
            {
                Rectangle imageRectangle = new((int)_imagePosition.X, (int)_imagePosition.Y, (int)_imageSize.X, (int)_imageSize.Y);
                Resource.SpriteBatch.Draw(_image, imageRectangle, Color.White);
            }
        }
    }
}
