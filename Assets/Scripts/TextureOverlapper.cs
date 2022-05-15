using Globals;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TextureOverlapper
{
    public readonly VisionTexture MainTexture;
    public List<StoredTexture> Textures;
    public TextureOverlapper(VisionTexture mainTex, List<StoredTexture> texes)
    {
        MainTexture = mainTex;
        Textures = texes;
    }
    public TextureOverlapper(VisionTexture mainTex) : this(mainTex, new()) { }
    public object this[int x, int y]
    {
        get
        {
            VisionType val = MainTexture[x, y];
            foreach (var storedTex in Textures)
            {
                if (storedTex.Active)
                {
                    try
                    {
                        val = storedTex.Tex[x - storedTex.Position.x, y - storedTex.Position.y];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }
                }
            }
            return val;

        }
    }
}
