using Microsoft.Xna.Framework.Input;
using System.Numerics;

namespace BombMan.Source.Core.IO
{
    public class InputManager
    {
        public KeyboardState CurrentKeyState;
        public KeyboardState PreviousKeyState;

        public MouseState CurrentMouseState;
        public MouseState PreviousMouseState;

        public InputManager() { }

        public void Update()
        {
            PreviousKeyState = CurrentKeyState;
            CurrentKeyState = Keyboard.GetState();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        public Vector2 PreviousMousePosition => new(PreviousMouseState.X, PreviousMouseState.Y);
        public Vector2 CurrentMousePosition => new(CurrentMouseState.X, CurrentMouseState.Y);

        public bool IsLeftMouseButtonClicked ()
        {
            return CurrentMouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
        }

        public bool IsLeftMouseButtonPressed()
        {
            return CurrentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsMoveUp()
        {
            return CurrentKeyState.IsKeyDown(Keys.W) || CurrentKeyState.IsKeyDown(Keys.Up);
        }

        public bool IsMoveDown()
        {
            return CurrentKeyState.IsKeyDown(Keys.S) || CurrentKeyState.IsKeyDown(Keys.Down);
        }

        public bool IsMoveLeft()
        {
            return CurrentKeyState.IsKeyDown(Keys.A) || CurrentKeyState.IsKeyDown(Keys.Left);
        }

        public bool IsMoveRight()
        {
            return CurrentKeyState.IsKeyDown(Keys.D) || CurrentKeyState.IsKeyDown(Keys.Right);
        }

        public bool IsEnter()
        {
            return CurrentKeyState.IsKeyDown(Keys.Enter);
        }

        public bool IsPause()
        {
            return CurrentKeyState.IsKeyDown(Keys.P) && PreviousKeyState.IsKeyUp(Keys.P);
        }

        public bool IsEscape()
        {
            return CurrentKeyState.IsKeyDown(Keys.Escape);
        }

        public bool IsSpace()
        {
            return CurrentKeyState.IsKeyDown(Keys.Space);
        }

        public bool IsEscapePressed()
        {
            return CurrentKeyState.IsKeyDown(Keys.Escape) && PreviousKeyState.IsKeyUp(Keys.Escape);
        }

        public bool IsMoveUpPressed()
        {
            return CurrentKeyState.IsKeyDown(Keys.W) && PreviousKeyState.IsKeyUp(Keys.W) || CurrentKeyState.IsKeyDown(Keys.Up) && PreviousKeyState.IsKeyUp(Keys.Up);
        }

        public bool IsMoveDownPressed()
        {
            return CurrentKeyState.IsKeyDown(Keys.S) && PreviousKeyState.IsKeyUp(Keys.S) || CurrentKeyState.IsKeyDown(Keys.Down) && PreviousKeyState.IsKeyUp(Keys.Down);
        }

        public bool IsMoveLeftPressed()
        {
            return CurrentKeyState.IsKeyDown(Keys.A) && PreviousKeyState.IsKeyUp(Keys.A) || CurrentKeyState.IsKeyDown(Keys.Left) && PreviousKeyState.IsKeyUp(Keys.Left);
        }

        public bool IsMoveRightPressed()
        {
            return CurrentKeyState.IsKeyDown(Keys.D) && PreviousKeyState.IsKeyUp(Keys.D) || CurrentKeyState.IsKeyDown(Keys.Right) && PreviousKeyState.IsKeyUp(Keys.Right);
        }

        public bool IsEnterPressed()
        {
            return CurrentKeyState.IsKeyDown(Keys.Enter) && PreviousKeyState.IsKeyUp(Keys.Enter);
        }

        public bool IsSpacePressed()
        {
            return CurrentKeyState.IsKeyDown(Keys.Space) && PreviousKeyState.IsKeyUp(Keys.Space);
        }
    }
}
