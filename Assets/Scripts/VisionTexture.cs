using Globals;
using System;
using UnityEngine;
public class VisionTexture : Texture
{

    private readonly VisionType[,] data;
    public VisionTexture() 
    {
    
    }
    public VisionTexture(VisionType[,] data)
    {
        Resolution = new Vector2Int(data.GetLength(0), data.GetLength(1));
        this.data = data;
    }
    public VisionTexture(Vector2Int resolution)
    {
        Resolution = resolution;
        data = new VisionType[resolution.x, resolution.y];
    }
    public VisionType this[int x, int y] { get { return data[x, y]; } set { data[x, y] = value; } }

    public override bool Equals(object obj)
    {
        return obj is VisionTexture texture && Equals(texture);
    }
    public override bool Equals(Texture other)
    {
        return other is VisionTexture texture && Equals(texture);
    }

    public bool Equals(VisionTexture other)
    {
        return data == other.data;
    }

    public override int GetHashCode()
    {
        return 1768953197 + data.GetHashCode();
    }

    public static bool operator ==(VisionTexture left, VisionTexture right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(VisionTexture left, VisionTexture right)
    {
        return !(left == right);
    }
    public static VisionTexture FromTexture2D(Texture2D texture)
    {
        var vtx = new VisionTexture(new Vector2Int(texture.width,texture.height));
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                vtx[x, y] = (VisionType)(int)(texture.GetPixel(x, y).grayscale * 2);
            }
        }
        return vtx;
    }
}
