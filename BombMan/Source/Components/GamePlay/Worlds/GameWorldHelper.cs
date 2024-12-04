using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BombMan.Source.Components.GamePlay.Worlds
{
    public static class GameWorldHelper
    {
        private static readonly string HighScoreFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BombMan",
            "HighScores.bombMan"
        );
        private const int MaxHighScores = 5;

        public static List<int> LoadHighScores()
        {
            // Ensure the directory exists before attempting to read
            var directory = Path.GetDirectoryName(HighScoreFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(HighScoreFilePath))
            {
                var highScores = File.ReadAllLines(HighScoreFilePath)
                                     .Where(line => int.TryParse(line, out _)) // Ignore invalid entries
                                     .Select(int.Parse)
                                     .OrderByDescending(score => score)
                                     .Take(MaxHighScores)
                                     .ToList();

                if (highScores.Count > 0)
                    return highScores;
            }

            // Return an empty list if no valid scores are found
            return new List<int>();
        }

        public static void SaveHighScores(List<int> highScores)
        {
            // Ensure the directory exists before saving
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
