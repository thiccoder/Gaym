using UnityEngine;
using Assets.Scripts.GameEngine;
using System;

namespace Assets.Scripts.Globals.Orders
{
    public abstract class Order : MonoBehaviour
    {
        [HideInInspector]
        public bool Completed = false;
        [HideInInspector]
        public bool Issuing = false;
        public Widget unit;
        [HideInInspector]
        public bool CanAbort = true;
        [HideInInspector]
        public Vector2 Range = Vector2.positiveInfinity;
        public abstract void Issue(Target target);
        public abstract void Abort();
        public override string ToString()
        {
            return $"\"{ToOrderString()}\", {(Issuing ? "I" : "Not i")}ssuing, {(Completed ? "C" : "Not c")}ompleted ";
        }
        public abstract string ToOrderString();
    }
}
