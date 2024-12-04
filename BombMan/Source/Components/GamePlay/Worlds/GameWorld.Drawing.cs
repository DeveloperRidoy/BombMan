namespace BombMan.Source.Components.GamePlay.Worlds
{
    public partial class GameWorld
    {

        private void DrawFloors()
        {
            foreach (var floor in _floors)
            {
                floor.Draw();
            }
        }

        private void DrawBlocks()
        {
            foreach (var block in _blocks)
            {
                block.Draw();
            }
        }

        private void DrawEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Draw();
            }
        }

        private void DrawBombs()
        {
            foreach (var bomb in _bombs)
            {
                bomb.Draw();
            }
        }
    }
}
