using BombMan.Source.Components.GamePlay.Characters.Enemies;
using BombMan.Source.Components.GamePlay.Items;
using BombMan.Source.Core.Shared;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay.Worlds
{
    public partial class GameWorld
    {
        private void UpdateEnemies()
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _enemies[i];
                enemy.Update();
                enemy.HandleCollisionWithHero(_hero);
                enemy.HandleCollisionWithBlocks(_blocks);
                enemy.HandleCollisionWithBombs(_bombs);
            }
        }

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
            foreach (var corner in _spawnCorners)
            {
                if (!_blocks.Exists(b => b.GetBoundingRectangle().Contains(corner)) &&
                    !_enemies.Exists(e => e.GetBoundingRectangle().Contains(corner)))
                {
                    Enemy newEnemy = random.Next(2) == 0
                        ? new EnemyLvl1(corner, CharacterWidth, CharacterHeight, 1, HudHeight, TileSize)
                        : new EnemyLvl2(corner, CharacterWidth, CharacterHeight, 0.5f, HudHeight, TileSize, _hero);

                    if (!_blocks.Exists(b => b.GetBoundingRectangle().Intersects(new Rectangle(
                        (int)corner.X, (int)corner.Y, CharacterWidth, CharacterHeight))))
                    {
                        newEnemy.LoadContent();
                        _enemies.Add(newEnemy);
                    }
                }
            }
        }
    }
}
