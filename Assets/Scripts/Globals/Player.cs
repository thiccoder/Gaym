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
                    if (obj.TryGetComponent(out Widget widget))
                    {
                        units.Add((Unit)widget);
                    }
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