using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Globals.Resourses
{
    public class PlasticResManager : ResourseManager
    {
        private int curResCount = 50;
        [SerializeField]
        public override int CurResCount
        {
            get
            {
                return curResCount;
            }
            set
            {
                if (CurResCount + value >= 0 && curResCount + value < ResLimit)
                {
                    curResCount = value;
                }
                updateHUD();
            }
        }

        public override int ResLimit { get; } = 5000;
        public override string ResName { get; } = "Plastic";

        [SerializeField]
        private TextMeshProUGUI resCounter;
        public PlasticResManager() { }
        private void updateHUD()
        {
            resCounter.text = $"{CurResCount}|{ResLimit}";
        }
    }
}

