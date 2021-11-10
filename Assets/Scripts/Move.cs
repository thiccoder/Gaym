using Globals;
using UnityEngine;
using System.Collections.Generic;

public class Move : Order
{
    RTSTerrain terrain;
    void Start()
    {
        id = "Omov";
        hotKey = 'M';
        targetType = TargetType.Point;
        stats = GetComponent<Stats>();
        terrain = Terrain.activeTerrain.GetComponent<RTSTerrain>();
    }
    public override void Issue(OrderTarget target)
    {
        var path = terrain.FindPath(transform.position, target.Point, stats.moveType);
        stats.SetPath(path);
    }
    public override void Stop() 
    {
        stats.SetPath(new Stack<Vector3>());
    }
    void Update() 
    {
        completed = !stats.isMoving;
    }
    
}