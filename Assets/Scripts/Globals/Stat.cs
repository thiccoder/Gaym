using System;
using GameEngine;

namespace Globals
{
    public struct Stat
    {
        public decimal Value;
        public string Name;
        public Tuple<decimal, decimal> range;
        public StatBehaviour bhv;
    }
}