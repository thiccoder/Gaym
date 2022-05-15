using Globals;
using UnityEngine;
using System.Collections.Generic;

public class Move : Order
{
    private RTSTerrain rtsTerrain;
    private float time = 0;
    private List<Vector3> Path;
    private int pathIndex;
    [HideInInspector]
    public float PathLength;
    [HideInInspector]
    public Terrain terrain;
    public float MoveSpeed;
    public float TurnSpeed;
    public TerrainType MoveType = TerrainType.Walkable;
    public void Start()
    {
        IsObjectTargeted = false;
        terrain = Terrain.activeTerrain;
        rtsTerrain = terrain.GetComponent<RTSTerrain>();
        time = 0;
        Path = new();
        pathIndex = 0;
    }
    public override void Invoke(OrderTarget target)
    {
        Path = rtsTerrain.FindPath(transform.position, target.Position, MoveType);
        time = 0;
        string s = "playfieldpath=";
        foreach (var point in Path)
        {
            s += point.ToString() + ' ';
        }
        Debug.Log(s);
        pathIndex = 0;
        PathLength = Utils.GetPathLength(Path);
        completed = false;
    }
    public override void Abort()
    {
        Path = new();
        completed = true;
    }
    public void Update()
    {
        if (pathIndex >= Path.Count)
        {
            completed = true;
        }
        if (!completed)
        {
            transform.position = Utils.Bezier(time * MoveSpeed / PathLength, Path);
            if (transform.position == Path[pathIndex])
            {
                pathIndex++;
            }
        }
        time += Time.deltaTime;
    }
}