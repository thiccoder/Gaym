using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Globals.Commands;
using TMPro;
using Assets.Scripts.Globals;
using System;
using UnityEngine.UI;
using System.Linq;

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
        [SerializeField]
        private LayerMask units;
        public HashSet<Widget> Selected = new();
        private bool clearSelection = true;
        private bool selectionChanged = false;
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
                issuingCommand = Input.GetButton("Select");
                if (Input.GetButtonDown("Smart"))
                {

                    canSelect = false;
                    currentCommandType = typeof(Smart);
                    currentTargetType = typeof(NullTarget);
                    issuingCommand = true;
                }
                if (canSelect)
                {
                    currentCommandType = null;
                }
                if (!inSelection && selectionChanged)
                {
                    List<Type> commands = new();
                    foreach (Widget unit in Selected)
                    {
                        foreach (Command command in unit.GetComponents<Command>())
                        {
                            commands.Add(command.GetType());
                        }
                    }
                    foreach (CommandButton btn in commandButtons)
                    {
                        btn.IsActive = commands.Contains(btn.Command);
                    }
                    selectionChanged = false;
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
                    Selection.sizeDelta = (bottomLeft - topRight) / Selection.lossyScale;
                }
            }
            else if (issuingCommand)
            {
                Vector3 loc = GetLocation();
                GameObject gameObj = GetObject();
                if (currentCommandType == typeof(Smart))
                {
                    if (gameObj != null)
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
                    if (w != null)
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
                            List<Type> commands = new();
                            foreach (Command command in obj.GetComponents<Command>())
                            {
                                commands.Add(command.GetType());
                            }
                            if (commands.Contains(currentCommandType))
                            {
                                CommandQueue commandQueue = obj.GetComponentInParent<CommandQueue>();
                                if (!Input.GetButton("ActionMod"))
                                {
                                    commandQueue.Clear();
                                }
                                if (currentTargetType != typeof(NullTarget) && currentCommandType != typeof(Move))
                                {
                                    Vector3 targetpos;
                                    if (currentTargetType == typeof(LocationTarget))
                                    {
                                        targetpos = loc;
                                        print($"Issuing (automatic) {obj.transform.name} to \"Move\" to {targetpos}");
                                        print($"Issuing {obj.transform.name} to \"{storedCommand.CommandType.Name}\" to {targetpos}");
                                    }
                                    else
                                    {
                                        targetpos = gameObj.transform.position;
                                        print($"Issuing (automatic) {obj.transform.name} to \"Move\" to {gameObj.transform.name}'s position");
                                        print($"Issuing {obj.transform.name} to \"{storedCommand.CommandType.Name}\" {gameObj.transform.name}");
                                    }
                                    commandQueue.Add(AutoMove(obj.transform.position, targetpos, storedCommand.Of(obj.gameObject).Range));
                                }
                                else if (currentCommandType == typeof(Move))
                                {
                                    print($"Issuing {obj.transform.name} to \"Move\" to {loc}");
                                }
                                else 
                                {

                                    print($"Issuing {obj.transform.name} to \"{storedCommand.CommandType.Name}\"");
                                }
                                commandQueue.Add(storedCommand);
                            }
                        }
                    }
                }
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                canSelect = true;
            }
            fpsText.text = Mathf.CeilToInt(1.0f / Time.deltaTime).ToString();
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
            MouseLocation = Vector3.zero;
            MouseObject = null;
            RaycastHit hitInfo;
            bool rayHit;
            if (OnlyTerrain)
            {
                rayHit = Terrain.activeTerrain.GetComponent<TerrainCollider>().Raycast(ray, out hitInfo, 1000);
            }
            else
            {
                rayHit = Physics.Raycast(ray, out hitInfo, 1000, units.value);
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
            selectionChanged = true;
        }
        public void Remove(Widget obj)
        {
            Selected.Remove(obj);
            obj.GetComponentInChildren<MouseSelectable>().DeSelect();
            selectionChanged = true;
        }
        static private StoredCommand AutoMove(Vector3 start, Vector3 end, Vector2 range)
        {
            Vector3 movepos;
            start.y = 0;
            end.y = 0;
            if ((end - start).sqrMagnitude < range.x * range.x)
            {
                movepos = Vector3.Normalize(end - start) * -range.x;
            } 
            else if ((end - start).sqrMagnitude > range.y * range.y)
            {
                movepos = Vector3.Normalize(end - start) * range.y;
            }
            else
            {
                movepos = Vector3.Normalize(end - start) * float.Epsilon;
            }
            movepos.y = 0;
            return new StoredCommand(typeof(Move), new LocationTarget(start+movepos));
        }
    }
}