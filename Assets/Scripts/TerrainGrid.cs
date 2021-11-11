using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Globals;
public class TerrainGrid
{
    private readonly PathTexture MainTexture;
    private static readonly Vector2Int[] NeighborsTemplate = {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),
            new Vector2Int(1, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };

    public TerrainGrid(int width, int height)
    {
        MainTexture = new PathTexture(new Vector2Int(width, height));
    }
    public TerrainGrid(PathTexture texture)
    {
        MainTexture = texture;
    }
    public Stack<Vector2Int> FindPath(Vector2Int from, Vector2Int to, TerrainType moveType = TerrainType.All)
    {
        List<Vector2Int> visited = new List<Vector2Int>();
        List<Node> active = new List<Node>()
        {
            new Node(from, 0, Mathf.Abs((to - from).y) + Mathf.Abs((to - from).y), null)
        };
        for (int i = 0; i < MainTexture.Resolution.x; i++)
            for (int j = 0; j < MainTexture.Resolution.y; j++)
            {
                if (!moveType.HasFlag(MainTexture[i, j])) visited.Add(new Vector2Int(i, j));
            }
        while (active.Any())
        {
            var lowest = active.Min(a => a.EstimatedTotalCost);
            Node current = active.First(a => a.EstimatedTotalCost == lowest);

            active.Remove(current);
            visited.Add(current.Position);

            if (current.Position.Equals(to))
            {
                var res = new Stack<Vector2Int>();
                while (current.Parent != null)
                {
                    res.Push(current.Position);
                    current = current.Parent;
                }
                res.Push(from);
                return res;
            }
            foreach (Vector2Int position in NeighborsTemplate)
            {
                Vector2Int nodePosition = position + current.Position;
                nodePosition.x %= MainTexture.Resolution.x;
                nodePosition.y %= MainTexture.Resolution.y;
                if (visited.Contains(nodePosition)) continue;
                Vector2Int delta = to - nodePosition;
                float traverseDistance = current.TraverseDistance + 1;
                float heuristicDistance = traverseDistance + Mathf.Abs(delta.x) + Mathf.Abs(delta.y);
                var neighbor = new Node(nodePosition, traverseDistance, heuristicDistance, current);

                var node = active.FirstOrDefault(a => a.Position == nodePosition);
                if (node == null) active.Add(neighbor);
                else if (neighbor.TraverseDistance < node.TraverseDistance)
                {
                    active.Remove(node);
                    active.Add(neighbor);
                }
            }
        }
        return null;
    }
}

