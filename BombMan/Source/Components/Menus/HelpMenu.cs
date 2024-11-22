using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BombMan.Source.Components.Menus
{
    public class HelpMenu : BaseMenu
    {
        public HelpMenu() : base(
            "Help",
            new List<string>
            {
                "Welcome to BombMan!",
                "",
                "Objective:",
                "Defeat all enemies and break blocks to find the exit. Place bombs strategically to clear your path, eliminate enemies, and progress to the next level.",
                "",
                "Controls:",
                " - Move Up: W or Up Arrow",
                " - Move Down: S or Down Arrow",
                " - Move Left: A or Left Arrow",
                " - Move Right: D or Right Arrow",
                " - Place Bomb: Spacebar",
                "",
                "Tips:",
                " - Bombs will explode in a cross pattern after a short delay.",
                " - Use blocks to shield yourself from bomb explosions.",
                " - Watch out for power-ups hidden in blocks! They can increase your speed, bomb count, or explosion range.",
                " - Avoid enemy contact and bomb explosions.",
                "",
                "Good luck, and have fun!"
            },
            true // Indicates this is a submenu
        )
        {
        }
    }
}
