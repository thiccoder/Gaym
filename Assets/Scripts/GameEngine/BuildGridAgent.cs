using UnityEngine;
using System;

public class BuildGridAgent : MonoBehaviour
{
    public Vector2Int Size = Vector2Int.one;
    public GameObject GridSelectorPrefab;
    public GameObject GridSelector;
    public bool Finished = false;
    private void Start()
    {
        GridSelector = Instantiate(GridSelectorPrefab, transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(90, 0, 0));
        GridSelector.transform.parent= transform;
        GridSelector.transform.localScale = new Vector3(Size.x, Size.y,1);
        GridSelector.GetComponent<Renderer>().material.SetInt("_GridsizeX", Size.x);
        GridSelector.GetComponent<Renderer>().material.SetInt("_GridsizeY", Size.y);
        //GridSelector.SetActive(false);
    }
}
