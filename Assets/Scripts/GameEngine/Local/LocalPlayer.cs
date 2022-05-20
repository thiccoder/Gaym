using UnityEngine;
using System.Collections.Generic;
using Globals.Orders;
using TMPro;
namespace GameEngine
{
    public class LocalPlayer : MonoBehaviour
    {
        private TerrainCollider Collider;
        private Vector3 MousePos;
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
            RaycastHit hitData;
            bool rayHit;
            if (OnlyTerrain)
            {
                Collider = Terrain.activeTerrain.GetComponent<TerrainCollider>();
                rayHit = Collider.Raycast(ray, out hitData, 1000);
            }
            else
            {
                rayHit = Physics.Raycast(ray, out hitData, 1000);
            }
            if (rayHit)
            {
                MousePos = hitData.point;
                MouseObject = hitData.transform.gameObject;
            }
        }
        public Vector3 GetPos()
        {
            Cast(true);
            return MousePos;
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
                if (Input.GetMouseButtonDown(0))
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
                if (Input.GetMouseButtonUp(0))
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
            if (Input.GetMouseButtonDown(1))
            {
                foreach (GameObject obj in GetSelected())
                {
                    if (!(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)))
                    {
                        obj.GetComponentInParent<OrderQueue>().Clear();
                    }
                    obj.GetComponentInParent<OrderQueue>().IssueImmediate(new StoredOrder(typeof(Move), new OrderTarget(GetPos())));

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