using Assets.Scripts.GameEngine.Locals;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.GameEngine.Locals.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private UnitUIView view;
        [SerializeField]
        private LocalPlayer player;

        void Update()
        {
            if (player.Selected.Count == 0)
            {
                view.ViewingUnit = null;
            }
            else 
            {
                view.ViewingUnit = player.Selected.First();
            }
        }
    }
}