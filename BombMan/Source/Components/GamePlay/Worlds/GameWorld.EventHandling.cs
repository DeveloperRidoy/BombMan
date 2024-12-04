using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework.Media;
using System.Linq;

namespace BombMan.Source.Components.GamePlay.Worlds
{
    public partial class GameWorld
    {
        private void HandleGameOver()
        {
            if (_hero.Health <= 0)
            {
                MediaPlayer.Stop();
                UpdateHighScores();
                SaveHighScores(HighScores);
                OnGameOver?.Invoke(Score, HighScores.Max(), IsNewHighScore);
            }
        }

        private void UpdateHighScores()
        {
            if (Score > 0 && (HighScores.Count < 5 || Score > HighScores.Min()))
            {
                HighScores.Add(Score);
                HighScores = HighScores.OrderByDescending(score => score)
                                       .Take(5)
                                       .ToList();
                IsNewHighScore = HighScores.Count == 1 || Score > HighScores.Min();
            }
            else
            {
                IsNewHighScore = false;
            }
        }

        private void PlayBackgroundMusic()
        {
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = true;

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
