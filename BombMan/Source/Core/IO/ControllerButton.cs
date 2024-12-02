using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Linq;
using BombMan.Source.Components.GamePlay;
using Microsoft.Xna.Framework.Input;

namespace BombMan.Source.Core.IO
{
    public class ControllerButton : GameObject
    {
        public bool IsPressed { get; private set; }

        private readonly Texture2D _defaultTexture;
        private readonly Texture2D _activeTexture;

        public ControllerButton(Vector2 position, int width, int height, Texture2D defaultTexture, Texture2D activeTexture)
            : base(position, width, height)
        {
            _defaultTexture = defaultTexture;
            _activeTexture = activeTexture;
            Texture = _defaultTexture;
        }

        public override void Update()
        {
            IsPressed = CheckInput();
            Texture = IsPressed ? _activeTexture : _defaultTexture;
        }

        private bool CheckInput()
        {
            // Touch input for Android
            if (TouchPanel.GetState().Any(touch =>
                touch.State == TouchLocationState.Pressed &&
                GetBoundingRectangle().Contains(touch.Position.ToPoint())))
            {
                return true;
            }

            // Check for mouse input (PC)
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed &&
                GetBoundingRectangle().Contains(mouseState.Position))
            {
                return true;
            }

            // Return false if not pressed
            return false;
        }
    }
}
