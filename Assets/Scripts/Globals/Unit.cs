using System.Collections.Generic;
using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Commands;
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
                _owner.AllUnits.Remove(this);
                _owner = value;
                _owner.AllUnits.Add(this);
            }
        }
        public Transform Transform { get { return _widget.transform; } }
        public bool Alive { get { return Health > 0; } }
        public HashSet<Command> Commands;
        public float Health;
        public float MaxHealth;
        public float HealthRegen;
        public float Stamina;
        public float MaxStamina;
        public float StaminaRegen;
        public string Name;
        public string Tooltip;
        private readonly Widget _widget;
        public Unit(Widget wdt)
        {
            _widget = wdt;
        }
        public static implicit operator Widget(Unit u)
        {
            return u._widget;
        }
        public Unit(Player owner, Vector2 loc, float facing)
        {
            var obj = Object.Instantiate(DefaultPrefab, new Vector3(loc.x, Terrain.activeTerrain.SampleHeight(new Vector3(loc.x, 0, loc.y) + Terrain.activeTerrain.transform.position), loc.y), Quaternion.AngleAxis(facing, new Vector3(0, 1, 0)));
            _widget = obj.GetComponent<Widget>();
            Commands = new HashSet<Command>(obj.GetComponents<Command>());
            _widget.Unit = this;
            Owner = owner;
        }
        public void OnDamage(Damage dmg)
        {
            Health -= dmg.Amount;
            if (!Alive) 
            {
                Object.Destroy(_widget.gameObject);
            }
        }
    }
}
