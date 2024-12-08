using BombMan.Source.Components.GamePlay.Characters.Enemies;
using BombMan.Source.Components.GamePlay.Characters.Heroes;
using BombMan.Source.Components.GamePlay.Items;
using BombMan.Source.Components.GamePlay.Objects;
using System.IO;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BombMan.Source.Components.GamePlay.Worlds
{
    public partial class GameWorld
    {
        private void LoadFromFile()
        {
            string[] lines = File.ReadAllLines(SaveFilePath);
            int currentLine = 0;

            PlaceFloors();

            Level = int.Parse(lines[currentLine++]);
            Score = int.Parse(lines[currentLine++]);

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

            string[] heroParts = lines[currentLine++].Split(',');
            float heroX = float.Parse(heroParts[0]);
            float heroY = float.Parse(heroParts[1]);
            _hero = new Hero(new Vector2(heroX, heroY + HudHeight), CharacterWidth, CharacterHeight, 1f, 5);
            _hero.OnPlaceBomb += PlaceBombAtPosition;

            int enemyCount = int.Parse(lines[currentLine++]);
            _enemies.Clear();
            for (int i = 0; i < enemyCount; i++)
            {
                string[] enemyParts = lines[currentLine++].Split(',');
                float enemyX = float.Parse(enemyParts[0]);
                float enemyY = float.Parse(enemyParts[1]);
                int enemyType = int.Parse(enemyParts[2]);
                Enemy enemy = enemyType == 1
                    ? new EnemyLvl1(new Vector2(enemyX, enemyY + HudHeight), CharacterWidth, CharacterHeight, 1, HudHeight, TileSize)
                    : new EnemyLvl2(new Vector2(enemyX, enemyY + HudHeight), CharacterWidth, CharacterHeight, 0.5f, HudHeight, TileSize, _hero);
                _enemies.Add(enemy);
            }

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
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(SaveFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using StreamWriter writer = new(SaveFilePath);

            writer.WriteLine(Level);
            writer.WriteLine(Score);

            writer.WriteLine(_blocks.Count);
            foreach (var block in _blocks)
            {
                writer.WriteLine($"{block.Position.X},{block.Position.Y - HudHeight},{block.Lvl}");
            }

            writer.WriteLine($"{_hero.Position.X},{_hero.Position.Y - HudHeight}");

            writer.WriteLine(_enemies.Count);
            foreach (var enemy in _enemies)
            {
                int enemyType = enemy is EnemyLvl1 ? 1 : 2;
                writer.WriteLine($"{enemy.Position.X},{enemy.Position.Y - HudHeight},{enemyType}");
            }

            writer.WriteLine(_bombs.Count);
            foreach (var bomb in _bombs)
            {
                writer.WriteLine($"{bomb.Position.X},{bomb.Position.Y - HudHeight}");
            }
        }


        public static List<int> LoadHighScores()
        {
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(HighScoreFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(HighScoreFilePath))
            {
                var highScores = File.ReadAllLines(HighScoreFilePath)
                                     .Where(line => int.TryParse(line, out _)) // Ignoring invalid entries
                                     .Select(int.Parse)
                                     .OrderByDescending(score => score)
                                     .Take(MaxHighScores)
                                     .ToList();

                if (highScores.Count > 0)
                    return highScores;
            }

            // Return an empty list if no valid scores are found
            return [];
        }

        public static void SaveHighScores(List<int> highScores)
        {
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(HighScoreFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var sortedHighScores = highScores.OrderByDescending(score => score)
                                             .Take(MaxHighScores)
                                             .ToList();

            File.WriteAllLines(HighScoreFilePath, sortedHighScores.Select(score => score.ToString()));
        }
    }
}
