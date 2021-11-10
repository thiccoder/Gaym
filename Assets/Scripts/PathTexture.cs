using Globals;
using UnityEngine;

public struct PathTexture
{
    public bool Active;
    public readonly Vector2Int Resolution;
    private TerrainType[,] data;
    public PathTexture(TerrainType[,] data)
    {
        Resolution = new Vector2Int(data.GetLength(0), data.GetLength(1));
        this.data = data;
        Active = true;
    }
    public PathTexture(Vector2Int resolution)
    {
        Resolution = resolution;
        data = new TerrainType[resolution.x, resolution.y];
        Active = true;
    }
    public TerrainType this[int x,int y] { get { return data[x, y]; } set { data[x, y] = value; } }
}
