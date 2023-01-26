using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.GameEngine.Locals
{
    public class MouseSelectable : MonoBehaviour
    {

        public GameObject SelectionCirclePrefab;
        private GameObject Circle = null;
        public void Select()
        {
            if (Circle == null)
            {
                Circle = Instantiate(SelectionCirclePrefab, new Vector3(0, 0, 0), Quaternion.AngleAxis(90, new Vector3(1, 0, 0)), transform.parent);
                Circle.transform.localPosition = new Vector3(0, 0, 0);
                Circle.GetComponent<Projector>().farClipPlane = transform.parent.GetComponent<Widget>().Height + transform.parent.GetComponent<Widget>().DeltaHeight+float.Epsilon;
                Circle.GetComponent<Projector>().orthographicSize = transform.parent.GetComponent<Widget>().Size;
            }
            else
            {
                Circle.SetActive(true);
            }
        }
        public void DeSelect()
        {
            if (Circle != null)
            {
                Circle.SetActive(false);
            }
        }

    }
}