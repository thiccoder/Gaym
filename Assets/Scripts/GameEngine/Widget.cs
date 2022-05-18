using Globals;
using UnityEngine;
using System;
namespace GameEngine
{
    public class Widget : MonoBehaviour
    {

        private GameObject model;
        private Unit _unit = null;
        private float h = 0;
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
        public void Start()
        {
            model = GetComponentInChildren<MeshRenderer>(false).gameObject;
            modelOffset = model.transform.localPosition;
            if (_unit is null) _unit = new Globals.Unit(this);
        }

        public void Update()
        {
            if (Height > 0)
            {
                h = Mathf.Lerp(h, Height, Time.deltaTime) + Mathf.Sin(Time.time) * (DeltaHeight / 2);
            }
            model.transform.localPosition = modelOffset + new Vector3(0, h, 0);
        }
        public static explicit operator Unit(Widget w)
        {
            return w._unit;
        }

    }
}
