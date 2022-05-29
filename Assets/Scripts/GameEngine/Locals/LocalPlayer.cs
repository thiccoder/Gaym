using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Globals.Orders;
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
        public HashSet<GameObject> Selected = new();
        public bool canSelect = true;
        private Type currentOrderType;
        private Type currentTargetType;
        [SerializeField]
        private TMP_Text fpsText;
        [SerializeField]
        private Texture2D cursorTexture;

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
        public void Update()
        {
            bool issuingOrder = false;
            if (Selected.Count != 0)
            {

                issuingOrder = Input.GetButtonDown("Select");
                if (canSelect)
                {
                    currentOrderType = null;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    Cursor.SetCursor(cursorTexture, cursorTexture.texelSize / 2, CursorMode.Auto);
                    canSelect = false;
                    currentOrderType = typeof(Attack);
                    currentTargetType = typeof(UnitTarget);
                }
                else if (Input.GetKey(KeyCode.M))
                {
                    Cursor.SetCursor(cursorTexture, cursorTexture.texelSize / 2, CursorMode.Auto);
                    canSelect = false;
                    currentOrderType = typeof(Move);
                    currentTargetType = typeof(LocationTarget);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    canSelect = false;
                    currentOrderType = typeof(Stop);
                    currentTargetType = typeof(NullTarget);
                    issuingOrder = true;
                }
                else if (Input.GetButton("Smart"))
                {
                    canSelect = false;
                    currentOrderType = typeof(Smart);
                    currentTargetType = typeof(NullTarget);
                    issuingOrder = true;
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
            else if (issuingOrder)
            {
                Vector3 loc = GetLocation();
                GameObject gameObj = GetObject();
                if (currentOrderType == typeof(Smart))
                {
                    if (gameObj != Terrain.activeTerrain.gameObject)
                    {
                        currentOrderType = typeof(Attack);
                        currentTargetType = typeof(UnitTarget);
                    }
                    else
                    {
                        currentOrderType = typeof(Move);
                        currentTargetType = typeof(LocationTarget);
                    }
                }
                StoredOrder sto = new(currentOrderType, Target.Null);
                if (currentTargetType == typeof(UnitTarget))
                {
                    Widget w = gameObj.GetComponentInParent<Widget>();
                    if (w is not null)
                    {
                        sto = new(currentOrderType, new UnitTarget((Unit)w));
                    }
                }
                else if (currentTargetType == typeof(LocationTarget))
                {
                    sto = new(currentOrderType, new LocationTarget(loc));
                }
                if (sto.OrderType is not null)
                {
                    foreach (GameObject obj in Selected)
                    {
                        print($"Issuing \"{sto.OrderType.Name}\" to {obj.transform.parent.name}");
                        OrderQueue orderQueue = obj.GetComponentInParent<OrderQueue>();
                        if (Input.GetButton("ActionMod"))
                        {
                            orderQueue.Add(sto);
                        }
                        else
                        {
                            orderQueue.IssueImmediate(sto);

                        }
                    }

                }
                canSelect = true;
            }
            int fps = (int)Mathf.Floor(1.0f / Time.deltaTime);
            fpsText.text = fps.ToString();
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
                Utils.DrawScreenRectBorder(rect, 2, new Color(0.25f, 0.5f, 0.25f));
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