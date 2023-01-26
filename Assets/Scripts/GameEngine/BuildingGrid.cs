using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{

    public Vector2Int GridSize = new(0, 0); // ������ �����
    private bool[,] grid; // �������� ������� �����
    public BuildGridAgent GridBuilding; // �������� �������, �� ������� ����� ������ ������� 
    public Camera MainCamera;

    public bool isBuilding = false;


    public BuildGridAgent[] buildingPrefab; // ����� ����� ������

    private void Awake()
    {
        grid = new bool[GridSize.x, GridSize.y]; //������������� �����
        MainCamera = Camera.main;
    }


    public void StartBuild(BuildGridAgent buildingPrefab) // �������, ��� �� ������������� ������ ��� ���������, ���������� ��� ���
    {
        if (!isBuilding)// ��������, ��������� �� �� ����� ��� ��� 
        {
            GridBuilding = Instantiate(buildingPrefab);
            GridBuilding.transform.position += new Vector3(0, 0.03f, 0);
            isBuilding = true;
        }
    }

    public void Update()
    {
        if (GridBuilding != null) // ���� �� ���������� ������ ��..
        {
            var GroundPlane = new Plane(Vector3.down, Vector3.zero); // �������� ��������� plane, �� �������� ����� ������������ ��� ������
            Ray raycast = MainCamera.ScreenPointToRay(Input.mousePosition); // ������� ����, �� �������� ����� ��������� ��� ������

            if (GroundPlane.Raycast(raycast, out float position)) // ���� ������� �������� ������ ������� ���������� ������, �� ...
            {
                Vector3 WrldPos = raycast.GetPoint(position); // �������� ������� ��������

                int x = Mathf.RoundToInt(WrldPos.x); //��� �� ����� �������� ��� �� ������ 
                int y = Mathf.RoundToInt(WrldPos.z);

                GridBuilding.transform.position = WrldPos;
                bool available = true;

                if (x <= 0 || x > GridSize.x - GridBuilding.Size.x)
                {
                    available = false;

                }

                if (y <= 0 || y > GridSize.y - GridBuilding.Size.y)
                {
                    available = false;
                }

                if (available == false)
                {
                    GridBuilding.GridSelector.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    GridBuilding.GridSelector.GetComponent<Renderer>().material.color = Color.green;
                }
                if (available && IsBuildable(new RectInt(x, y, GridBuilding.Size.x, GridBuilding.Size.y)))
                {
                    available = false;
                    GridBuilding.GridSelector.GetComponent<Renderer>().material.color = Color.red;
                }
                GridBuilding.transform.position = new Vector3(x, 0, y);
                if (Input.GetMouseButtonUp(0) && available)
                {
                    GridBuilding.GridSelector.GetComponent<Renderer>().material.color = Color.gray;
                    GridBuilding.transform.position += new Vector3(0, -0.03f, 0);
                    PlaceBuildingGrid(new RectInt(x, y, GridBuilding.Size.x, GridBuilding.Size.y));
                }
            }
        }
        DoWithBuild();
    }

    private bool IsBuildable(RectInt area)
    {
        for (int x = area.xMin; x < area.xMax; x++)
        {
            for (int y = area.yMin; y < area.yMax; y++)
            {
                if (grid[x, y])
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void PlaceBuildingGrid(RectInt area)
    {
        for (int x = area.xMin; x < area.xMax; x++)
        {
            for (int y = area.yMin; y < area.yMax; y++)
            {
                grid[x, y] = true;
            }
        }
        Destroy(GridBuilding.GridSelector);
        GridBuilding = null;
        isBuilding = false;

    }

    private void StopBuild()
    {
        Destroy(GridBuilding.gameObject);
        isBuilding = false;
    }

    private void DoWithBuild() // ��� �������� ��� �������� 
    {
        if ((Input.GetKeyDown(KeyCode.Z)) && GridBuilding == null) // ������, �� ������� ���� ����������� ������� StartPlacingBuilding
        {
            StartBuild(buildingPrefab[0]); // ��� ������ �� ������ Z ����� ��������������� ������ ��� �������� 0 � �������.
        }
        if (Input.GetKeyDown(KeyCode.X) && GridBuilding == null) // ������, �� ������� ���� ����������� ������� StartPlacingBuilding
        {
            StartBuild(buildingPrefab[1]); // ��� ������ �� ������ Z ����� ��������������� ������ ��� �������� 0 � �������.   
        }
        if (Input.GetKeyDown(KeyCode.C)) // ������, �� ������� ���� ����������� ������� StartPlacingBuilding
        {
            StopBuild();
        }
    }

}

