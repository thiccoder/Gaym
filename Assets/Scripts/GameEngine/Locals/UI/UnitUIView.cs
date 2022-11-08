using UnityEngine;
using Assets.Scripts.Globals;
namespace Assets.Scripts.GameEngine.Locals.UI
{
    public class UnitUIView : MonoBehaviour
    {
        [SerializeField]
        private UIValMaxVal Health;
        [SerializeField]
        private UIValMaxVal Stamina;
        private Unit _viewingUnit;
        public Unit ViewingUnit
        { 
            get 
            { 
                return _viewingUnit;
            }
            set 
            { 
                _viewingUnit = value;
                UpdateMaxVals = true;
                if (_viewingUnit is null)
                {
                    Health.ShowValue = false;
                    Stamina.ShowValue = false;
                    UpdateValues = false;
                }
                else
                {
                    Health.ShowValue = true;
                    Stamina.ShowValue = true;
                    UpdateValues = true;
                }
            }
        }
        public bool UpdateMaxVals;
        public bool UpdateValues;
        void Update()
        {

            if (UpdateValues) 
            {
                if (UpdateMaxVals) 
                {
                    UpdateMaxVals = false;
                    Health.MaxVal = _viewingUnit.MaxHealth;
                    Stamina.MaxVal = _viewingUnit.MaxStamina;
                }
                Health.Value = _viewingUnit.Health;
                Stamina.Value = _viewingUnit.Stamina;
            }
        }
    }
}