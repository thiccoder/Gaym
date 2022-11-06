using UnityEngine;
namespace Assets.Scripts.GameEngine.Locals
{
    internal class CameraController : MonoBehaviour
    {
        public float MouseBorder;
        public float Speed;
        public GameObject Camera;
        public float SpdModStrength = 2;
        public float Distance;
        [SerializeField]
        private Texture2D[] cursors;
        private void Start()
        {
            Camera.transform.localPosition = Camera.transform.forward * -Distance;
        }
        private void Update()
        {
            Vector3 camVelocity = Vector3.Normalize(new(Input.GetAxis("CamX"), Input.GetAxis("CamY"), Input.GetAxis("CamZ")));
            Vector2 displacement = Vector2.zero;
            var speed = Speed;
            if (Input.GetButton("SpdMod"))
            {
                speed *= SpdModStrength;
            }
            if (camVelocity.y != 0)
            {
                if (Input.GetButton("ActionMod"))
                {
                    Camera.transform.rotation *= Quaternion.AngleAxis(2 * speed * camVelocity.y, Camera.transform.right);
                }
                else
                {
                    Distance -= camVelocity.y * speed / 10;
                }
                Camera.transform.localPosition = Camera.transform.forward * -Distance;
            }
            if (camVelocity.x == 0 && camVelocity.z == 0)
            {
                if ((Mathf.Abs(Screen.width / 2 - Input.mousePosition.x) <= Screen.width / 2) && (Mathf.Abs(Screen.height / 2 - Input.mousePosition.y) <= Screen.height / 2))
                {
                    displacement = Input.mousePosition.Displacement(Rect.MinMaxRect(Screen.width * MouseBorder, Screen.height * MouseBorder, Screen.width * (1 - MouseBorder), Screen.height * (1 - MouseBorder))) * speed;
                }
            }
            else
            {
                displacement.x = camVelocity.x * speed;
                displacement.y = camVelocity.z * speed;
            }
            transform.Translate(new Vector3(displacement.x, 0, displacement.y) * Time.deltaTime);
        }
    }
}