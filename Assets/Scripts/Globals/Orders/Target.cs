using Assets.Scripts.Globals;
using UnityEngine;

namespace Assets.Scripts.Globals.Orders
{
    public abstract class Target
    {
        public static NullTarget Null = new();
        public virtual object Value { get; set; }
    }
    public abstract class GenericTarget<T> : Target
    {
        public new virtual T Value { get; set; }
    }
    public class NullTarget : Target 
    {
    
    }
    public class LocationTarget : GenericTarget<Vector3>
    {
        private Vector3 _location;
        public LocationTarget(Vector3 loc)
        {
            _location = loc;
        }
        public override Vector3 Value { get { return _location; } set { _location = value; } }
    }
    public class GameObjectTarget : GenericTarget<GameObject>
    {
        private GameObject _object;
        public GameObjectTarget(GameObject obj)
        {
            _object = obj;
        }
        public override GameObject Value { get { return _object; } set { _object = value; } }
    }
    public class UnitTarget : GenericTarget<Unit> 
    {
        private Unit _unit;
        public UnitTarget(Unit u) 
        {
            _unit = u;
        }
        public override Unit Value { get { return _unit; } set { _unit = value; } }
    }
}
