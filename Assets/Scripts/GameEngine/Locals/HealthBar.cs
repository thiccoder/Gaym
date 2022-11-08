using UnityEngine;
using Assets.Scripts.GameEngine.Locals.UI;
namespace Assets.Scripts.GameEngine.Locals
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private GameObject uiBarPrefab;
        [SerializeField]
        private Widget widget;
        [SerializeField]
        private float barHeight;
        private UIBar uibar;
        private void Start()
        {
            uibar = Instantiate(uiBarPrefab).GetComponent<UIBar>();
            uibar.transform.SetParent(FindObjectOfType<Canvas>().transform);
        }
        private void Update()
        {
            Vector2 uibarPos = Camera.main.WorldToScreenPoint(gameObject.transform.position + Vector3.up * barHeight);
            uibar.transform.position = uibarPos;
            uibar.Value = widget.Health/widget.MaxHealth;
        }
        private void OnDestroy()
        {
            Destroy(uibar.gameObject);
        }
    }
}