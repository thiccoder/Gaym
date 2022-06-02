using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.GameEngine
{
    public static class Utils
    {
        private static Texture2D _whiteTexture;
        private static LayerMask UILayer = LayerMask.NameToLayer("UI");
        public static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }
                return _whiteTexture;
            }
        }
        public static Type GetTypeByName(string name)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
            {
                var tt = assembly.GetType(name);
                if (tt != null)
                {
                    return tt;
                }
            }

            throw new Exception("shit");
        }
        public static Bounds GetViewportBounds(this Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
        {
            var v1 = camera.ScreenToViewportPoint(screenPosition1);
            var v2 = camera.ScreenToViewportPoint(screenPosition2);
            var min = Vector3.Min(v1, v2);
            var max = Vector3.Max(v1, v2);
            min.z = camera.nearClipPlane;
            max.z = camera.farClipPlane;

            var bounds = new Bounds();
            bounds.SetMinMax(min, max);
            return bounds;
        }
        public static Vector2 Displacement(this Vector3 loc, Rect area)
        {
            Vector2 displacement = Vector2.zero;
            if (area.xMin > loc.x)
            {
                displacement.x = -1;
            }
            else if (area.xMax < loc.x)
            {
                displacement.x = 1;
            }
            if (area.yMin > loc.y)
            {
                displacement.y = -1;
            }
            else if (area.yMax < loc.y)
            {
                displacement.y = 1;
            }
            return displacement;
        }
        public static bool IsPointerOverUI()
        {
            PointerEventData eventData = new(EventSystem.current)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> raysastResults = new();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            foreach (RaycastResult result in raysastResults)
            {
                if (result.gameObject.layer == UILayer)
                    return true;
            }
            return false;
        }
        public static void SetSize(this RectTransform transform, Vector2 newSize)
        {
            Vector2 oldSize = transform.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            transform.offsetMin -= new Vector2(deltaSize.x * transform.pivot.x, deltaSize.y * transform.pivot.y);
            transform.offsetMax += new Vector2(deltaSize.x * (1f - transform.pivot.x), deltaSize.y * (1f - transform.pivot.y));
        }
    }
}