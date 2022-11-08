using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.GameEngine.Locals
{
    public class HeavyVehiclesMoving : MonoBehaviour
    {
        private Vector3 lastDir;
        private Vector3 curDir;
        private float curAngularVelocity;
        private Vector3 lastPos;
        private bool isRotating;
        private Widget unit;
        private NavMeshAgent agent;
        // Start is called before the first frame update
        void Start()
        {
            unit = GetComponent<Widget>();
            agent = GetComponent<NavMeshAgent>();
            lastPos = unit.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            /*curDir = unit.transform.forward;
            curAngularVelocity = Vector2.Angle(curDir, lastDir) / Time.deltaTime;
            lastDir = curDir;
            isRotating = curAngularVelocity > 0.1f;
            if (isRotating)
            {
                agent.isStopped = true;
                //unit.transform.position = lastPos;
                Debug.Log("Rotating");
                //lastPos = unit.transform.position; 
            } */
            bool isRotating = (Vector2.Angle(unit.transform.position, agent.nextPosition) / Time.deltaTime) > 1f;
            Debug.Log(isRotating);
            if (isRotating)
            {
                agent.isStopped = true;
                Quaternion rotation = Quaternion.LookRotation(agent.nextPosition - unit.transform.position);
                unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, rotation, Time.deltaTime * agent.angularSpeed);
            } else
            {
                agent.isStopped = false;
            }
            
        }
    }
}
