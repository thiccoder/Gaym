using UnityEngine;
namespace Assets.Scripts.GameEngine.Locals
{
    internal class CameraController : MonoBehaviour
    {
        public int MouseBorder;
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
                    Distance -= camVelocity.y * speed / 10;
                }
                else
                {
                    Camera.transform.rotation *= Quaternion.AngleAxis(2 * speed * camVelocity.y, Camera.transform.right);
                }
                Camera.transform.localPosition = Camera.transform.forward * -Distance;
            }
            if (camVelocity.x == 0 && camVelocity.z == 0)
            {
                displacement = Input.mousePosition.Displacement(Rect.MinMaxRect(MouseBorder, MouseBorder, Screen.width - MouseBorder, Screen.height - MouseBorder)) * speed;
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