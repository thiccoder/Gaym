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
        Circle = Instantiate(SelectionCirclePrefab, stats.GetSeletionCirclePosition(), Quaternion.AngleAxis(90, new Vector3(1,0,0)));
        Circle.transform.localScale = stats.model.transform.localScale;
        Circle.transform.SetParent(transform);
    }
    public void DeSelect() 
    {
        Destroy(Circle);
        Circle = null;
    }
    public void Update()
    {
        if (Circle is not null)
        {
            Circle.transform.position = stats.GetSeletionCirclePosition();
        }
    }
}
