using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

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
        public static Texture2D GameBackground { get; set; }
        public static Texture2D BomberManPoster { get; set; }
        public static Texture2D StageBackgroundLvl1 { get; set; }
        public static Texture2D StageBackgroundLvl2 { get; set; }
        public static Texture2D StageBackgroundLvl3 { get; set; }
       
        public static Texture2D BlockLvl1 { get; set; }
        public static Texture2D BlockLvl2 { get; set; }
        public static Texture2D BlockLvl3 { get; set; }
        public static Texture2D Floor { get; set; }
        public static Texture2D AboutImage { get; set; }
        public static Texture2D HelpImage { get; set; }
        public static Texture2D HealthIcon { get; set; }
        public static Texture2D WhiteBackgroundTexture { get; set; }

        public static SoundEffect ExplosionSound { get; set; }
        public static SoundEffect GameOverSound { get; set; }
        public static SoundEffect PauseSound { get; set; }
        public static SoundEffect HeroOuchSound { get; set; }
        public static Song CreditsSong { get; set; }
        public static Song Map1Bgm { get; set; }
        public static Song Map2Bgm { get; set; }
        public static Song HighScoresBgm { get; set; }

        // for controller 
        public static Texture2D ControllerDefault { get; set; }
        public static Texture2D ControllerActive { get; set; }
        public static Texture2D BackBtnDefault { get; set; }
        public static Texture2D BackBtnActive { get; set; }

        public static void LoadContent()
        {
            DefaultFont = Resource.ContentManager.Load<SpriteFont>("Fonts/DefaultFont");
            Bomb = Resource.ContentManager.Load<Texture2D>("Images/Items/Bomb");
            Explosion = Resource.ContentManager.Load<Texture2D>("Images/Items/Explosion");
            HeroImages = Resource.ContentManager.Load<Texture2D>("Images/Characters/Heroes");
            GameBackground = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/Background");
            BomberManPoster = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/BomberManPoster");
            EnemyLvl1 = Resource.ContentManager.Load<Texture2D>("Images/Characters/Enemies/EnemyLvl1");
            EnemyLvl2 = Resource.ContentManager.Load<Texture2D>("Images/Characters/Enemies/EnemyLvl2");
            EnemyLvl3 = Resource.ContentManager.Load<Texture2D>("Images/Characters/Enemies/EnemyLvl3");
            StageBackgroundLvl1 = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/StageBackgroundLvl1");
            StageBackgroundLvl2 = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/StageBackgroundLvl2");
            StageBackgroundLvl3 = Resource.ContentManager.Load<Texture2D>("Images/Backgrounds/StageBackgroundLvl3");
            BlockLvl1 = Resource.ContentManager.Load<Texture2D>("Images/Blocks/Block1");
            BlockLvl2 = Resource.ContentManager.Load<Texture2D>("Images/Blocks/Block2");
            BlockLvl3 = Resource.ContentManager.Load<Texture2D>("Images/Blocks/Block3");
            Floor = Resource.ContentManager.Load<Texture2D>("Images/Floors/Floor");
            AboutImage = Resource.ContentManager.Load<Texture2D>("Images/About");
            HelpImage = Resource.ContentManager.Load<Texture2D>("Images/Help");
            HealthIcon = Resource.ContentManager.Load<Texture2D>("Images/HeartIcon");
            WhiteBackgroundTexture = new Texture2D(Resource.GraphicsDevice, 1, 1);
            WhiteBackgroundTexture.SetData([Color.White]);

            ExplosionSound = Resource.ContentManager.Load<SoundEffect>("Audio/Bombs/ExplosionSound");
            GameOverSound = Resource.ContentManager.Load<SoundEffect>("Audio/GameOverSound");
            PauseSound = Resource.ContentManager.Load<SoundEffect>("Audio/PauseSound");
            HeroOuchSound = Resource.ContentManager.Load<SoundEffect>("Audio/CharacterSounds/Ouch");
            CreditsSong = Resource.ContentManager.Load<Song>("Audio/Credits");
            Map1Bgm = Resource.ContentManager.Load<Song>("Audio/Map1");
            Map2Bgm = Resource.ContentManager.Load<Song>("Audio/Map2");
            HighScoresBgm = Resource.ContentManager.Load<Song>("Audio/HighScoresBgm");

            // for controller
            ControllerDefault = Resource.ContentManager.Load<Texture2D>("Images/Controller/ControllerImg_Default");
            ControllerActive = Resource.ContentManager.Load<Texture2D>("Images/Controller/ControllerImg_Active");
            BackBtnDefault = Resource.ContentManager.Load<Texture2D>("Images/Controller/BackBtnDefault");
            BackBtnActive = Resource.ContentManager.Load<Texture2D>("Images/Controller/BackBtnActive");
        }
    }
}
