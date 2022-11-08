using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Assets.Scripts.GameEngine.Locals.UI
{
    public class UINumeric : UIText
    {
        protected float _value = 0;
        public virtual float Value { get { return _value; } set { _value = value;Text = value.ToString(); } }
    }
}