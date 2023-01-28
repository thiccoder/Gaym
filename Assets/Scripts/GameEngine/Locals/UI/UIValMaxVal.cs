using UnityEngine;
namespace Assets.Scripts.GameEngine.Locals.UI
{
    public class UIValMaxVal : UINumeric
    {
        public float MaxVal;
        public override float Value { get { return _value; } set { _value = Mathf.Min(value,MaxVal); Text = value.ToString()+ "\\"+MaxVal.ToString(); } }

    }
}