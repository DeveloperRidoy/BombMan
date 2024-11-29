using BombMan.Source.Components.GamePlay;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    public class HighScoreMenu : BaseMenu
    {
        public HighScoreMenu() : base(
            "High Scores:",
            FormatHighScores(GameWorldHelper.LoadHighScores()), // Format high scores as a list of strings
            true // Indicates this is a submenu
        )
        {
        }

        private static List<string> FormatHighScores(List<int> highScores)
        {
            List<string> result = new ();
            if (highScores.Count > 0)
            {
                foreach (var score in highScores)
                {
                    result.Add(score.ToString());
                }
            }
            else
            {
                result.Add("No High Scores");
            }

            return result;
        }
    }
}
