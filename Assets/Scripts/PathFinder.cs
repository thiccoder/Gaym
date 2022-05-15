using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Globals;
public class PathFinder
{
    public readonly TextureOverlapper<PathTexture> Overlapper;
    private static readonly Vector2Int[] neighborsTemplate = {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),
            new Vector2Int(1, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };

    public PathFinder(Vector2Int resolution)
    {
        Overlapper = new TextureOverlapper<PathTexture>(new PathTexture(resolution));
    }
    public PathFinder(PathTexture texture)
    {
        Overlapper = new TextureOverlapper<PathTexture>(texture);
    }
    public PathFinder(TextureOverlapper<PathTexture> overlapper)
    {
        Overlapper = overlapper;
    }
    public List<Vector2Int> FindPath(Vector2Int from, Vector2Int to, TerrainType moveType = TerrainType.All)
    {
        HashSet<Vector2Int> visited = new();
        HashSet<Node> active = new()
        {
            new Node(from, 0, Mathf.Abs((to - from).y) + Mathf.Abs((to - from).y), null)
        };
        for (int i = 0; i < Overlapper.MainTexture.Resolution.x; i++)
        {
            for (int j = 0; j < Overlapper.MainTexture.Resolution.y; j++)
            {
                if (!moveType.HasFlag((TerrainType)Overlapper[i, j]))
                {
                    visited.Add(new Vector2Int(i, j));
                }
            }
        }
        while (active.Any())
        {
            var lowest = active.Min(a => a.EstimatedTotalCost);
            Node current = active.First(a => a.EstimatedTotalCost == lowest);

            active.Remove(current);
            visited.Add(current.Position);

            if (current.Position.Equals(to))
            {
                var res = new List<Vector2Int>();
                while (current.Parent is not null)
                {
                    res.Add(current.Position);
                    current = current.Parent;
                }
                res.Add(from);
                return res;
            }
            foreach (Vector2Int position in neighborsTemplate)
            {
                Vector2Int nodePosition = position + current.Position;
                if (nodePosition.x >= Overlapper.MainTexture.Resolution.x || nodePosition.x < 0 || nodePosition.y >= Overlapper.MainTexture.Resolution.y || nodePosition.y < 0 || visited.Contains(nodePosition)) 
                {
                    continue;
                }
                Vector2Int delta = to - nodePosition;
                float traverseDistance = current.TraverseDistance + 1;
                float heuristicDistance = traverseDistance + Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y);
                var neighbor = new Node(nodePosition, traverseDistance, heuristicDistance, current);
                var node = active.FirstOrDefault(a => a.Position == nodePosition);
                if (node is null) active.Add(neighbor);
                else if (neighbor.TraverseDistance < node.TraverseDistance)
                {
                    active.Remove(node);
                    active.Add(neighbor);
                }
            }
        }
        return new();
    }
}

