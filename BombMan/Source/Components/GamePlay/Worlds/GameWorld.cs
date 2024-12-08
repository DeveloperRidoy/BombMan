using System;
using System.Collections.Generic;
using System.IO;
using BombMan.Source.Components.GamePlay.Characters.Enemies;
using BombMan.Source.Components.GamePlay.Characters.Heroes;
using BombMan.Source.Components.GamePlay.Items;
using BombMan.Source.Components.GamePlay.Objects;
using BombMan.Source.Core.IO;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay.Worlds
{
    public partial class GameWorld
    {
        public event Action<int, int, bool> OnGameOver;
        public event Action OnPauseMenuRequest;
        private int WorldWidth;
        private int WorldHeight;
        private int TileSize;
        private int CharacterWidth;
        private int CharacterHeight;
        private int HudHeight;
        private int StageBackgroundPadding;

        private static readonly string SaveFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BombMan",
            "SavedWorld.bombMan"
        );

        private static readonly string HighScoreFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BombMan",
            "HighScores.bombMan"
        );

        private const int MaxHighScores = 5;

        private readonly Floor[,] _floors;
        private readonly List<Block> _blocks;
        private readonly List<Enemy> _enemies;
        private readonly List<Bomb> _bombs;
        private Hero _hero;
        private readonly HUD _hud;

        private bool _shouldClearBombs = false;

        private int SafeZoneRadius; // Blocks and enemies won't spawn within 2 tiles of the hero

        private Controller _controller;

        public int Level { get; private set; } = 1;
        public int Score { get; private set; } = 0;
        public List<int> HighScores { get; private set; } = [];
        public bool IsNewHighScore { get; private set; } = false;

        private TimeSpan _enemySpawnTimer = TimeSpan.Zero;
        private readonly TimeSpan _enemySpawnInterval = TimeSpan.FromSeconds(10); // Spawn every 10 seconds
        private readonly Vector2[] _spawnCorners; // corner positions for spawning enemies

        private StageBackground _stageBackground;

        private readonly int _horizontalCenterOffset = 0;

        public GameWorld(bool loadGame)
        {
            InitializeVariables();

            _floors = new Floor[WorldHeight, WorldWidth];
            _blocks = [];
            _enemies = [];
            _bombs = [];

            HighScores = LoadHighScores() ?? [];

            // Center the game area horizontally
            int gameWidth = WorldWidth * TileSize + StageBackgroundPadding * 2;
            _horizontalCenterOffset = (Resource.ScreenWidth - gameWidth) / 2;

            _spawnCorners = GenerateSpawnCorners();

            if (loadGame && File.Exists(SaveFilePath))
            {
                LoadFromFile();
            }
            else
            {
                InitializeDefaultWorld();
            }
            _hud = new HUD(_hero, this);
            _hero.OnPlaceBomb += PlaceBombAtPosition;

            InitializeStageBackground();
            InitializeController();
            PlayBackgroundMusic();
        }

        public void LoadContent()
        {
            LoadAllContent();
        }

        public void Update()
        {
            // Update the controller
            _controller.Update();

            // Check if the back button is pressed
            if (_controller.IsBackPressed())
            {
                OnPauseMenuRequest?.Invoke();
            }

            Vector2 previousPosition = _hero.Position;
            _hero.Update(_controller);
            HandleHeroCollision(previousPosition);
            UpdateEnemies();
            EnsureCharactersStaysWithinBounds();
            UpdateBombs();
            ConditionallySpawnEnemies();
            ResetHeroBombReference();
            _hud.Update();
            _stageBackground.Update();
            HandleGameOver();
        }

        private void LoadAllContent()
        {

            foreach (var floor in _floors)
                floor.LoadContent();

            foreach (var block in _blocks)
                block.LoadContent();

            foreach (var enemy in _enemies)
                enemy.LoadContent();

            foreach (var bomb in _bombs)
                bomb.LoadContent();

            _hero.LoadContent();
            _hud.LoadContent();
            _stageBackground.LoadContent();
            _controller.LoadContent();
        }


        public void Draw()
        {
            _stageBackground.Draw();
            DrawFloors();
            DrawBlocks();
            DrawBombs();
            DrawEnemies();
            _hero.Draw();
            _hud.Draw();
            _controller.Draw();
        }
    }
}