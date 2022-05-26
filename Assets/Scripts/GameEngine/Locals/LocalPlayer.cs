using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Globals.Orders;
using TMPro;
using Assets.Scripts.Globals;

namespace Assets.Scripts.GameEngine.Locals
{
    public class LocalPlayer : MonoBehaviour
    {
        private Vector3 MouseLocation;
        private GameObject MouseObject;
        private Ray ray;
        private bool inSelection = false;
        private Vector3 mousePositionStart;
        public static HashSet<GameObject> Selected = new();
        public static bool isSelecting = true;
        public TMP_Text text;
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
            if (isSelecting)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    inSelection = true;
                    mousePositionStart = Input.mousePosition;
                    if (!(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)))
                    {
                        foreach (var obj in FindObjectsOfType<MouseSelectable>())
                        {
                            Remove(obj.gameObject);
                        }
                    }
                }
                if (Input.GetButtonUp("Fire1"))
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
            if (Input.GetButtonDown("Fire2"))
            {
                Vector3 loc = GetLocation();
                GameObject gameObj = GetObject();
                foreach (GameObject obj in GetSelected())
                {
                    OrderQueue orderQueue = obj.GetComponentInParent<OrderQueue>();
                    StoredOrder sto;
                    if (Input.GetKey(KeyCode.Space))
                    {
                        Widget w = gameObj.GetComponentInParent<Widget>();
                        sto = new StoredOrder(new UnitTarget((Unit)w), typeof(Attack));
                    }
                    else
                    {
                        sto = new(new LocationTarget(loc), typeof(Move));
                    }
                    if (!Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift))
                    {
                        orderQueue.IssueImmediate(sto);
                    }
                    else
                    {
                        orderQueue.Add(sto);
                    }
                }
            }
            int fps = (int)Mathf.Floor(1.0f / Time.deltaTime);
            text.text = fps.ToString();
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

        public HashSet<GameObject> GetSelected()
        {
            return Selected;
        }
    }
}