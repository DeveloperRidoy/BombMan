using BombMan.Source.Core.IO;
using BombMan.Source.Core.Shared;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BombMan.Source.Components.GamePlay.Worlds
{
    public partial class GameWorld
    {
        private void InitializeController()
        {
            var controllerPosition = new Vector2(
                Resource.ScreenWidth / 2 - Controller.GetWidth() / 2,
                Resource.ScreenHeight - Controller.GetWidth()
            );
            Debug.Write($"{controllerPosition}, {Resource.ScreenWidth}, {Resource.ScreenHeight}, {Controller.Width}, {Controller.Height}");
            _controller = new Controller(controllerPosition);
        }

        private void InitializeStageBackground()
        {
            var levelType = Level switch
            {
                1 => ELvl.Lvl1,
                _ => ELvl.Lvl2,
            };

            _stageBackground = new StageBackground(
                new Vector2(0 + _horizontalCenterOffset, HudHeight),
                WorldWidth * TileSize + StageBackgroundPadding * 2,
                WorldHeight * TileSize + StageBackgroundPadding * 2,
                levelType
            );
        }

        private void InitializeDefaultWorld()
        {
            PlaceFloors();
            PlaceHero();
            PlaceBlocks();
            PlaceEnemies();
        }

        private void InitializeNextLevel()
        {
            InitializeStageBackground();
            _blocks.Clear();
            _shouldClearBombs = true;
            _enemies.Clear();
            _hero.Health = 5; // Reset hero health
            _hero.Position = CalculateHeroStartPosition(); // Reset hero position
            PlaceBlocks();
            PlaceEnemies();
            LoadAllContent();
            PlayBackgroundMusic();
        }
    }
}
