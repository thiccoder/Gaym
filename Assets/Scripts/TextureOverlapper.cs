using Globals;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TextureOverlapper<T> where T : Texture
{
    public readonly T MainTexture;
    public List<StoredTexture<T>> Textures;
    private readonly bool isPathTextureOverlapper;
    private readonly bool isVisionTextureOverlapper;
    public TextureOverlapper(T mainTex, List<StoredTexture<T>> texes)
    {
        MainTexture = mainTex;
        Textures = texes;
        isPathTextureOverlapper = (MainTexture is PathTexture);
        isVisionTextureOverlapper = (MainTexture is VisionTexture);
        Debug.Log("ptx: " + isPathTextureOverlapper.ToString());
        Debug.Log("vtx: " + isVisionTextureOverlapper.ToString());
    }
    public TextureOverlapper(T mainTex) : this(mainTex, new()) { }
    public object this[int x, int y]
    {
        get
        {
            if (isPathTextureOverlapper)
            {
                TerrainType val = (MainTexture as PathTexture)[x, y];
                foreach (var storedTex in Textures)
                {
                    if (storedTex.Active)
                    {
                        try
                        {
                            val = (storedTex.Tex as PathTexture)[x - storedTex.Position.x, y - storedTex.Position.y];
                        }
                        catch (IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }
                return val;
            }
            else if (isVisionTextureOverlapper)
            {
                VisionType val = (MainTexture as VisionTexture)[x, y];
                foreach (var storedTex in Textures)
                {
                    if (storedTex.Active)
                    {
                        try
                        {
                            val = (storedTex.Tex as VisionTexture)[x - storedTex.Position.x, y - storedTex.Position.y];
                        }
                        catch (IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }
                return val;
            }
            else 
            {
                return null;
            }
        }
    }
}
