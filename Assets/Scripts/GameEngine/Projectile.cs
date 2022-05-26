using UnityEngine;

namespace Assets.Scripts.GameEngine
{
    public class Projectile : MonoBehaviour
    {
        public float Speed;
        public float Angle;
        private Vector3 initialPosition;
        [HideInInspector]
        public Vector3 TargetPosition;
        private float time = 0;
        private float maxHeight;
        public void Start()
        {
            initialPosition = transform.position;
            time = 0;
            maxHeight = Mathf.Tan(Angle * Mathf.PI / 2) * (transform.position - TargetPosition).magnitude / 2 / Physics.gravity.magnitude;
        }
        void Update()
        {
            if ((transform.position - new Vector3(TargetPosition.x,transform.position.y,TargetPosition.z)).sqrMagnitude > float.Epsilon)
            {
                float x = time / ((TargetPosition - initialPosition).magnitude / Speed);
                var y = 4 * maxHeight * x - 4 * maxHeight * x * x;
                Vector3 newPosition = Vector3.Lerp(initialPosition, TargetPosition, x) + new Vector3(0, y, 0);
                transform.SetPositionAndRotation(newPosition, Quaternion.LookRotation(newPosition - transform.position, Vector3.Cross(newPosition, Vector3.up)));
                time += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
