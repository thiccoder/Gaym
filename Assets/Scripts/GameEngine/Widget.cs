using Assets.Scripts.Globals;
using UnityEngine;
using System;
namespace Assets.Scripts.GameEngine
{
    public class Widget : MonoBehaviour
    {
        private Unit _unit = null;
        private float h = 0;
        [HideInInspector]
        public Unit Unit
        {
            set
            {
                if (_unit is not null)
                {
                    throw new ArgumentException("\"Unit\" can be set only once");
                }
                else
                {
                    _unit = value;
                }
            }
        }
        public float Height;
        public float DeltaHeight;
        public float Size;
        public Vector3 modelOffset;
        public float Health;
        public float MaxHealth;
        public float HealthRegen;
        public float Stamina;
        public float MaxStamina;
        public float StaminaRegen;
        public string Name;
        public string Tooltip;
        public GameObject Model;

        public void Awake()
        {
            modelOffset = Model.transform.localPosition;
            if (_unit is null)
            {
                _unit = new Unit(this)
                {
                    Name = Name,
                    Tooltip = Tooltip,
                    MaxHealth = MaxHealth,
                    Health = Health,
                    HealthRegen = HealthRegen,
                    MaxStamina = MaxStamina,
                    Stamina = Stamina,
                    StaminaRegen = StaminaRegen
                };
            }
        }
        public void Update()
        {
            if (Height > 0)
            {
                h = Mathf.Lerp(0, Height, Time.deltaTime) + Mathf.Sin(Time.time) * (DeltaHeight / 2);
            }
            Model.transform.localPosition = modelOffset + new Vector3(0, h, 0);
            if (_unit.Health < _unit.MaxHealth)
            {
                _unit.Health = Mathf.Min(_unit.Health+_unit.HealthRegen * Time.deltaTime,_unit.MaxHealth);
            }
            Name = _unit.Name;
            Tooltip = _unit.Tooltip;
            MaxHealth = _unit.MaxHealth;
            Health = _unit.Health;
            HealthRegen = _unit.HealthRegen;
            MaxStamina = _unit.MaxStamina;
            Stamina = _unit.Stamina;
            StaminaRegen = _unit.StaminaRegen;
        }
        public static implicit operator Unit(Widget w)
        {
            return w._unit;
        }
    }
}
