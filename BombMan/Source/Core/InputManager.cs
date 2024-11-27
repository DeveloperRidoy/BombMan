using Microsoft.Xna.Framework.Input;

namespace BombMan.Source.Core
{
    public class InputManager
    {
        private KeyboardState _currentKeyState;
        private KeyboardState _previousKeyState;

        public InputManager() { }

        public void Update()
        {
            _previousKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();
        }

        public bool IsMoveUp()
        {
            return _currentKeyState.IsKeyDown(Keys.W) || _currentKeyState.IsKeyDown(Keys.Up);
        }

        public bool IsMoveDown()
        {
            return _currentKeyState.IsKeyDown(Keys.S) || _currentKeyState.IsKeyDown(Keys.Down);
        }

        public bool IsMoveLeft()
        {
            return _currentKeyState.IsKeyDown(Keys.A) || _currentKeyState.IsKeyDown(Keys.Left);
        }

        public bool IsMoveRight()
        {
            return _currentKeyState.IsKeyDown(Keys.D) || _currentKeyState.IsKeyDown(Keys.Right);
        }

        public bool IsEnter()
        {
            return _currentKeyState.IsKeyDown(Keys.Enter);
        }

        public bool IsPause()
        {
            return _currentKeyState.IsKeyDown(Keys.P) && _previousKeyState.IsKeyUp(Keys.P);
        }

        public bool IsEscape()
        {
            return _currentKeyState.IsKeyDown(Keys.Escape);
        }

        public bool IsEscapePressed()
        {
            return (_currentKeyState.IsKeyDown(Keys.Escape) && _previousKeyState.IsKeyUp(Keys.Escape));
        }

        public bool IsMoveUpPressed()
        {
            return (_currentKeyState.IsKeyDown(Keys.W) && _previousKeyState.IsKeyUp(Keys.W)) || (_currentKeyState.IsKeyDown(Keys.Up) && _previousKeyState.IsKeyUp(Keys.Up));
        }

        public bool IsMoveDownPressed()
        {
            return (_currentKeyState.IsKeyDown(Keys.S) && _previousKeyState.IsKeyUp(Keys.S)) || (_currentKeyState.IsKeyDown(Keys.Down) && _previousKeyState.IsKeyUp(Keys.Down));
        }

        public bool IsMoveLeftPressed()
        {
            return (_currentKeyState.IsKeyDown(Keys.A) && _previousKeyState.IsKeyUp(Keys.A)) || (_currentKeyState.IsKeyDown(Keys.Left) && _previousKeyState.IsKeyUp(Keys.Left));
        }

        public bool IsMoveRightPressed()
        {
            return (_currentKeyState.IsKeyDown(Keys.D) && _previousKeyState.IsKeyUp(Keys.D)) || (_currentKeyState.IsKeyDown(Keys.Right) && _previousKeyState.IsKeyUp(Keys.Right));
        }

        public bool IsEnterPressed()
        {
            return _currentKeyState.IsKeyDown(Keys.Enter) && _previousKeyState.IsKeyUp(Keys.Enter);
        }
    }
}
