using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.GameEngine
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private GameObject uibar;
        [SerializeField]
        private Widget widget;
        [SerializeField]
        private float barHeight;

        void Update()
        {
            Vector2 uibarPos = Camera.main.WorldToScreenPoint(gameObject.transform.position + Vector3.up * barHeight);
            uibar.transform.position = uibarPos;
        }
    }
}