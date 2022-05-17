using Globals;
using System;
using UnityEngine;
namespace GameEngine
{
    public class Stats : MonoBehaviour
    {
        [HideInInspector]
        public GameObject model;
        public float Height;
        public float DeltaHeight;
        [HideInInspector]
        public float h = 0;
        [HideInInspector]
        public Terrain terrain;
        private Vector3 modelOffset;
        public void Start()
        {
            terrain = Terrain.activeTerrain;
            model = GetComponentInChildren<MeshRenderer>(false).gameObject;
            modelOffset = model.transform.localPosition;
        }

        public void Update()
        {
            h = 0;
            if (Height > 0)
            {
                h = Mathf.Lerp(h, Height + h, Time.deltaTime) + Mathf.Sin(Time.time) * (DeltaHeight / 2);
            }
            model.transform.position = transform.position + modelOffset;
        }
        public Vector3 GetSeletionCirclePosition()
        {
            return transform.position + new Vector3(0, 0.01f, 0);
        }
    }
}
