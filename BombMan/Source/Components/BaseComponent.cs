namespace BombMan.Source.Components
{
    public abstract class BaseComponent
    {

        public BaseComponent() { }

        public abstract void LoadContent ();

        public abstract void Update();

        public abstract void Draw();
    }
}
