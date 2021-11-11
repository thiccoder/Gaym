using Globals;
using UnityEngine;
using System.Collections.Generic;

public class RTSTerrain : MonoBehaviour
{
    [HideInInspector]
    public TerrainGrid grid;
    public Vector2 Scale = new Vector2(1.0f, 1.0f);
    private Vector2Int Resolution;
    PathTexture ptx;
    private Terrain terrain;
    public void Start()
    {
        terrain = GetComponent<Terrain>();
        Resolution = new Vector2Int((int)(terrain.terrainData.size.x*Scale.x), (int)(terrain.terrainData.size.y * Scale.y));
        ptx = new PathTexture(Resolution);
        for (int x = 0; x < Resolution.x; x++)
        {
            for (int y = 0; y < Resolution.y; y++)
            {
                var steepness = terrain.terrainData.GetSteepness(x /Scale.x, y / Scale.y);
                ptx[x, y] = (steepness < 0.75) ? TerrainType.All : TerrainType.None;
            }
        }
        grid = new TerrainGrid(ptx);
    }
    public Stack<Vector3> FindPath(Vector3 from, Vector3 to, TerrainType type)
    {
        from -= transform.position;
        to -= transform.position;
        var gridFrom = new Vector2Int((int)Mathf.Clamp(from.x * Scale.x, 0, Resolution.x), (int)Mathf.Clamp(from.z * Scale.y, 0, Resolution.y));
        var gridTo = new Vector2Int((int)Mathf.Clamp(to.x * Scale.x, 0, Resolution.x), (int)Mathf.Clamp(to.z * Scale.y, 0, Resolution.y));
        var gridPath = grid.FindPath(gridFrom, gridTo, type);
        var path = new Stack<Vector3>();
        foreach (var gridPoint in gridPath.ToArray())
        {
            var point = new Vector3(gridPoint.x / Scale.x, 0, gridPoint.y / Scale.y) + transform.position;
            Debug.Log(gridPoint.ToString()+" "+point.ToString());
            path.Push(point);
        }
        string s = "";
        foreach (var point in gridPath.ToArray())
        {
            s += point.ToString() + ' ';
        }
        Debug.Log(s);
        s = "";
        foreach (var point in path.ToArray())
        {
            s += point.ToString() + ' ';
        }
        Debug.Log(s);
        return path;
    }
}
