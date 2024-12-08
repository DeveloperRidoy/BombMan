using BombMan.Source.Components.GamePlay.Characters.Enemies;
using BombMan.Source.Components.GamePlay.Characters.Heroes;
using BombMan.Source.Components.GamePlay.Items;
using BombMan.Source.Components.GamePlay.Objects;
using System;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay.Worlds
{
    public partial class GameWorld
    {

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

                    // Calculate the position of the block
                    Vector2 blockPosition = CalculateItemPosition(x, y);

                    Vector2 heroStartPosition = CalculateHeroStartPosition();

                    // Ensure the block is not placed too close to the hero or other blocks
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
            _hero = new Hero(CalculateHeroStartPosition(),
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
                const int maxAttempts = 100; // limiting attempts to 100

                while (attempts < maxAttempts)
                {
                    x = random.Next(WorldWidth);
                    y = random.Next(WorldHeight);

                    Vector2 enemyPosition = CalculateItemPosition(x, y);

                    Rectangle enemyBounds = new((int)enemyPosition.X, (int)enemyPosition.Y, CharacterWidth, CharacterHeight);

                    // Ensure no overlap with hero, blocks, or other enemies
                    bool isPositionValid = Vector2.Distance(enemyPosition, _hero.Position) >= SafeZoneRadius &&
                                           !_blocks.Exists(b => b.GetBoundingRectangle().Intersects(enemyBounds)) &&
                                           !_enemies.Exists(e => e.GetBoundingRectangle().Intersects(enemyBounds));

                    // place enemy if position is valid
                    if (isPositionValid)
                    {
                        if (Level == 1)
                        {
                            // Place only enemy type 1 in level 1
                            _enemies.Add(new EnemyLvl1(enemyPosition, CharacterWidth, CharacterHeight, 1, HudHeight, TileSize));
                        }
                        else
                        {
                            // Randomly choose between enemy types
                            var toss = random.Next(2);
                            if (toss == 0)
                                _enemies.Add(new EnemyLvl2(enemyPosition, CharacterWidth, CharacterHeight, 0.5f, HudHeight, TileSize, _hero));
                            else
                                _enemies.Add(new EnemyLvl1(enemyPosition, CharacterWidth, CharacterHeight, 1, HudHeight, TileSize));
                        }
                        break;
                    }

                    attempts++;
                }

                if (attempts >= maxAttempts)
                {
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

            //// Prevent placing multiple bombs at the same location
            if (!_bombs.Exists(b => b.Position == bombPosition))
            {
                Bomb bomb = new(bombPosition, TileSize, TileSize);
                bomb.LoadContent();
                _bombs.Add(bomb);

                // Associate the bomb with the hero
                hero.LastPlacedBomb = bomb;
            }
        }

        private void ResetHeroBombReference()
        {
            if (_hero.LastPlacedBomb != null && !_hero.GetBoundingRectangle().Intersects(_hero.LastPlacedBomb.GetBoundingRectangle()))
            {
                _hero.LastPlacedBomb = null;
            }
        }

        private void HandleHeroCollision(Vector2 previousPosition)
        {
            foreach (var block in _blocks)
            {
                // Revert hero's position if colliding with a block
                if (_hero.GetBoundingRectangle().Intersects(block.GetBoundingRectangle()))
                {
                    _hero.Position = previousPosition;
                    _hero.Stop();
                    break;
                }
            }

            // Prevent hero from walking over bombs
            foreach (var bomb in _bombs)
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
        }

        public Vector2[] GenerateSpawnCorners ()
        {
            var corners =  new Vector2[]
            {
                new (StageBackgroundPadding + _horizontalCenterOffset, HudHeight + StageBackgroundPadding),
                new (WorldWidth * TileSize - StageBackgroundPadding + _horizontalCenterOffset, HudHeight + StageBackgroundPadding),
                new (StageBackgroundPadding + _horizontalCenterOffset, (WorldHeight - 1) * TileSize + HudHeight - StageBackgroundPadding),
                new (WorldWidth * TileSize - StageBackgroundPadding + _horizontalCenterOffset, (WorldHeight - 1) * TileSize + HudHeight - StageBackgroundPadding)
            };
            return corners;
        }

        public Vector2 CalculateHeroStartPosition()
        {
            return new(
                WorldWidth / 2 * TileSize + StageBackgroundPadding + _horizontalCenterOffset,
                WorldHeight / 2 * TileSize + HudHeight + StageBackgroundPadding
            );
        }

        public Vector2 CalculateItemPosition (int x, int y)
        {
            return new(
                 x * TileSize + StageBackgroundPadding + _horizontalCenterOffset,
                 y * TileSize + HudHeight + StageBackgroundPadding
             );
        }
    }
}
