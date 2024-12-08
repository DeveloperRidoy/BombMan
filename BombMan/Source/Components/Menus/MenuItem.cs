using System;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace BombMan.Source.Components.Menus
{
    public class MenuItem(string name, Action action) : BaseComponent
    {
        public string Name { get; } = name;
        public Action Action { get; set; } = action;
        public Vector2 Position { get; private set; }
        public Vector2 Size { get; private set; }
        public bool IsSelected { get; set; }

        private readonly int _paddingX = 10;
        private readonly int _paddingY = 5;

        public override void LoadContent()
        {
        }

        public bool IsMouseClicked()
        {
            return Resource.InputManager.IsLeftMouseButtonClicked() && BoundingRectangle.Contains(Resource.InputManager.CurrentMousePosition);
        }

        public bool IsTapped()
        {
            foreach (var touch in TouchPanel.GetState())
            {
                if (touch.State == TouchLocationState.Pressed)
                {
                    return BoundingRectangle.Contains(touch.Position);
                }
            }
            return false;
        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            // Draw background for the active menu item
            if (IsSelected)
            {
                Rectangle backgroundRect = new(
                    (int)(Position.X - _paddingX),
                    (int)(Position.Y - _paddingY),
                    (int)(Size.X + _paddingX * 2),
                    (int)(Size.Y + _paddingY * 2)
                );

                // Draw orange background for active item
                Resource.SpriteBatch.Draw(Art.WhiteBackgroundTexture, backgroundRect, Color.Orange);
            }

            // Draw the menu item text
            Resource.SpriteBatch.DrawString(
                Art.DefaultFont,
                Name,
                Position,
                IsSelected ? Color.White : Color.Black // White text for active, black for inactive
            );
        }

        // Bounding Rectangle
        public Rectangle BoundingRectangle => new(
            (int)(Position.X - _paddingX),
            (int)(Position.Y - _paddingY),

            (int)(Size.X + _paddingX * 2),
            (int)(Size.Y + _paddingY * 2)
        );

        // Method to set the position and size
        public void SetPosition(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }
    }
}
