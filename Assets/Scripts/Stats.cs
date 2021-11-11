using UnityEngine;
using Globals;
using System.Collections.Generic;

public class Stats : MonoBehaviour
{
    private GameObject model;
    private float time = 0;
    public float Height;
    public float MoveSpeed;
    public float deltaHeight;
    public float RotateSpeed;
    public TerrainType moveType = TerrainType.Walkable;
    private Vector3[] Path;
    private Vector3 startPosition;
    private int pathIndex;
    [HideInInspector]
    public float _h;
    [HideInInspector]
    public float h = 0;
    [HideInInspector]
    public Terrain terrain;
    [HideInInspector]
    public bool isMoving = false;
    public void Start()
    {
        terrain = Terrain.activeTerrain;
        model = GetComponentInChildren<MeshRenderer>(false).gameObject;
        time = 0;
        startPosition = transform.position;
        Path = new Vector3[0];
        pathIndex = 0;
    }
    public Vector3 Bezier(float t, Vector3[] vectors)
    {
        for (int i = vectors.Length; i > 1; i--)
        {
            Vector3[] nv = new Vector3[i];
            for (int j = 0; j < i - 1; j++)
            {
                nv[j] = Vector3.Slerp(vectors[j + 1], vectors[j], t);
            }
            vectors = nv;
        }
        return vectors[0];
    }

    public void SetPath(Stack<Vector3> path)
    {
        time = 0;
        startPosition = transform.position;
        Path = new Vector3[path.Count + 1];
        Path[0] = startPosition;
        path.ToArray().CopyTo(Path, 1);
        pathIndex = 0;
        isMoving = true;
    }

    public void Update()
    {

        h = terrain.SampleHeight(transform.position) + terrain.transform.position.y;
        if (Height > 0)
            h = Mathf.Lerp(h, Height + h, Time.deltaTime) + Mathf.Sin(Time.time) * (deltaHeight / 2);
        
        if (pathIndex < Path.Length)
        {
            transform.position = Bezier((time * MoveSpeed) / Vector3.Distance(startPosition, Path[Path.Length - 1]), Path);
            if (transform.position == Path[pathIndex])
            {
                pathIndex++;
            }
        }
        else isMoving = false;
        time += Time.deltaTime;
        model.transform.position = transform.position + new Vector3(0, h + 1, 0);
    }
}
