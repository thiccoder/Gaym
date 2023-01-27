
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.GameEngine.Locals
{

    public class CommandButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private KeyCode hotkey;
        [SerializeField]
        private string commandName;
        [SerializeField]
        private string targetTypeName;
        public Type Command;
        private Type targetType;
        public event Action<Type, Type> OnPress;
        private bool isActive;
        public bool IsActive { get { return isActive; }set { isActive = value;button.interactable = value; } }
        private void Start()
        {
            if (hotkey == KeyCode.None)
            {
                IsActive= false;
            }
            else
            {
                IsActive = true;
                button.onClick.AddListener(ButtonListener);
                Command = Utils.GetTypeByName($"Assets.Scripts.Globals.Commands.{commandName}");
                targetType = Utils.GetTypeByName($"Assets.Scripts.Globals.Commands.{targetTypeName}Target");

            }
        }
        private void Update()
        {
            if (isActive && Input.GetKeyDown(hotkey))
            {
                button.Select();
                button.OnSubmit(null);
            }
        }
        private void ButtonListener()
        {
            OnPress.Invoke(Command, targetType);
        }
    }
}