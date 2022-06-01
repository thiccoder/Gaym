using Assets.Scripts.GameEngine;

namespace Assets.Scripts.Globals.Commands
{
    public class Stop : Command
    {
        public override void Issue(Target target)
        {
            Issuing = false;
            Completed = true;
        }
        public override void Abort()
        {
            Issuing = false;
            Completed = false;
        }
        public override string ToCommandString()
        {
            return "Stop";
        }
    }
}