using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombMan.Source.Components.Menus
{
    public class MenuItem
    {
        public string Name { get; }
        public Action Action { get; set; }
        public MenuItem(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }
}
