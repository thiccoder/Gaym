using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Globals.Commands;
using TMPro;
using Assets.Scripts.Globals;
using System;

namespace Assets.Scripts.GameEngine.Locals
{
    public class LocalPlayer : MonoBehaviour
    {
        private Vector3 MouseLocation;
        private GameObject MouseObject;
        private Ray ray;
        private bool inSelection = false;
        private Vector3 mousePositionStart;
        private bool canSelect = true;
        private bool issuingCommand;
        private Type currentCommandType;
        private Type currentTargetType;
        [SerializeField]
        private TMP_Text fpsText;
        [SerializeField]
        private Texture2D cursorTexture;
        [SerializeField]
        private List<CommandButton> commandButtons;
        public HashSet<GameObject> Selected = new();
        private void Start()
        {
            foreach (var cmdBtn in commandButtons)
            {
                cmdBtn.OnPress += OnCommandButtonPress;
            }
        }

        private void OnCommandButtonPress(Type commandType, Type targetType)
        {
            canSelect = false;
            currentCommandType = commandType;
            currentTargetType = targetType;
            if (targetType == typeof(NullTarget))
            {
                issuingCommand = true;
            }
            else
            {
                Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.Auto);
            }
            print($"{currentCommandType.Name}");
            print($"{currentTargetType.Name}");
        }

        public void Update()
        {
            print(issuingCommand);
            if (Selected.Count != 0)
            {

                issuingCommand |= Input.GetButtonDown("Select");
                if (canSelect)
                {
                    currentCommandType = null;
                }
                if (Input.GetButton("Smart"))
                {
                    canSelect = false;
                    currentCommandType = typeof(Smart);
                    currentTargetType = typeof(NullTarget);
                    issuingCommand = true;
                }
            }
            if (canSelect)
            {
                if (Input.GetButtonDown("Select"))
                {
                    inSelection = true;
                    mousePositionStart = Input.mousePosition;
                    if (!Input.GetButton("ActionMod"))
                    {
                        foreach (var obj in FindObjectsOfType<MouseSelectable>())
                        {
                            Remove(obj.gameObject);
                        }
                    }
                }
                if (Input.GetButtonUp("Select"))
                {
                    foreach (var obj in FindObjectsOfType<MouseSelectable>())
                    {
                        if (IsWithinSelectionBounds(obj.gameObject))
                        {
                            Add(obj.gameObject);
                        }
                    }
                    inSelection = false;
                }
            }
            else if (issuingCommand)
            {
                Vector3 loc = GetLocation();
                GameObject gameObj = GetObject();
                if (currentCommandType == typeof(Smart))
                {
                    if (gameObj != Terrain.activeTerrain.gameObject)
                    {
                        currentCommandType = typeof(Attack);
                        currentTargetType = typeof(UnitTarget);
                    }
                    else
                    {
                        currentCommandType = typeof(Move);
                        currentTargetType = typeof(LocationTarget);
                    }
                }
                StoredCommand storedCommand = new(currentCommandType, Target.Null);
                if (currentTargetType == typeof(UnitTarget))
                {
                    Widget w = gameObj.GetComponentInParent<Widget>();
                    if (w is not null)
                    {
                        storedCommand = new(currentCommandType, new UnitTarget((Unit)w));
                    }
                }
                else if (currentTargetType == typeof(LocationTarget))
                {
                    storedCommand = new(currentCommandType, new LocationTarget(loc));
                }
                if (storedCommand.CommandType is not null)
                {
                    foreach (GameObject obj in Selected)
                    {
                        print($"Issuing \"{storedCommand.CommandType.Name}\" to {obj.transform.parent.name}");
                        CommandQueue commandQueue = obj.GetComponentInParent<CommandQueue>();
                        if (Input.GetButton("ActionMod"))
                        {
                            commandQueue.Add(storedCommand);
                        }
                        else
                        {
                            commandQueue.IssueImmediate(storedCommand);

                        }
                    }
                }
                issuingCommand = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                canSelect = true;
            }
            int fps = (int)Mathf.Floor(1.0f / Time.deltaTime);
            fpsText.text = fps.ToString();
        }
        private void Cast(bool OnlyTerrain)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool rayHit;
            if (OnlyTerrain)
            {
                rayHit = Terrain.activeTerrain.GetComponent<TerrainCollider>().Raycast(ray, out hitInfo, 1000);
            }
            else
            {
                rayHit = Physics.Raycast(ray, out hitInfo, 1000, LayerMask.GetMask("Default"));
            }
            if (rayHit)
            {
                MouseLocation = hitInfo.point;
                MouseObject = hitInfo.transform.gameObject;
            }
        }
        public Vector3 GetLocation()
        {
            Cast(true);
            return MouseLocation;
        }
        public GameObject GetObject()
        {
            Cast(false);
            return MouseObject;
        }
        public bool IsWithinSelectionBounds(GameObject obj)
        {
            if (!inSelection)
                return false;

            var camera = Camera.main;
            var viewportBounds = Utils.GetViewportBounds(camera, mousePositionStart, Input.mousePosition);
            return viewportBounds.Contains(camera.WorldToViewportPoint(obj.transform.position));
        }
        public void OnGUI()
        {
            if (inSelection)
            {
                var rect = Utils.GetScreenRect(mousePositionStart, Input.mousePosition);
                Utils.DrawScreenRect(rect, new Color(0.25f, 0.5f, 0.25f, 0.25f));
                Utils.DrawScreenRectBcommand(rect, 2, new Color(0.25f, 0.5f, 0.25f));
            }
        }
        public void Add(GameObject obj)
        {
            Selected.Add(obj);
            obj.GetComponent<MouseSelectable>().Select();
        }
        public void Remove(GameObject obj)
        {
            Selected.Remove(obj);
            obj.GetComponent<MouseSelectable>().DeSelect();
        }
    }
}