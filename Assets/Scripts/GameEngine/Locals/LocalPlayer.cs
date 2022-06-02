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
        private RectTransform selectionTransform;
        public HashSet<GameObject> Selected = new();
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
                    if ((!inSelection) && (Input.GetButton("Select") && (Input.GetAxis("MouseX") != 0 || Input.GetAxis("MouseY") != 0)))
                    {
                        inSelection = true;
                        selectionTransform.gameObject.SetActive(true);
                        var vector3 = Input.mousePosition;
                        mousePositionStart = new Vector2(vector3.x, vector3.y);
                        Cursor.visible = false;
                        if (!Input.GetButton("ActionMod"))
                        {
                            foreach (var obj in FindObjectsOfType<MouseSelectable>())
                            {
                                Remove(obj.gameObject);
                            }
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
                        Cursor.visible = true;
                        inSelection = false;
                        selectionTransform.gameObject.SetActive(false);
                    }
                
                if (inSelection)
                {
                    var vector3 = Input.mousePosition;
                    Vector2 currentMousePos = new(vector3.x, vector3.y);
                    var topRight = Vector2.Min(mousePositionStart, currentMousePos);
                    var bottomLeft = Vector2.Max(mousePositionStart, currentMousePos);
                    selectionTransform.position = new Vector2(topRight.x, bottomLeft.y);
                    selectionTransform.sizeDelta = ((bottomLeft - topRight) / selectionTransform.lossyScale);
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