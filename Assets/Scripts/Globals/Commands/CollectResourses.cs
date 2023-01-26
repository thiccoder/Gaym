using Assets.Scripts.Globals.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Globals.Commands
{
    public class CollectResourses : Command
    {
        private UnitTarget targetResourses;
        private UnitTarget targetBase;
        [SerializeField]
        private byte collectTime;
        private float time;
        public void Start()
        {
            
        }
        public override void Abort()
        {
            Issuing = false;
            Completed = false;
        }
        public override void Issue(Target target)
        {
            Issue(target as UnitTarget);
        }
        public void Issue(UnitTarget targRes, UnitTarget targBase)
        {
            targetResourses = targRes;
            targetBase = targBase;
            Issuing = true;
            Completed = false;
            time = 0;
        }
        public void Update()
        {
            if (Issuing)
            {
                if (targetResourses.Value.Alive && targetBase.Value.Alive)
                {
                    
                }
                else
                {
                    Completed = true;
                    Issuing = false;
                }
                time += Time.deltaTime;
            }
        }
        public override string ToCommandString()
        {
            return $"CollectResourses from {targetResourses.Value} to {targetBase.Value}";
        }
    }
}
