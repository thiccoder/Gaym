using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Globals.Resourses
{
    public abstract class Resourse : MonoBehaviour
    {
        protected abstract int count { get; set; }
        protected abstract int resLimit { get; }
        public abstract string resName { get; }
        protected abstract GameObject resCounter { get; }
        public abstract void updateHUD();
    }
}


