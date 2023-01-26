using Assets.Scripts.Globals.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AI;
using UnityEngine;

namespace Assets.Scripts.Globals.Commands
{
    internal class Build : Command
    {
        [SerializeField]
        protected BuildGridAgent buildingPrefab;
        [SerializeField]
        protected BuildGridAgent currentBuilding;
        [SerializeField]
        protected LocationTarget target;
        [SerializeField]
        protected Builder builder;
        public override void Issue(Target target)
        {
            Issue(target as LocationTarget);
        }
        public virtual void Issue(LocationTarget target)
        {
            Issuing = true;
            Completed = false;
            builder.OnIssue(target, Caster);
        }
        public override void Abort()
        {
            builder.OnAbort(Caster);
            Completed = true;
            Issuing = false;
        }
        private void Update()
        {
            if (Issuing)
            {
                builder.OnUpdate(Caster);
                if (currentBuilding.Finished == true)
                {
                    Completed = true;
                    Issuing = false;
                }
            }
        }
        public override string ToCommandString()
        {
            return $"Build a building at {builder}";
        }
    }
}
