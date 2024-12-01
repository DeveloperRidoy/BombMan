using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    public class AboutMenu : BaseMenu 
    {

        private Song CreditsSong { get; set; }
        public AboutMenu() : base(
            "About",
            new List<string>(), // Explicitly specify the type to resolve ambiguity
            true  // Indicates this is a submenu
            )
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _image = Art.AboutImage;
            CreditsSong = Art.CreditsSong;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(CreditsSong);
        }


        public override void Draw()
        {
            // Get font for drawing
            SpriteFont font = Art.DefaultFont;

            // Measure the size of the title and BACK button text
            Vector2 titleSize = font.MeasureString("ABOUT");
            Vector2 buttonSize = font.MeasureString("BACK");

            // Image scale
            float scale = 0.25f; // Adjust scale as needed
            Vector2 scaledSize = new(_image.Width * scale, _image.Height * scale);

            // Calculate positions
            Vector2 titlePosition = new(
                (Resource.GraphicsDevice.Viewport.Width - titleSize.X) / 2, // Center horizontally
                150 // Fixed Y-coordinate for the title (adjust as needed)
            );
            Vector2 buttonPosition = new(
                (Resource.GraphicsDevice.Viewport.Width - buttonSize.X) / 2, // Center horizontally
                titlePosition.Y + titleSize.Y + 20 // Space below the title for the BACK button
            );
            Vector2 imagePosition = new(
                (Resource.GraphicsDevice.Viewport.Width - scaledSize.X) / 2, // Center horizontally
                buttonPosition.Y + buttonSize.Y + 50 // Space below the BACK button for the image
            );

            // Calculate the total height for the semi-transparent background
            float totalHeight = (imagePosition.Y + scaledSize.Y) - titlePosition.Y + 40;

            // Draw the semi-transparent background that includes the title, BACK button, and image
            Rectangle backgroundRectangle = new(
                (int)((Resource.GraphicsDevice.Viewport.Width - scaledSize.X - 40) / 2), // Add padding on both sides
                (int)titlePosition.Y - 20, // Padding above the title
                (int)(scaledSize.X + 40),  // Width includes padding
                (int)(totalHeight)         // Total height from the title to the image
            );
            Resource.SpriteBatch.Draw(_backgroundTexture, backgroundRectangle, Color.White * 0.95f);

            // Draw the title
            Resource.SpriteBatch.DrawString(font, "ABOUT", titlePosition, Color.Black);

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

            // Draw the About image
            if (_image != null)
            {
                Resource.SpriteBatch.Draw(_image, imagePosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }
    }
}

