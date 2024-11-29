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

namespace BombMan.Source.Components.GamePlay
{
    public class GameWorld
    {
        public event Action<int, int, bool> OnGameOver;
        private const int WorldWidth = 10;
        private const int WorldHeight = 10;
        private const int TileSize = 64;
        private const int CharacterWidth = 30;
        private const int CharacterHeight = 40;
        private const int HudHeight = 150;

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

        public GameWorld(bool loadGame)
        {
            _floors = new Floor[WorldHeight, WorldWidth];
            _blocks = new List<Block>();
            _enemies = new List<Enemy>();
            _bombs = new List<Bomb>();

            HighScores = GameWorldHelper.LoadHighScores() ?? new List<int>();

            _corners = new[]
            {
                new Vector2(0, HudHeight), // Top-left corner
                new Vector2((WorldWidth - 1) * TileSize, HudHeight), // Top-right corner
                new Vector2(0, (WorldHeight - 1) * TileSize + HudHeight), // Bottom-left corner
                new Vector2((WorldWidth - 1) * TileSize, (WorldHeight - 1) * TileSize + HudHeight) // Bottom-right corner
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
        }

        private void InitializeDefaultWorld()
        {
            PlaceFloors();
            PlaceBlocks();
            PlaceHero();
            PlaceEnemies();
        }


        private void PlaceFloors()
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    _floors[y, x] = new Floor(
                        new Vector2(x * TileSize, y * TileSize + HudHeight),
                        TileSize,
                        TileSize);
                }
            }
        }

        private void PlaceBlocks()
        {
            Random random = new();
            for (int i = 0; i < 15; i++) // Place 15 random blocks
            {
                int x, y;
                do
                {
                    x = random.Next(WorldWidth);
                    y = random.Next(WorldHeight);

                    Vector2 blockPosition = new(x * TileSize, y * TileSize + HudHeight);
                    Vector2 heroStartPosition = new(WorldWidth / 2 * TileSize, WorldHeight / 2 * TileSize + HudHeight);

                    // Ensure block is not in the safe zone
                    if (Vector2.Distance(blockPosition, heroStartPosition) < SafeZoneRadius)
                    {
                        continue;
                    }

                    // Ensure block does not overlap with existing blocks
                    if (_blocks.Exists(b => b.Position == blockPosition))
                    {
                        continue;
                    }

                    // Add the block
                    _blocks.Add(new Block(blockPosition, TileSize, TileSize, Level == 1 ? ELvl.Lvl1 : ELvl.Lvl2));
                    break;

                } while (true);
            }
        }


        private void PlaceHero()
        {
            _hero = new Hero(
                new Vector2(WorldWidth / 2 * TileSize, WorldHeight / 2 * TileSize + HudHeight),
                CharacterWidth,
                CharacterHeight,
                1f,
                5);
        }

        private void PlaceEnemies()
        {
            Random random = new();
            _enemies.Clear();

            int numEnemies = Level == 1 ? 5 : 5 + (Level - 1) * 2; // Increase enemies with level

            for (int i = 0; i < numEnemies; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(WorldWidth);
                    y = random.Next(WorldHeight);

                    Vector2 enemyPosition = new(x * TileSize, y * TileSize + HudHeight);
                    Vector2 heroStartPosition = new(WorldWidth / 2 * TileSize, WorldHeight / 2 * TileSize + HudHeight);

                    // Ensure enemy is not in the safe zone
                    if (Vector2.Distance(enemyPosition, heroStartPosition) < SafeZoneRadius)
                    {
                        continue;
                    }

                    // Ensure enemy does not overlap with blocks or other enemies
                    if (_blocks.Exists(b => b.Position == enemyPosition) ||
                        _enemies.Exists(e => e.Position == enemyPosition))
                    {
                        continue;
                    }

                    // Add the enemy
                    if (Level == 1 || i % 2 == 0)
                    {
                        _enemies.Add(new EnemyLvl1(enemyPosition, CharacterWidth, CharacterHeight, 1, WorldWidth, WorldHeight, HudHeight, TileSize));
                    }
                    else
                    {
                        _enemies.Add(new EnemyLvl2(enemyPosition, CharacterWidth, CharacterHeight, 0.5f, WorldWidth, WorldHeight, HudHeight, TileSize, _hero));
                    }
                    break;

                } while (true);
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
            EnsureHeroStaysWithinBounds();
            UpdateEnemies();
            UpdateBombs();

            // Spawn new enemies every 10 seconds if the level is higher than 1
            if (Level > 1)
            {
                _enemySpawnTimer += Resource.UpdateGameTime.ElapsedGameTime;
                if (_enemySpawnTimer >= _enemySpawnInterval)
                {
                    SpawnEnemiesFromCorners();
                    _enemySpawnTimer = TimeSpan.Zero;
                }
            }

            // Reset the hero's bomb reference when moving away
            if (_hero.LastPlacedBomb != null && !_hero.GetBoundingRectangle().Intersects(_hero.LastPlacedBomb.GetBoundingRectangle()))
            {
                _hero.LastPlacedBomb = null;
            }

            _hud.Update();
            HandleGameOver();
        }

        private void SpawnEnemiesFromCorners()
        {
            Random random = new();
            foreach (var corner in _corners)
            {
                // Check if the corner is free to spawn
                if (!_blocks.Exists(b => b.GetBoundingRectangle().Contains(corner)) &&
                    !_enemies.Exists(e => e.GetBoundingRectangle().Contains(corner)))
                {
                    // Alternate between Level 1 and Level 2 enemies
                    Enemy newEnemy;
                    if (random.Next(2) == 0)
                    {
                        newEnemy = new EnemyLvl1(corner, CharacterWidth, CharacterHeight, 1, WorldWidth, WorldHeight, HudHeight, TileSize);
                    }
                    else
                    {
                        newEnemy = new EnemyLvl2(corner, CharacterWidth, CharacterHeight, 0.5f, WorldWidth, WorldHeight, HudHeight, TileSize, _hero);
                    }

                    newEnemy.LoadContent();
                    _enemies.Add(newEnemy);
                }
            }
        }



        private void HandleGameOver()
        {
            if (_hero.Health <= 0)
            {
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

        private void EnsureHeroStaysWithinBounds()
        {
            _hero.Position = new Vector2(
                Math.Clamp(_hero.Position.X, 0, WorldWidth * TileSize - CharacterWidth),
                Math.Clamp(_hero.Position.Y, HudHeight, WorldHeight * TileSize - CharacterHeight + HudHeight)
            );
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
        }


        public void Draw()
        {
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
                    enemy = new EnemyLvl1(new Vector2(enemyX, enemyY + HudHeight), CharacterWidth, CharacterHeight, 1, WorldWidth, WorldHeight, HudHeight, TileSize);
                }
                else
                {
                    enemy = new EnemyLvl2(new Vector2(enemyX, enemyY + HudHeight), CharacterWidth, CharacterHeight, 0.5f, WorldWidth, WorldHeight, HudHeight, TileSize, _hero);
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

    }
}
