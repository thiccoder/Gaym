using Globals;
using UnityEngine;
using System.Collections.Generic;
using System;

public class RTSTerrain : MonoBehaviour
{
    private PathFinder finder;
    public Vector2 Scale = new(1.0f, 1.0f);
    private Terrain terrain;
    public TextureOverlapper<PathTexture> Overlapper
    {
        get
        { 
            //Debug.Log("Finder: "+finder.ToString());
            //Debug.Log("Overlapper: "+(finder.Overlapper).ToString());
            return finder.Overlapper;
        }
    }
    public Vector2Int WorldToGridPosition(Vector3 pos)
    {
        pos -= transform.position;
        return new Vector2Int((int)Mathf.Clamp(pos.x * Scale.x, 0, Overlapper.MainTexture.Resolution.x), (int)Mathf.Clamp(pos.z * Scale.y, 0, Overlapper.MainTexture.Resolution.y));
    }
    public Vector3 GridToWorldPosition(Vector2Int pos)
    {
        return new Vector3(pos.x / Scale.x, 0, pos.y / Scale.y) + transform.position;
    }
    public void Awake()
    {
        terrain = GetComponent<Terrain>();
        var resolution = new Vector2Int((int)(terrain.terrainData.size.x * Scale.x), (int)(terrain.terrainData.size.y * Scale.y));
        PathTexture ptx = new(resolution);
        for (int x = 0; x < resolution.x; x++)
        {
            for (int y = 0; y < resolution.y; y++)
            {
                var steepness = terrain.terrainData.GetSteepness(x*Scale.x / resolution.x, y*Scale.y / resolution.y)/90;
                ptx[x, y] = (steepness < 0.25) ? TerrainType.All : TerrainType.None;
            }
        }
        finder = new PathFinder(ptx);
    }
    public List<Vector3> FindPath(Vector3 from, Vector3 to, TerrainType type, List<int> ignoreIndexes = null)
    {
        from -= transform.position;
        to -= transform.position;
        var gridFrom = new Vector2Int((int)Mathf.Clamp(from.x * Scale.x, 0, Overlapper.MainTexture.Resolution.x), (int)Mathf.Clamp(from.z * Scale.y, 0, Overlapper.MainTexture.Resolution.y));
        var gridTo = new Vector2Int((int)Mathf.Clamp(to.x * Scale.x, 0, Overlapper.MainTexture.Resolution.x), (int)Mathf.Clamp(to.z * Scale.y, 0, Overlapper.MainTexture.Resolution.y));
        var gridPath = finder.FindPath(gridFrom, gridTo, type, ignoreIndexes);
        var path = new List<Vector3>();
        foreach (var gridPos in gridPath)
        {
            var pos = new Vector3(gridPos.x / Scale.x, 0, gridPos.y / Scale.y) + transform.position;
            path.Add(pos);
        }
        path[^1] = from + transform.position;
        path[0] = to + transform.position;
        string s = "gridpath=";
        foreach (var point in gridPath)
        {
            s += point.ToString() + ' ';
        }
        Debug.Log(s);
        return path;
    }
}
