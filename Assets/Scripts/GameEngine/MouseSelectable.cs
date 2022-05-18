using UnityEngine;

namespace GameEngine
{
    public class MouseSelectable : MonoBehaviour
    {

        public GameObject SelectionCirclePrefab;
        private Widget unit;
        private GameObject Circle;
        public void Start()
        {
            unit = GetComponentInParent<Widget>();
        }
        public void Select()
        {
            Circle = Instantiate(SelectionCirclePrefab, new Vector3(0,1,0), Quaternion.AngleAxis(90, new Vector3(1, 0, 0)),transform);
            Circle.transform.localPosition = new Vector3(0,0,0);   
            Circle.transform.localScale = new Vector3(unit.Size,1,unit.Size);
        }
        public void DeSelect()
        {
            Destroy(Circle);
            Circle = null;
        }

    }
}