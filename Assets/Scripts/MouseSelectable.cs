using UnityEngine;

public class MouseSelectable : MonoBehaviour
{

    public GameObject SelectionCirclePrefab;
    private Stats stats;
    private GameObject Circle;
    public void Start()
    {
        stats = GetComponentInParent<Stats>();
    }
    public void Select() 
    {
        Circle = Instantiate(SelectionCirclePrefab, new Vector3(transform.position.x, transform.position.y - stats._h - 0.99f + stats.terrain.SampleHeight(stats.transform.position) + stats.terrain.transform.position.y, transform.position.z), Quaternion.AngleAxis(90, new Vector3(1,0,0)));
        Circle.transform.localScale = stats.transform.localScale;
        Circle.transform.SetParent(transform);
    }
    public void DeSelect() 
    {
        Destroy(Circle);
        Circle = null;
    }
    public void Update()
    {
        if (!(Circle is null))
        {
            Circle.transform.position = new Vector3(transform.position.x, transform.position.y - stats.h - 0.99f + stats.terrain.SampleHeight(stats.transform.position) + stats.terrain.transform.position.y, transform.position.z);
        }
    }
}
