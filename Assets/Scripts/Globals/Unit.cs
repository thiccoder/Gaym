using System.Collections.Generic;
using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Orders;
using UnityEngine;

namespace Assets.Scripts.Globals
{
    public class Unit
    {
        public readonly GameObject DefaultPrefab;
        private Player _owner;
        public Player Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
                _owner.AllUnits.Add(this);
            }
        }
        public Transform Transform { get { return widget.transform; }}
        public HashSet<Order> Orders;
        public float Health;
        public float MaxHealth;
        public float HealthRegen;
        public float Stamina;
        public float MaxStamina;
        public float StaminaRegen;
        private readonly Widget widget;

        public Unit(Widget wdt)
        {
            widget = wdt;
        }
        public static explicit operator Widget(Unit u)
        {
            return u.widget;
        }
        public Unit(Player owner,Vector2 loc,float facing) 
        {
            var obj = Object.Instantiate(DefaultPrefab,new Vector3(loc.x,Terrain.activeTerrain.SampleHeight(new Vector3(loc.x,0,loc.y)),loc.y),Quaternion.AngleAxis(facing,new Vector3(0,1,0)));
            widget = obj.GetComponent<Widget>();
            widget.Unit = this;
            Owner = owner;
        }
    }
}
