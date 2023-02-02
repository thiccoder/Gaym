using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Globals.Resourses
{
    public class Plastic : Resourse
    {
        [SerializeField]
        protected override int count { get; set; } = 5000;

        [SerializeField]
        protected override int resLimit { get; } = 5000;
        public override string resName { get; } = "Plastic";

        [SerializeField]
        private Text resCounter;
        public Plastic() { }
        public void updateHUD() {
            resCounter.text = $"{count}|{resLimit}";
        }

        public override void addCount(int value)
        {
            if (count + value < resLimit)
            {
                count += value;
            }
            updateHUD();
        }
        public override void subCount(int value)
        {
            if (count - value >= 0)
            {
                count -= value;
            }
            updateHUD();
        }

        public override void setCount(int value)
        {
            if (count - value >= 0 && count + value < resLimit)
            {
                count = value;
            }
            updateHUD();
        }
    }
}

