using BombMan.Source.Components;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace BombMan.Source.Core.IO
{
    public class Controller : BaseComponent
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }

        public readonly ControllerButton _upButton;
        public readonly ControllerButton _downButton;
        public readonly ControllerButton _leftButton;
        public readonly ControllerButton _rightButton;
        public readonly ControllerButton _enterButton;
        public readonly ControllerButton _backButton;

        private readonly static int buttonSize = 30;  // Standard button size
        private readonly static int spacing = 5;    // Spacing between buttons
        private readonly static int sideOffset = 60; // Horizontal/vertical offset for action buttons

        public Controller(Vector2 basePosition)
        {

            Width = GetWidth();
            Height = GetHeight();

            // Define source rectangles for button images
            Rectangle upSource = new(176, 0, 158, 158);
            Rectangle downSource = new(176, 394, 158, 158);
            Rectangle leftSource = new(0, 176, 158, 158);
            Rectangle rightSource = new(394, 178, 158, 158);
            Rectangle enterSource = new(156, 156, 200, 200);
            Rectangle backSource = new(0, 0, Art.BackBtnActive.Width, Art.BackBtnActive.Height);

            // Positioning buttons
            _upButton = new ControllerButton(
                basePosition + new Vector2(buttonSize + spacing, spacing),
                buttonSize, buttonSize, Art.ControllerDefault, Art.ControllerActive, upSource, false, Keys.W, Keys.Up
            );

            _downButton = new ControllerButton(
                basePosition + new Vector2(buttonSize + spacing, (buttonSize + spacing) * 2),
                buttonSize, buttonSize, Art.ControllerDefault, Art.ControllerActive, downSource, false, Keys.S, Keys.Down
            );

            _leftButton = new ControllerButton(
                basePosition + new Vector2(spacing, buttonSize + spacing),
                buttonSize, buttonSize, Art.ControllerDefault, Art.ControllerActive, leftSource, false, Keys.A, Keys.Left
            );

            _rightButton = new ControllerButton(
                basePosition + new Vector2((buttonSize + spacing) * 2, buttonSize + spacing),
                buttonSize, buttonSize, Art.ControllerDefault, Art.ControllerActive, rightSource, false, Keys.D, Keys.Right
            );

            // Place Enter button to the right of the D-Pads
            _enterButton = new ControllerButton(
                basePosition + new Vector2((buttonSize + spacing) * 3, buttonSize),
                buttonSize + 10, buttonSize + 10, Art.ControllerDefault, Art.ControllerActive, enterSource, true, Keys.Enter, Keys.Space
            );

            // Place Back button below and slightly left of the Enter button
            _backButton = new ControllerButton(
                basePosition + new Vector2((buttonSize + spacing) * 3 - sideOffset / 3, buttonSize + spacing + sideOffset / 2),
                buttonSize, buttonSize, Art.BackBtnDefault, Art.BackBtnActive, backSource, true, Keys.Escape
            );
        }

        public static int GetWidth() => (buttonSize + spacing) * 3 + (buttonSize + 10);
        public static int GetHeight() => (buttonSize + spacing) * 3 + sideOffset / 2;

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
