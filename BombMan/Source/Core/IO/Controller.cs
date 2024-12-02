using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using BombMan.Source.Components;

namespace BombMan.Source.Core.IO
{
    public class Controller : BaseComponent
    {
        private readonly ControllerButton _upButton;
        private readonly ControllerButton _downButton;
        private readonly ControllerButton _leftButton;
        private readonly ControllerButton _rightButton;
        private readonly ControllerButton _enterButton;
        private readonly ControllerButton _backButton;

        public Controller()
        {
            _upButton = new ControllerButton(new Vector2(100, 200), 50, 50, Art.ControllerDefault, Art.ControllerActive);
            _downButton = new ControllerButton(new Vector2(100, 300), 50, 50, Art.ControllerDefault, Art.ControllerActive);
            _leftButton = new ControllerButton(new Vector2(50, 250), 50, 50, Art.ControllerDefault, Art.ControllerActive);
            _rightButton = new ControllerButton(new Vector2(150, 250), 50, 50, Art.ControllerDefault, Art.ControllerActive);
            _enterButton = new ControllerButton(new Vector2(200, 300), 50, 50, Art.ControllerDefault, Art.ControllerActive);
            _backButton = new ControllerButton(new Vector2(10, 10), 50, 50, Art.ControllerDefault, Art.ControllerActive);
        }

        public override void LoadContent()
        {
            _upButton.LoadContent();
            _downButton.LoadContent();
            _leftButton.LoadContent();
            _rightButton.LoadContent();
            _enterButton.LoadContent();
            _backButton.LoadContent();
        }

        public override void Update()
        {
            _upButton.Update();
            _downButton.Update();
            _leftButton.Update();
            _rightButton.Update();
            _enterButton.Update();
            _backButton.Update();
        }

        public override void Draw()
        {
            _upButton.Draw();
            _downButton.Draw();
            _leftButton.Draw();
            _rightButton.Draw();
            _enterButton.Draw();
            _backButton.Draw();
        }

        public bool IsUpPressed() => _upButton.IsPressed;
        public bool IsDownPressed() => _downButton.IsPressed;
        public bool IsLeftPressed() => _leftButton.IsPressed;
        public bool IsRightPressed() => _rightButton.IsPressed;
        public bool IsEnterPressed() => _enterButton.IsPressed;
        public bool IsBackPressed() => _backButton.IsPressed;
    }
}
