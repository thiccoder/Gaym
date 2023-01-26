using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Globals.Resourses
{
    public class Plastic : Resourse
    {
        [SerializeField]
        protected override int count { get; set; }
        [SerializeField]
        protected override int resLimit { get; }
        public override string resName { get; }
        [SerializeField]
        protected override GameObject resCounter { get; }
        public Plastic()
        {
            this.count = 100;
            this.resLimit = 100;
            this.resName = "Plastic";
        }
        public override void updateHUD()
        {

        }
    }
}

