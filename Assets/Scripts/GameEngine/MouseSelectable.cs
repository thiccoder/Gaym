using UnityEngine;

namespace Assets.Scripts.GameEngine
{
    public class MouseSelectable : MonoBehaviour
    {

        public GameObject SelectionCirclePrefab;
        private GameObject Circle = null;
        public void Select()
        {
            if (Circle is null)
            {
                Circle = Instantiate(SelectionCirclePrefab, new Vector3(0, 0, 0), Quaternion.AngleAxis(90, new Vector3(1, 0, 0)), transform);
                Circle.transform.localPosition = new Vector3(0, 0, 0);
            }
            else
            {
                Circle.SetActive(true);
            }
        }
        public void DeSelect()
        {
            if (Circle is not null)
            {
                Circle.SetActive(false);
            }
        }

    }
}