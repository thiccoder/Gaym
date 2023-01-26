using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{

    public Vector2Int GridSize = new(0, 0); // размер сетки
    private bool[,] grid; // создание массива сетки
    public BuildGridAgent GridBuilding; // создание объекта, на котором висит скрипт билдинг 
    public Camera MainCamera;

    public bool isBuilding = false;


    public BuildGridAgent[] buildingPrefab; // масив наших зданий

    private void Awake()
    {
        grid = new bool[GridSize.x, GridSize.y]; //инициализация сетки
        MainCamera = Camera.main;
    }


    public void StartBuild(BuildGridAgent buildingPrefab) // функция, где мы устанавливаем объект или проверяем, установили или нет
    {
        if (!isBuilding)// проверка, посатвили ли мы обект или нет 
        {
            GridBuilding = Instantiate(buildingPrefab);
            GridBuilding.transform.position += new Vector3(0, 0.03f, 0);
            isBuilding = true;
        }
    }

    public void Update()
    {
        if (GridBuilding != null) // если мы перемещаем объект то..
        {
            var GroundPlane = new Plane(Vector3.down, Vector3.zero); // создание условного plane, по которому будет перемещаться наш объект
            Ray raycast = MainCamera.ScreenPointToRay(Input.mousePosition); // райкаст мыши, по которому будет следовать наш объект

            if (GroundPlane.Raycast(raycast, out float position)) // если райкаст касается нашего условно созданного плэйна, то ...
            {
                Vector3 WrldPos = raycast.GetPoint(position); // создание позиции райкаста

                int x = Mathf.RoundToInt(WrldPos.x); //что бы обект двигался как по стетке 
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

    private void DoWithBuild() // все операции над зданиями 
    {
        if ((Input.GetKeyDown(KeyCode.Z)) && GridBuilding == null) // кнопки, по которым буде срабатывать функция StartPlacingBuilding
        {
            StartBuild(buildingPrefab[0]); // при нажати на кнопку Z будет устанавливаться здание под индексом 0 в массиве.
        }
        if (Input.GetKeyDown(KeyCode.X) && GridBuilding == null) // кнопки, по которым буде срабатывать функция StartPlacingBuilding
        {
            StartBuild(buildingPrefab[1]); // при нажати на кнопку Z будет устанавливаться здание под индексом 0 в массиве.   
        }
        if (Input.GetKeyDown(KeyCode.C)) // кнопки, по которым буде срабатывать функция StartPlacingBuilding
        {
            StopBuild();
        }
    }

}

