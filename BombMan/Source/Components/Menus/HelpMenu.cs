using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace BombMan.Source.Components.Menus
{
    public class HelpMenu : BaseMenu
    {
        public HelpMenu() : base(
                "Help",
                new List<string>(), // Explicitly specify the type to resolve ambiguity
                true  // Indicates this is a submenu
                )
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _image = Resource.ContentManager.Load<Texture2D>("Images/Help"); // Load Help.png
        }

        public override void Draw()
        {
            // Get font for drawing
            SpriteFont font = Resource.DefaultFont;

            // Measure the size of the title and BACK button text
            Vector2 titleSize = font.MeasureString("HELP");
            Vector2 buttonSize = font.MeasureString("BACK");

            // Calculate title and button positions
            Vector2 titlePosition = new(
                (Resource.GraphicsDevice.Viewport.Width - titleSize.X) / 2,
                50 // Fixed Y-coordinate for the title
            );
            Vector2 buttonPosition = new(
                (Resource.GraphicsDevice.Viewport.Width - buttonSize.X) / 2,
                titlePosition.Y + titleSize.Y + 20 // Space below the title for the BACK button
            );

            // Calculate the dynamic scaling for the image to fit within the window
            float windowWidth = Resource.GraphicsDevice.Viewport.Width;
            float windowHeight = Resource.GraphicsDevice.Viewport.Height;

            float imageScale = Math.Min(
                windowWidth / _image.Width, // Scale to fit the width
                (windowHeight - (buttonPosition.Y + buttonSize.Y + 50)) / _image.Height // Scale to fit the height below the BACK button
            );

            Vector2 scaledSize = new(_image.Width * imageScale, _image.Height * imageScale);

            // Calculate the image position to center it horizontally and place it below the BACK button
            Vector2 imagePosition = new(
                (windowWidth - scaledSize.X) / 2,
                buttonPosition.Y + buttonSize.Y + 50 // Space below the BACK button
            );

            // Calculate the total height for the semi-transparent background
            float totalHeight = (imagePosition.Y + scaledSize.Y) - titlePosition.Y + 40;

            // Draw the semi-transparent background
            Rectangle backgroundRectangle = new(
                (int)(windowWidth - scaledSize.X - 40) / 2, // Padding on both sides
                (int)titlePosition.Y - 20, // Padding above the title
                (int)(scaledSize.X + 40),  // Width includes padding
                (int)totalHeight           // Total height from the title to the image
            );
            Resource.SpriteBatch.Draw(_backgroundTexture, backgroundRectangle, Color.White * 0.8f);

            // Draw the title
            Resource.SpriteBatch.DrawString(font, "HELP", titlePosition, Color.Black);

            // Draw the orange BACK button background
            Rectangle buttonBackground = new(
                (int)buttonPosition.X - 20, // Add padding to the background
                (int)buttonPosition.Y - 5,
                (int)(buttonSize.X + 40),
                (int)(buttonSize.Y + 10)
            );
            Resource.SpriteBatch.Draw(_backgroundTexture, buttonBackground, Color.Orange);

            // Draw the BACK text in white, centered on the orange background
            Vector2 textPosition = new(
                buttonBackground.X + (buttonBackground.Width - buttonSize.X) / 2,
                buttonBackground.Y + (buttonBackground.Height - buttonSize.Y) / 2
            );
            Resource.SpriteBatch.DrawString(font, "BACK", textPosition, Color.White);

            // Draw the dynamically scaled Help image
            if (_image != null)
            {
                Resource.SpriteBatch.Draw(_image, imagePosition, null, Color.White, 0f, Vector2.Zero, imageScale, SpriteEffects.None, 0f);
            }
        }


        public override void Update()
        {
            base.Update();

            // Check for BACK button selection (user input)
            if (Resource.InputManager.IsEnterPressed())
            {
                InvokeBackRequestedEvent();
            }
        }
    }
}
