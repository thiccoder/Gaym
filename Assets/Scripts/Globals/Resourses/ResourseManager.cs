using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Globals.Resourses
{
    public abstract class ResourseManager : MonoBehaviour
    {
        public abstract int CurResCount { get; set; }
        public abstract int ResLimit { get; }
        public abstract string ResName { get; }
    }
}


