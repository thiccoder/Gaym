using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if( _whiteTexture == null )
            {
                _whiteTexture = new Texture2D( 1, 1 );
                _whiteTexture.SetPixel( 0, 0, Color.white );
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }
    public static float GetPathLength(List<Vector3> path) 
    {
        var len = 0.0f;
        for (int i = 1; i < path.Count - 1; i++) 
        {
            len += (path[i] - path[i - 1]).magnitude;
        }
        return len;
    }
    public static Vector3 Bezier(float t, List<Vector3> path)
    {
        for (int i = path.Count; i > 1; i--)
        {
            List<Vector3> nv = new(i);
            for (int j = 0; j < i - 1; j++)
            {
                nv.Add(Vector3.Lerp(path[j + 1], path[j], t));
            }
            path = nv;
        }
        return path[0];
    }
    public static Rect GetScreenRect( Vector3 screenPosition1, Vector3 screenPosition2 )
    {
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        var topLeft = Vector3.Min( screenPosition1, screenPosition2 );
        var bottomRight = Vector3.Max( screenPosition1, screenPosition2 );
        return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
    }

    public static Bounds GetViewportBounds( Camera camera, Vector3 screenPosition1, Vector3 screenPosition2 )
    {
        var v1 = camera.ScreenToViewportPoint( screenPosition1 );
        var v2 = camera.ScreenToViewportPoint( screenPosition2 );
        var min = Vector3.Min( v1, v2 );
        var max = Vector3.Max( v1, v2 );
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax( min, max );
        return bounds;
    }

    public static void DrawScreenRect( Rect rect, Color color )
    {
        GUI.color = color;
        GUI.DrawTexture( rect, WhiteTexture );
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder( Rect rect, float thickness, Color color )
    {
        DrawScreenRect( new Rect( rect.xMin, rect.yMin, rect.width, thickness ), color );
        DrawScreenRect( new Rect( rect.xMin, rect.yMin, thickness, rect.height ), color );
        DrawScreenRect( new Rect( rect.xMax - thickness, rect.yMin, thickness, rect.height ), color );
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }
    public static void PaintTerrain(Terrain terrain)
    {
        float[,,] map = new float[terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight, 2];

        for (var x = 0; x < terrain.terrainData.alphamapHeight; x++)
        {
            for (var y = 0; y < terrain.terrainData.alphamapWidth; y++)
            {
                var normY = y * 1.0f / (terrain.terrainData.alphamapWidth - 1);
                var normX = x * 1.0f / (terrain.terrainData.alphamapHeight - 1);

                var angle = terrain.terrainData.GetSteepness(normX, normY);

                var frac = angle/90;
                map[y, x, 0] = frac;
                map[y, x, 1] = 1 - frac;
            }
        }
        terrain.terrainData.SetAlphamaps(0, 0, map);
    }
}
