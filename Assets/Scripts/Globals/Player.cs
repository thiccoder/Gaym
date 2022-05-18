using GameEngine;
using System.Collections.Generic;

namespace Globals
{
    public class Player
    {
        private readonly LocalPlayer player;
        public HashSet<Unit> Selection
        {
            get
            {
                var gameobjs = player.GetSelected();
                HashSet<Unit> units = new();
                foreach (var obj in gameobjs)
                { 
                    units.Add((Unit)obj.GetComponent<Widget>());
                }
                return units;
            }
        }
        public HashSet<Unit> AllUnits;

        public Player() 
        {
            
        }
    }
}