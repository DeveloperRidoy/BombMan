using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    public class HighScoreMenu : BaseMenu
    {
        public HighScoreMenu() : base(
            "High Scores",
            new List<string>
            {
                "Top 5 Players!",
                "",
                " 1 - Ridoy",
                " 2 - Shong",
                " 3 - Farrukh",
                " 4 - Valentine",
                " 5 - ---",
                "",
                "Someday you could be here too!"
            },
            true // Indicates this is a submenu
        )
        {
        }
    }
}
