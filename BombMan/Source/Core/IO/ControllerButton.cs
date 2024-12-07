using BombMan.Source.Components.GamePlay;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace BombMan.Source.Core.IO
{
    public class ControllerButton(Vector2 position, int width, int height, Texture2D defaultTexture, Texture2D activeTexture, Rectangle sourceRect, bool isSinglePressKey, params Keys[] keyboardKeys) : GameObject(position, width, height)
    {
        private readonly Texture2D _defaultTexture = defaultTexture;
        private readonly Texture2D _activeTexture = activeTexture;
        private readonly Rectangle _sourceRect = sourceRect;
        private readonly Keys[] _keyboardKeys = keyboardKeys; // Associated keyboard keys
        private readonly bool _isSinglePressKey = isSinglePressKey; // Indicates if this button should only accept a single click

        public bool IsPressed { get; private set; }

        public override void LoadContent()
        {
            // Set textures for the GameObject base class
            Texture = _defaultTexture;
        }

        public override void Update()
        {
            // Check if the button is pressed via touch, mouse, or keyboard input
            IsPressed = CheckIfPressed();

            // Update the texture to reflect active or default state
            Texture = IsPressed ? _activeTexture : _defaultTexture;
        }

        private bool CheckIfPressed()
        {
            return IsLeftMousePressed() || IsTapped() || IsKeyPressed();
        }

        private bool IsLeftMousePressed()
        {
            return Resource.InputManager.IsLeftMouseButtonPressed() && GetBoundingRectangle().Contains(Resource.InputManager.CurrentMousePosition);
        }

        private bool IsTapped()
        {
            foreach (var touch in TouchPanel.GetState())
            {
                if (touch.State == TouchLocationState.Pressed)
                {
                    return GetBoundingRectangle().Contains(touch.Position.ToPoint());
                }
            }
            return false;
        }

        private bool IsKeyPressed()
        {
            if (_keyboardKeys == null || _keyboardKeys.Length == 0)
                return false;

            foreach (var key in _keyboardKeys)
            {
                if (_isSinglePressKey)
                {
                    // Single press: only true on key down transition
                    if (Resource.InputManager.CurrentKeyState.IsKeyDown(key) && !Resource.InputManager.PreviousKeyState.IsKeyDown(key))
                    {
                        return true;
                    }
                }
                else
                {
                    // Continuous press
                    if (Resource.InputManager.CurrentKeyState.IsKeyDown(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override Rectangle GetSourceRectangle()
        {
            return _sourceRect;
        }
    }
}
