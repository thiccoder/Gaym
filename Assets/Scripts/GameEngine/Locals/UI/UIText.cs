using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Assets.Scripts.GameEngine.Locals.UI
{
    public class UIText : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;
        private bool _showValue = true;
        public bool ShowValue { get { return _showValue; }set { _showValue = value; text.gameObject.SetActive(value); } }
        public string Text { get { return text.text; } set { text.text = value; } }
    }
}
