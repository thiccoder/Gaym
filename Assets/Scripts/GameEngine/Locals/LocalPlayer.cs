using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Globals.Commands;
using TMPro;
using Assets.Scripts.Globals;
using System;
using UnityEngine.UI;

namespace Assets.Scripts.GameEngine.Locals
{
    public class LocalPlayer : MonoBehaviour
    {
        private Vector3 MouseLocation;
        private GameObject MouseObject;
        private Ray ray;
        private bool inSelection = false;
        private Vector2 mousePositionStart;
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
        [SerializeField]
        private RectTransform Selection;
        public HashSet<Widget> Selected = new();
        private bool clearSelection = true;
        private void Start()
        {
            foreach (var cmdBtn in commandButtons)
            {
                cmdBtn.OnPress += OnCommandButtonPress;
            }
        }
        public void Update()
        {
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
                if (!Utils.IsPointerOverUI())
                {
                    if ((!inSelection) && Input.GetButtonDown("Select"))
                    {
                        inSelection = true;
                        Selection.gameObject.SetActive(true);
                        var vector3 = Input.mousePosition;
                        mousePositionStart = new Vector2(vector3.x, vector3.y);
                        clearSelection = !Input.GetButton("ActionMod");
                    }
                }
                if (Input.GetButtonUp("Select"))
                {
                    foreach (var obj in FindObjectsOfType<MouseSelectable>())
                    {
                        if (IsWithinSelectionBounds(obj.gameObject))
                        {
                            Add(obj.transform.parent.GetComponent<Widget>());
                        }
                    }
                    inSelection = false;
                    Selection.gameObject.SetActive(false);
                }

                if (inSelection)
                {
                    var vector3 = Input.mousePosition;
                    Vector2 currentMousePos = new(vector3.x, vector3.y);
                    var topRight = Vector2.Min(mousePositionStart, currentMousePos);
                    var bottomLeft = Vector2.Max(mousePositionStart, currentMousePos);
                    Selection.position = new Vector2(topRight.x, bottomLeft.y);
                    Selection.sizeDelta = ((bottomLeft - topRight) / Selection.lossyScale);
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
                    if (!Utils.IsPointerOverUI() || currentTargetType == typeof(NullTarget))
                    {
                        foreach (Widget obj in Selected)
                        {
                            print($"Issuing \"{storedCommand.CommandType.Name}\" to {obj.transform.name}");
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
                }
                issuingCommand = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                canSelect = true;
            }
            int fps = (int)Mathf.Floor(1.0f / Time.deltaTime);
            fpsText.text = fps.ToString();
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
            var viewportBounds = camera.GetViewportBounds(mousePositionStart, Input.mousePosition);
            return viewportBounds.Contains(camera.WorldToViewportPoint(obj.transform.position));
        }
        public void Add(Widget obj)
        {
            if (clearSelection) 
            {
                foreach (Widget gameObj in Selected) 
                {
                    gameObj.GetComponentInChildren<MouseSelectable>().DeSelect();
                }
                Selected.Clear();
                clearSelection = false;
            }
            Selected.Add(obj);
            obj.GetComponentInChildren<MouseSelectable>().Select();
        }
        public void Remove(Widget obj)
        {
            Selected.Remove(obj);
            obj.GetComponentInChildren<MouseSelectable>().DeSelect();
        }
    }
}