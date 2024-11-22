using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    internal class LevelSelectMenu: BaseMenu
    {

        public LevelSelectMenu() : base(
            "Select Level",
            new List<MenuItem>
            {
                new ("Level 1", Level1),
                new ("Level 2", Level2),
                new ("Level 3", Level3),
                new ("Level 4", Level4),
                new ("Level 5", Level5),
            },
            true
        )
        {
        }
        private static void Level1()
        {
            // Start level 1 logic
        }
        private static void Level2()
        {
            // Start level 2 logic
        }
        private static void Level3()
        {
            // Start level 3 logic
        }
        private static void Level4()
        {
            // Start level 4 logic
        }
        private static void Level5()
        {
            // Start level 5 logic
        }
    }
}
