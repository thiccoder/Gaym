using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Globals.Resourses
{
    public abstract class Resourse : MonoBehaviour
    {
        protected abstract int count { get; set; }
        protected abstract int resLimit { get; }
        public abstract string resName { get; }
        public abstract void addCount(int value);
        public abstract void subCount(int value);
        public abstract void setCount(int value);
    }
}


