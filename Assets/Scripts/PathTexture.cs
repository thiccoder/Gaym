using Globals;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct PathTexture : IEquatable<PathTexture>
{
    public bool Active;
    public readonly Vector2Int Resolution;
    private readonly TerrainType[,] data;
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

    public override bool Equals(object obj)
    {
        return obj is PathTexture texture && Equals(texture);
    }

    public bool Equals(PathTexture other)
    {
        return data == other.data;
    }

    public override int GetHashCode()
    {
        return 1768953197 + data.GetHashCode();
    }

    public static bool operator ==(PathTexture left, PathTexture right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PathTexture left, PathTexture right)
    {
        return !(left == right);
    }
}
