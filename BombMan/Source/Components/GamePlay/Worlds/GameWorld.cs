using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BombMan.Source.Components.GamePlay.Characters.Enemies;
using BombMan.Source.Components.GamePlay.Characters.Heroes;
using BombMan.Source.Components.GamePlay.Items;
using BombMan.Source.Components.GamePlay.Objects;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace BombMan.Source.Components.GamePlay.Worlds
{
    public partial class GameWorld
    {
        public event Action<int, int, bool> OnGameOver;
        private const int WorldWidth = 8;
        private const int WorldHeight = 8;
        private const int TileSize = 64;
        private const int CharacterWidth = 30;
        private const int CharacterHeight = 40;
        private const int HudHeight = 150;
        private const int StageBackgroundPadding = 85;

        private const string SaveFilePath = "GameWorldSave.bombMan";

        private readonly Floor[,] _floors;
        private readonly List<Block> _blocks;
        private readonly List<Enemy> _enemies;
        private readonly List<Bomb> _bombs;
        private Hero _hero;
        private readonly HUD _hud;

        private bool _shouldClearBombs = false;

        private const int SafeZoneRadius = TileSize * 2; // Blocks and enemies won't spawn within 2 tiles of the hero

        public int Level { get; private set; } = 1;
        public int Score { get; private set; } = 0;
        public List<int> HighScores { get; private set; } = new List<int>();
        public bool IsNewHighScore { get; private set; } = false;

        private TimeSpan _enemySpawnTimer = TimeSpan.Zero;
        private readonly TimeSpan _enemySpawnInterval = TimeSpan.FromSeconds(10); // Spawn every 10 seconds
        private readonly Vector2[] _corners; // Stores corner positions for spawning

        private StageBackground _stageBackground;

        private readonly int _horizontalCenterOffset = 0;

        public GameWorld(bool loadGame)
        {
            _floors = new Floor[WorldHeight, WorldWidth];
            _blocks = new List<Block>();
            _enemies = new List<Enemy>();
            _bombs = new List<Bomb>();

            HighScores = GameWorldHelper.LoadHighScores() ?? new List<int>();

            // Center the game area horizontally
            int gameWidth = WorldWidth * TileSize + StageBackgroundPadding * 2;
            _horizontalCenterOffset = (Resource.ScreenWidth - gameWidth) / 2;

            _corners = new[]
            {
        new Vector2(StageBackgroundPadding + _horizontalCenterOffset, HudHeight + StageBackgroundPadding),
        new Vector2(WorldWidth * TileSize - StageBackgroundPadding + _horizontalCenterOffset, HudHeight + StageBackgroundPadding),
        new Vector2(StageBackgroundPadding + _horizontalCenterOffset, (WorldHeight - 1) * TileSize + HudHeight - StageBackgroundPadding),
        new Vector2(WorldWidth * TileSize - StageBackgroundPadding + _horizontalCenterOffset, (WorldHeight - 1) * TileSize + HudHeight - StageBackgroundPadding)
    };

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
            PlayBackgroundMusic();
        }



        // Method to initialize the correct stage background based on the level
        private void InitializeStageBackground()
        {
            var levelType = Level switch
            {
                1 => ELvl.Lvl1,
                _ => ELvl.Lvl2,
            };

            // Adjust StageBackground to be horizontally centered
            _stageBackground = new StageBackground(
                new Vector2(0 + _horizontalCenterOffset, HudHeight), // Apply horizontal offset
                WorldWidth * TileSize + StageBackgroundPadding * 2,
                WorldHeight * TileSize + StageBackgroundPadding * 2,
                levelType
            );
        }

        private void InitializeDefaultWorld()
        {
            PlaceFloors();
            PlaceHero();
            PlaceEnemies();
            PlaceBlocks();
        }

        private void PlaceFloors()
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2 position = new(
                        x * TileSize + StageBackgroundPadding + _horizontalCenterOffset,
                        y * TileSize + HudHeight + StageBackgroundPadding
                    );

                    _floors[y, x] = new Floor(position, TileSize, TileSize);
                }
            }
        }

        private void PlaceBlocks()
        {
            Random random = new();
            for (int i = 0; i < 15; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(WorldWidth);
                    y = random.Next(WorldHeight);

                    Vector2 blockPosition = new(
                        x * TileSize + StageBackgroundPadding + _horizontalCenterOffset,
                        y * TileSize + HudHeight + StageBackgroundPadding
                    );

                    Vector2 heroStartPosition = new(
                        WorldWidth / 2 * TileSize + StageBackgroundPadding + _horizontalCenterOffset,
                        WorldHeight / 2 * TileSize + HudHeight + StageBackgroundPadding
                    );

                    if (Vector2.Distance(blockPosition, heroStartPosition) < SafeZoneRadius ||
                        _blocks.Exists(b => b.Position == blockPosition))
                    {
                        continue;
                    }

                    _blocks.Add(new Block(blockPosition, TileSize, TileSize, Level == 1 ? ELvl.Lvl1 : ELvl.Lvl2));
                    break;

                } while (true);
            }
        }


        private void PlaceHero()
        {
            _hero = new Hero(
                new Vector2(
                    WorldWidth / 2 * TileSize + StageBackgroundPadding + _horizontalCenterOffset,
                    WorldHeight / 2 * TileSize + HudHeight + StageBackgroundPadding
                ),
                CharacterWidth,
                CharacterHeight,
                1f,
                5
            );
        }

        private void PlaceEnemies()
        {
            Random random = new();
            _enemies.Clear();

            int numEnemies = Level == 1 ? 5 : 5 + (Level - 1) * 2;

            for (int i = 0; i < numEnemies; i++)
            {
                int x, y;
                int attempts = 0;
                const int maxAttempts = 100; // Prevent infinite loops by limiting attempts

                while (attempts < maxAttempts)
                {
                    x = random.Next(WorldWidth);
                    y = random.Next(WorldHeight);

                    Vector2 enemyPosition = new(
                        x * TileSize + StageBackgroundPadding + _horizontalCenterOffset,
                        y * TileSize + HudHeight + StageBackgroundPadding
                    );

                    Rectangle enemyBounds = new ((int)enemyPosition.X, (int)enemyPosition.Y, CharacterWidth, CharacterHeight);

                    // Ensure no overlap with hero, blocks, or other enemies
                    bool isPositionValid = Vector2.Distance(enemyPosition, _hero.Position) >= SafeZoneRadius &&
                                           !_blocks.Exists(b => b.GetBoundingRectangle().Intersects(enemyBounds)) &&
                                           !_enemies.Exists(e => e.GetBoundingRectangle().Intersects(enemyBounds));

                    if (isPositionValid)
                    {
                        _enemies.Add(new EnemyLvl1(enemyPosition, CharacterWidth, CharacterHeight, 1, HudHeight, TileSize));
                        break;
                    }

                    attempts++;
                }

                if (attempts >= maxAttempts)
                {
                    // Log or handle the case where an enemy could not be placed
                    Console.WriteLine($"Failed to place enemy {i + 1} after {maxAttempts} attempts.");
                }
            }
        }




        private void PlaceBombAtPosition(Vector2 position, Hero hero)
        {
            // Align the bomb position to the grid
            Vector2 bombPosition = new(
                (float)Math.Floor(position.X / TileSize) * TileSize,
                (float)Math.Floor((position.Y - HudHeight) / TileSize) * TileSize + HudHeight
            );

            // Prevent placing multiple bombs at the same location
            if (!_bombs.Exists(b => b.Position == bombPosition))
            {
                Bomb bomb = new(bombPosition, TileSize, TileSize);
                bomb.LoadContent();
                _bombs.Add(bomb);

                // Associate the bomb with the hero
                hero.LastPlacedBomb = bomb;
            }
        }

        public void LoadContent()
        {
            LoadAllContent();
        }

        public void Update()
        {
            Vector2 previousPosition = _hero.Position;
            _hero.Update();
            CheckHeroCollisionWithBlocks(previousPosition);
            EnsureCharactersStaysWithinBounds();
            UpdateEnemies();
            UpdateBombs();
            ConditionallySpawnEnemies();
            ResetHeroBombReference();
            _hud.Update();
            _stageBackground.Update();
            HandleGameOver();
        }



        private void ResetHeroBombReference()
        {
            if (_hero.LastPlacedBomb != null && !_hero.GetBoundingRectangle().Intersects(_hero.LastPlacedBomb.GetBoundingRectangle()))
            {
                _hero.LastPlacedBomb = null;
            }
        }

        private void ConditionallySpawnEnemies()
        {
            if (Level > 1)
            {
                _enemySpawnTimer += Resource.UpdateGameTime.ElapsedGameTime;
                if (_enemySpawnTimer >= _enemySpawnInterval)
                {
                    SpawnEnemiesFromCorners();
                    _enemySpawnTimer = TimeSpan.Zero;
                }
            }
        }

        private void SpawnEnemiesFromCorners()
        {
            Random random = new();
            foreach (var corner in _corners)
            {
                // Ensure the corner is free to spawn
                if (!_blocks.Exists(b => b.GetBoundingRectangle().Contains(corner)) &&
                    !_enemies.Exists(e => e.GetBoundingRectangle().Contains(corner)))
                {
                    Enemy newEnemy;

                    // Alternate between Level 1 and Level 2 enemies
                    if (random.Next(2) == 0)
                    {
                        newEnemy = new EnemyLvl1(corner, CharacterWidth, CharacterHeight, 1, HudHeight, TileSize);
                    }
                    else
                    {
                        newEnemy = new EnemyLvl2(corner, CharacterWidth, CharacterHeight, 0.5f, HudHeight, TileSize, _hero);
                    }

                    // Verify the enemy does not intersect with any blocks
                    if (!_blocks.Exists(b => b.GetBoundingRectangle().Intersects(new Rectangle(
                        (int)corner.X, (int)corner.Y, CharacterWidth, CharacterHeight))))
                    {
                        newEnemy.LoadContent();
                        _enemies.Add(newEnemy);
                    }
                }
            }
        }



        private void HandleGameOver()
        {
            if (_hero.Health <= 0)
            {
                // Stop background music
                MediaPlayer.Stop();

                UpdateHighScores();
                OnGameOver?.Invoke(Score, HighScores.Max(), IsNewHighScore);
            }
        }

        private void CheckHeroCollisionWithBlocks(Vector2 previousPosition)
        {
            foreach (var block in _blocks)
            {
                if (_hero.GetBoundingRectangle().Intersects(block.GetBoundingRectangle()))
                {
                    // Revert hero's position if colliding with a block
                    _hero.Position = previousPosition;
                    _hero.Stop();
                    break;
                }
            }

            // Prevent hero from walking over bombs
            foreach (var bomb in _bombs)
            {
                if (!_bombsToRemove.Contains(bomb))
                {
                    // Allow hero to pass through their own bomb until they move away
                    if (bomb == _hero.LastPlacedBomb)
                    {
                        // Hero can pass through this bomb
                        continue;
                    }
                    else if (_hero.GetBoundingRectangle().Intersects(bomb.GetBoundingRectangle()))
                    {
                        // Revert hero's position if colliding with other bombs
                        _hero.Position = previousPosition;
                        _hero.Stop();
                        break;
                    }
                }
            }
        }

        private void EnsureCharactersStaysWithinBounds()
        {
            var bounds = _stageBackground.GetBoundingRectangle();
            _hero.Position = new Vector2(
                Math.Clamp(_hero.Position.X, bounds.Left + StageBackgroundPadding, bounds.Right - StageBackgroundPadding - CharacterWidth),
                Math.Clamp(_hero.Position.Y, bounds.Top + StageBackgroundPadding, bounds.Bottom - StageBackgroundPadding - CharacterHeight)
            );

            foreach (var enemy in _enemies)
            {
                enemy.Position = new Vector2(
                     Math.Clamp(enemy.Position.X, bounds.Left + StageBackgroundPadding, bounds.Right - StageBackgroundPadding - CharacterWidth),
                Math.Clamp(enemy.Position.Y, bounds.Top + StageBackgroundPadding, bounds.Bottom - StageBackgroundPadding - CharacterHeight)
                    );
            }
        }

        private void UpdateEnemies()
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _enemies[i];
                enemy.Update();
                enemy.HandleCollisionWithHero(_hero);
                enemy.HandleCollisionWithBlocks(_blocks);
                enemy.HandleCollisionWithBombs(_bombs);

                // Check if enemy is dead or removed (implement enemy health if needed)
            }
        }

        private readonly List<Bomb> _bombsToRemove = new();

        private void UpdateBombs()
        {
            if (_shouldClearBombs)
            {
                _bombs.Clear();
                _shouldClearBombs = false;
            }

            var bombsToRemove = new List<Bomb>();

            foreach (var bomb in _bombs)
            {
                bomb.Update();

                if (bomb.HasExploded && !bomb.ExplosionHandled)
                {
                    HandleBombExplosion(bomb);
                    bomb.ExplosionHandled = true;
                }

                if (!bomb.IsActive)
                {
                    bombsToRemove.Add(bomb);
                }
            }

            foreach (var bomb in bombsToRemove)
            {
                _bombs.Remove(bomb);
            }
        }

        private void HandleBombExplosion(Bomb bomb)
        {
            Rectangle explosionArea = bomb.GetBoundingRectangle();

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                if (explosionArea.Intersects(_enemies[i].GetBoundingRectangle()))
                {
                    _enemies.RemoveAt(i);
                    Score += 100; // Increment score for killing an enemy
                }
            }

            for (int i = _blocks.Count - 1; i >= 0; i--)
            {
                if (explosionArea.Intersects(_blocks[i].GetBoundingRectangle()))
                {
                    _blocks.RemoveAt(i);
                }
            }

            if (explosionArea.Intersects(_hero.GetBoundingRectangle()))
            {
                _hero.TakeDamage();
            }

            // Check for level progression
            if (_enemies.Count == 0)
            {
                if (Level <= 1) // Only progress to the next level if not in endless mode
                {
                    Level++;
                    InitializeNextLevel(); // Initialize the next level
                }
            }

            // Update high scores whenever the score changes
            UpdateHighScores();
        }



        private void InitializeNextLevel()
        {
            // updaste stage background
            InitializeStageBackground();

            // Clear existing walls, bombs, and enemies
            _blocks.Clear();
            _shouldClearBombs = true; // Set the flag to clear bombs later
            _enemies.Clear();

            // Replenish hero's life to 5
            _hero.Health = 5;

            // Reinitialize blocks for the new level
            PlaceBlocks();

            // Reinitialize enemies based on the current level
            PlaceEnemies();

            // Load contents
            LoadAllContent();

            // Play appropriate background music
            PlayBackgroundMusic();
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
        }


        public void Draw()
        {
            // Draw background first
            _stageBackground.Draw();
            DrawFloors();
            DrawBlocks();
            DrawBombs();
            DrawEnemies();
            _hero.Draw();
            _hud.Draw();
        }

        private void DrawFloors()
        {
            foreach (var floor in _floors)
            {
                floor.Draw();
            }
        }

        private void DrawBlocks()
        {
            foreach (var block in _blocks)
            {
                block.Draw();
            }
        }

        private void DrawEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Draw();
            }
        }

        private void DrawBombs()
        {
            foreach (var bomb in _bombs)
            {
                bomb.Draw();
            }
        }

        private void LoadFromFile()
        {
            string[] lines = File.ReadAllLines(SaveFilePath);
            int currentLine = 0;

            // Initialize floors (always static in size and layout)
            PlaceFloors();

            // Load level, score, and high score
            Level = int.Parse(lines[currentLine++]);
            Score = int.Parse(lines[currentLine++]);

            // Load blocks
            int blockCount = int.Parse(lines[currentLine++]);
            _blocks.Clear();
            for (int i = 0; i < blockCount; i++)
            {
                string[] parts = lines[currentLine++].Split(',');
                float x = float.Parse(parts[0]);
                float y = float.Parse(parts[1]);
                ELvl level = Enum.Parse<ELvl>(parts[2]);
                _blocks.Add(new Block(new Vector2(x, y + HudHeight), TileSize, TileSize, level));
            }

            // Load hero
            string[] heroParts = lines[currentLine++].Split(',');
            float heroX = float.Parse(heroParts[0]);
            float heroY = float.Parse(heroParts[1]);
            _hero = new Hero(new Vector2(heroX, heroY + HudHeight), CharacterWidth, CharacterHeight, 1f, 5);
            _hero.OnPlaceBomb += PlaceBombAtPosition;

            // Load enemies
            int enemyCount = int.Parse(lines[currentLine++]);
            _enemies.Clear();
            for (int i = 0; i < enemyCount; i++)
            {
                string[] enemyParts = lines[currentLine++].Split(',');
                float enemyX = float.Parse(enemyParts[0]);
                float enemyY = float.Parse(enemyParts[1]);
                int enemyType = int.Parse(enemyParts[2]);
                Enemy enemy;
                if (enemyType == 1)
                {
                    enemy = new EnemyLvl1(new Vector2(enemyX, enemyY + HudHeight), CharacterWidth, CharacterHeight, 1, HudHeight, TileSize);
                }
                else
                {
                    enemy = new EnemyLvl2(new Vector2(enemyX, enemyY + HudHeight), CharacterWidth, CharacterHeight, 0.5f, HudHeight, TileSize, _hero);
                }
                _enemies.Add(enemy);
            }

            // Load bombs
            int bombCount = int.Parse(lines[currentLine++]);
            _bombs.Clear();
            for (int i = 0; i < bombCount; i++)
            {
                string[] bombParts = lines[currentLine++].Split(',');
                float bombX = float.Parse(bombParts[0]);
                float bombY = float.Parse(bombParts[1]);
                Bomb bomb = new(new Vector2(bombX, bombY + HudHeight), TileSize, TileSize);
                bomb.LoadContent();
                _bombs.Add(bomb);
            }
        }

        public void SaveToFile()
        {
            using StreamWriter writer = new(SaveFilePath);

            // Save level, score, and high score
            writer.WriteLine(Level);
            writer.WriteLine(Score);

            // Save blocks
            writer.WriteLine(_blocks.Count);
            foreach (var block in _blocks)
            {
                writer.WriteLine($"{block.Position.X},{block.Position.Y - HudHeight},{block.Lvl}");
            }

            // Save hero
            writer.WriteLine($"{_hero.Position.X},{_hero.Position.Y - HudHeight}");

            // Save enemies
            writer.WriteLine(_enemies.Count);
            foreach (var enemy in _enemies)
            {
                int enemyType = enemy is EnemyLvl1 ? 1 : 2;
                writer.WriteLine($"{enemy.Position.X},{enemy.Position.Y - HudHeight},{enemyType}");
            }

            // Save bombs
            writer.WriteLine(_bombs.Count);
            foreach (var bomb in _bombs)
            {
                writer.WriteLine($"{bomb.Position.X},{bomb.Position.Y - HudHeight}");
            }
        }

        private void UpdateHighScores()
        {
            // Add current score to high scores if it qualifies
            if (Score > 0 && (HighScores.Count < 5 || Score > HighScores.Min()))
            {
                HighScores.Add(Score);
                HighScores = HighScores.OrderByDescending(score => score)
                                       .Take(5)
                                       .ToList();
                IsNewHighScore = true;
            }
            else
            {
                IsNewHighScore = false;
            }

            // Save updated high scores
            GameWorldHelper.SaveHighScores(HighScores);
        }

        private void PlayBackgroundMusic()
        {
            // Stop any currently playing music
            MediaPlayer.Stop();

            // Optional: Set MediaPlayer properties
            MediaPlayer.IsRepeating = true; // Loop the background music

            // Select and play the appropriate music
            if (Level == 1)
            {
                MediaPlayer.Play(Art.Map1Bgm);
            }
            else
            {
                MediaPlayer.Play(Art.Map2Bgm);
            }      
        }

    }
}
