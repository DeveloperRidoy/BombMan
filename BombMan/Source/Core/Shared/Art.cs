using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
        public static Texture2D Explosion { get; set; }
        public static Texture2D HeroImages { get; set; }
        public static Texture2D EnemyLvl1 { get; set; }
        public static Texture2D EnemyLvl2 { get; set; }
        public static Texture2D EnemyLvl3 { get; set; }
        public static Texture2D BackgroundLvl1 { get; set; }
        public static Texture2D BackgroundLvl2 { get; set; }
        public static Texture2D BackgroundLvl3 { get; set; }
        public static Texture2D BlockLvl1 { get; set; }
        public static Texture2D BlockLvl2 { get; set; }
        public static Texture2D BlockLvl3 { get; set; }
        public static Texture2D Floor { get; set; }
        public static Texture2D AboutImage { get; set; }
        public static Texture2D HelpImage { get; set; }
        public static Texture2D HealthIcon { get; set; }

        public static SoundEffect ExplosionSound { get; set; }
        public static SoundEffect GameOverSound { get; set; }
        public static SoundEffect PauseSound { get; set; }
        public static SoundEffect HeroOuchSound { get; set; }
        public static Song CreditsSong { get; set; }
        public static Song Map1Bgm { get; set; }
        public static Song Map2Bgm { get; set; }
        public static Song HighScoresBgm { get; set; }


        public static void LoadContent()
        {
            DefaultFont = Resource.ContentManager.Load<SpriteFont>("Fonts/DefaultFont");
            Bomb = Resource.ContentManager.Load<Texture2D>("Images/Items/Bomb");
            Explosion = Resource.ContentManager.Load<Texture2D>("Images/Items/Explosion");
            HeroImages = Resource.ContentManager.Load<Texture2D>("Images/Characters/Heroes");
            EnemyLvl1 = Resource.ContentManager.Load<Texture2D>("Images/Characters/Enemies/EnemyLvl1");
            EnemyLvl2 = Resource.ContentManager.Load<Texture2D>("Images/Characters/Enemies/EnemyLvl2");
            EnemyLvl3 = Resource.ContentManager.Load<Texture2D>("Images/Characters/Enemies/EnemyLvl3");
            BackgroundLvl1 = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/BackgroundLvl1");
            BackgroundLvl2 = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/BackgroundLvl2");
            //BackgroundLvl3 = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/BackgroundLvl3");
            BlockLvl1 = Resource.ContentManager.Load<Texture2D>("Images/Blocks/Block1");
            BlockLvl2 = Resource.ContentManager.Load<Texture2D>("Images/Blocks/Block2");
            BlockLvl3 = Resource.ContentManager.Load<Texture2D>("Images/Blocks/Block3");
            Floor = Resource.ContentManager.Load<Texture2D>("Images/Floors/Floor");
            AboutImage = Resource.ContentManager.Load<Texture2D>("Images/About");
            HelpImage = Resource.ContentManager.Load<Texture2D>("Images/Help");
            HealthIcon = Resource.ContentManager.Load<Texture2D>("Images/HeartIcon");

            ExplosionSound = Resource.ContentManager.Load<SoundEffect>("Audio/Bombs/ExplosionSound");
            GameOverSound = Resource.ContentManager.Load<SoundEffect>("Audio/GameOverSound");
            PauseSound = Resource.ContentManager.Load<SoundEffect>("Audio/PauseSound");
            HeroOuchSound = Resource.ContentManager.Load<SoundEffect>("Audio/CharacterSounds/Ouch");
            CreditsSong = Resource.ContentManager.Load<Song>("Audio/Credits");
            Map1Bgm = Resource.ContentManager.Load<Song>("Audio/Map1");
            Map2Bgm = Resource.ContentManager.Load<Song>("Audio/Map2");
            HighScoresBgm = Resource.ContentManager.Load<Song>("Audio/HighScoresBgm");
        }
    }
}
