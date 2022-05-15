using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Globals;
public class PathFinder
{
    public TextureOverlapper<PathTexture> Overlapper;
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
    public List<Vector2Int> FindPath(Vector2Int from, Vector2Int to, TerrainType moveType = TerrainType.All, List<int> ignoreIndexes = null)
    {
        var overlapperClone = Overlapper;
        if (ignoreIndexes is not null)
        {
            foreach (int index in ignoreIndexes)
            {
                var ptx = overlapperClone.Textures[index];
                ptx.Active = false;
                overlapperClone.Textures[index] = ptx;
            }
        }
        var resolution = overlapperClone.MainTexture.Resolution;
        Node nullNode = new(new(-1, -1), 0, 0);
        Dictionary<Node, Node> parents = new();
        HashSet<Vector2Int> visited = new();
        HashSet<Node> active = new()
        {
            new Node(from, 0, Mathf.Abs((to - from).y) + Mathf.Abs((to - from).y))
        };
        for (int i = 0; i < resolution.x; i++)
        {
            for (int j = 0; j < resolution.y; j++)
            {
                if (!moveType.HasFlag((TerrainType)overlapperClone[i, j]))
                {
                    visited.Add(new Vector2Int(i, j));
                }
            }
        }
        while (active.Any())
        {
            float lowest = active.Min(a => a.EstimatedTotalCost);
            Node current = active.First(a => a.EstimatedTotalCost == lowest);

            active.Remove(current);
            visited.Add(current.Position);

            if (current.Position == to)
            {
                List<Vector2Int> res = new();
                while (parents.ContainsKey(current))
                {
                    res.Add(current.Position);
                    current = parents[current];
                }
                res.Add(from);
                return res;
            }
            foreach (Vector2Int position in neighborsTemplate)
            {
                Vector2Int nodePosition = position + current.Position;
                if (nodePosition.x >= resolution.x || nodePosition.x < 0 || nodePosition.y >= resolution.y || nodePosition.y < 0 || visited.Contains(nodePosition))
                {
                    continue;
                }
                Vector2Int delta = to - nodePosition;
                float traverseDistance = current.TraverseDistance + 1;
                float heuristicDistance = traverseDistance + Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y);
                var neighbor = new Node(nodePosition, traverseDistance, heuristicDistance);
                parents[neighbor] = current;
                if (!active.Contains(neighbor))
                {
                    active.Add(neighbor);
                }
                else
                {
                    var node = active.FirstOrDefault(a => a == neighbor);
                    if (neighbor.TraverseDistance < node.TraverseDistance)
                    {
                        active.Remove(node);
                        active.Add(neighbor);
                    }
                }
            }
        }
        return new();
    }
}

