using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombMan.Source.Core.Shared
{
    public static class Art
    {
        public static SpriteFont DefaultFont { get; set; }
        public static Texture2D Bomb { get; set; }
        public static Texture2D BombExplosion { get; set; }
        public static Texture2D HealthPotion { get; set; }
        public static Texture2D MultiBombPotion { get; set; }
        public static Texture2D HeroImages { get; set; }
        public static Texture2D LvlOneEnemy { get; set; }
        public static Texture2D LvlTwoEnemy { get; set; }
        public static Texture2D LvlThreeEnemy { get; set; }
        public static Texture2D LvlOneBackground { get; set; }
        public static Texture2D LvlTwoBackground { get; set; }
        public static Texture2D LvlThreeBackground { get; set; }
        public static Texture2D AboutImage { get; set; }
        public static Texture2D HelpImage { get; set; }

        public static void LoadContent()
        {
            DefaultFont = Resource.ContentManager.Load<SpriteFont>("Fonts/DefaultFont");
            //Bomb = Resource.ContentManager.Load<Texture2D>("Images/Items/Bomb");
            //BombExplosion = Resource.ContentManager.Load<Texture2D>("Images/Items/BombExplosion");
            //HealthPotion = Resource.ContentManager.Load<Texture2D>("Images/Items/HealthPotion");
            //MultiBombPotion = Resource.ContentManager.Load<Texture2D>("Images/Items/MultiBombPotion");
            HeroImages = Resource.ContentManager.Load<Texture2D>("Images/Characters/Heroes");
            //LvlOneEnemy = Resource.ContentManager.Load<Texture2D>("Images/Characters/Enemies/LvlOneEnemy");
            //LvlTwoEnemy = Resource.ContentManager.Load<Texture2D>("Images/Characters/Enemies/LvlTwoEnemy");
            //LvlThreeEnemy = Resource.ContentManager.Load<Texture2D>("Images/Characters/Enemies/LvlThreeEnemy");
            //LvlOneBackground = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/LvlOneBackground");
            //LvlTwoBackground = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/LvlTwoBackground");
            //LvlThreeBackground = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/LvlThreeBackground");
            AboutImage = Resource.ContentManager.Load<Texture2D>("Images/About");
            HelpImage = Resource.ContentManager.Load<Texture2D>("Images/Help");
        }
    }
}
