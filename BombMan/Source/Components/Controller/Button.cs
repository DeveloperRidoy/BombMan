//using BombMan.Source.Core.Shared;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using BombMan.Source.Components.GamePlay;

//namespace BombMan.Source.Components.Controller
//{
//    public enum ButtonType
//    {
//        Start,
//        Pause,
//        Resume,
//        Restart,
//        Exit,
//        HighScores,
//        About,
//        Help,
//        Back
//    }

//    public class Button: BaseComponent
//    {
//        public Vector2 Position { get; }
//        public string Name { get; }
//        public bool IsActive { get; private set; }
//        private Texture2D _texture;

//        public Button(Vector2 position, string name)
//        {
//            Position = position;
//            Name = name;
//            IsActive = false;
//        }

//        public override void LoadContent()
//        {
//            // Load button texture
//            _texture = Art.GetButtonTexture(Name); // Use a helper function to get button textures
//        }

//        public bool IsPressed(Vector2 touchPosition)
//        {
//            Rectangle bounds = new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
//            IsActive = bounds.Contains(touchPosition);
//            return IsActive;
//        }

//        public void TriggerAction()
//        {
//            // Set the button to active, logic handled in Controller
//        }

//        public override void Draw()
//        {
//            Color tint = IsActive ? Color.Red : Color.White; // Highlight when active
//            Resource.SpriteBatch.Draw(_texture, Position, tint);
//        }
//    }
//}
