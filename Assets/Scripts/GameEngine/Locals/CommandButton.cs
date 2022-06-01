
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
        private Type command;
        private Type targetType;
        public event Action<Type, Type> OnPress;
        private void Start()
        {
            button.onClick.AddListener(ButtonListener);
            command = Utils.ByName($"Assets.Scripts.Globals.Commands.{commandName}");
            targetType = Utils.ByName($"Assets.Scripts.Globals.Commands.{targetTypeName}Target");
        }
        private void Update()
        {
            if (Input.GetKeyDown(hotkey))
            {
                button.Select();
                button.OnSubmit(null);
            }
        }
        private void ButtonListener()
        {
            OnPress.Invoke(command, targetType);
        }
    }
}