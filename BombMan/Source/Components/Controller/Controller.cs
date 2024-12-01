//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input.Touch;
//using System.Collections.Generic;

//namespace BombMan.Source.Components.Controller;

//public class GameController : BaseComponent
//{
//    private readonly List<Button> _buttons;

//    // Constructor
//    public GameController()
//    {
//        _buttons = new List<Button>
//        {
//            new Button(new Vector2(100, 700), "Up"),    // Up arrow
//            new Button(new Vector2(100, 800), "Down"),  // Down arrow
//            new Button(new Vector2(50, 750), "Left"),   // Left arrow
//            new Button(new Vector2(150, 750), "Right"), // Right arrow
//            new Button(new Vector2(300, 750), "Bomb")   // Bomb icon
//        };
//    }

//    public override void LoadContent()
//    {
//        foreach (var button in _buttons)
//        {
//            button.LoadContent();
//        }
//    }

//    public override void Update()
//    {
//        // Detect touch inputs
//        TouchCollection touchCollection = TouchPanel.GetState();
//        foreach (var touch in touchCollection)
//        {
//            if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved)
//            {
//                foreach (var button in _buttons)
//                {
//                    if (button.IsPressed(touch.Position))
//                    {
//                        button.TriggerAction();
//                    }
//                }
//            }
//        }
//    }

//    public override void Draw()
//    {
//        foreach (var button in _buttons)
//        {
//            button.Draw();
//        }
//    }

//    // Check if a specific button is pressed
//    public bool IsButtonPressed(string buttonName)
//    {
//        return _buttons.Exists(b => b.Name == buttonName && b.IsActive);
//    }
//}
