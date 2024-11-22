using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
