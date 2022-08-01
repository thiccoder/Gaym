using Assets.Scripts.Globals.Commands;

namespace Assets.Scripts.Globals.Abilities
{
    public class Mover : AbilityObject
    {
        public float MoveSpeed;
        public float TurnSpeed;
        public Mover() : base()
        {
            CommandType = typeof(Move);
            TargetType = typeof(LocationTarget);
        }
    }
}