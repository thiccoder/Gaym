using Globals;
using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private GameObject model;
    private float accumulator;
    public float Height;
    public float DeltaHeight;
    public Texture2D PathingTexture;
    public float PathUpdatePeriod = 1;
    [HideInInspector]
    public int storedTextureIndex = -1;
    [HideInInspector]
    public float h = 0;
    [HideInInspector]
    public Terrain terrain;
    private RTSTerrain rtsTerrain;
    private void CreatePathTexture()
    {
        StoredTexture<PathTexture> ptx = new(PathTexture.FromTexture2D(PathingTexture), rtsTerrain.WorldToGridPosition(transform.position));
        rtsTerrain.Overlapper.Textures.Add(ptx);
        storedTextureIndex = rtsTerrain.Overlapper.Textures.Count - 1;
    }
    public void Start()
    {
        terrain = Terrain.activeTerrain;
        model = GetComponentInChildren<MeshRenderer>(false).gameObject;
        rtsTerrain = terrain.GetComponent<RTSTerrain>();
        if (storedTextureIndex == -1)
        {
            CreatePathTexture();
        }
        Utils.PaintTerrain(terrain);
    }

    public void Update()
    {
        h = terrain.SampleHeight(transform.position) + terrain.transform.position.y;
        if (Height > 0)
        {
            h = Mathf.Lerp(h, Height + h, Time.deltaTime) + Mathf.Sin(Time.time) * (DeltaHeight / 2);
        }
        model.transform.position = transform.position + new Vector3(0, h + 1, 0);
        accumulator += Time.deltaTime;
        if (accumulator >= PathUpdatePeriod)
        {
            accumulator = 0;
            var ptx = rtsTerrain.Overlapper.Textures[storedTextureIndex];
            if (rtsTerrain.WorldToGridPosition(transform.position) != ptx.Position)
            {
                ptx.Position = rtsTerrain.WorldToGridPosition(transform.position);
                rtsTerrain.Overlapper.Textures[storedTextureIndex] = ptx;
                Debug.Log(ptx.Position.ToString());
            }
        }
    }
}
