using Assets.Scripts.Globals.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Globals.Abilities
{
    internal class Builder : AbilityObject
    {
        public float BuildSpeed;
        public float TurnSpeed;
        public Builder() : base()
        {
            CommandType = typeof(Build);
            TargetType = typeof(LocationTarget);
        }
    }
}
