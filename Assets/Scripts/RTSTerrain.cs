using Globals;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RTSTerrain : MonoBehaviour
{
    [HideInInspector]
    public TerrainGrid grid;
    public Vector2Int Resolution = new Vector2Int(1000, 1000);
    public Vector2Int TerrainSize = new Vector2Int(1000, 1000);
    PathTexture ptx;
    private Terrain terrain;
    public void Start()
    {
        terrain = GetComponent<Terrain>();
        ptx = new PathTexture(Resolution);
        for (int x = 0; x < Resolution.x; x++)
        {
            for (int y = 0; y < Resolution.y; y++)
            {
                var steepness = terrain.terrainData.GetSteepness(x * TerrainSize.x / Resolution.x, y * TerrainSize.y / Resolution.y);
                ptx[x, y] = (steepness < 0.75) ? TerrainType.All : TerrainType.None;
            }
        }
        grid = new TerrainGrid(ptx);
    }
    public Stack<Vector3> FindPath(Vector3 from, Vector3 to, TerrainType type)
    {
        from -= transform.position;
        to -= transform.position;
        var gridFrom = new Vector2Int((int)Mathf.Clamp(from.x * Resolution.x / TerrainSize.x, 0, Resolution.x), (int)Mathf.Clamp(from.z * Resolution.y / TerrainSize.y, 0, Resolution.y));
        var gridTo = new Vector2Int((int)Mathf.Clamp(to.x * Resolution.x / TerrainSize.x, 0, Resolution.x), (int)Mathf.Clamp(to.z * Resolution.y / TerrainSize.y, 0, Resolution.y));
        var gridPath = grid.FindPath(gridFrom, gridTo, type);
        var path = new Stack<Vector3>();
        foreach (var gridPoint in gridPath.ToArray().Reverse())
        {
            var point = new Vector3(gridPoint.x * TerrainSize.x / Resolution.x, 0, gridPoint.y * TerrainSize.y / Resolution.y) + transform.position;
            path.Push(point);
        }
        //string s = "";
        //foreach (var point in gridPath.ToArray())
        //{
        //    s += point.ToString() + ' ';
        //}
        //Debug.Log(s);
        //s = "";
        //foreach (var point in path.ToArray())
        //{
        //    s += point.ToString() + ' ';
        //}
        //Debug.Log(s);
        return path;
    }
}
