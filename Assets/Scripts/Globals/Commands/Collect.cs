using Assets.Scripts.GameEngine;
using Assets.Scripts.Globals.Abilities;
using Assets.Scripts.Globals.Resourses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net.WebSockets;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Globals.Commands
{
    public class Collect : Command
    {
        [SerializeField]
        private int collectTime = 0;
        private float time;
        [SerializeField]
        protected NavMeshAgent agent;
        [SerializeField]
        protected Mover Mover;
        private string curTargetTag = "resourses";
        private UnitTarget curTarget;
        private UnitTarget targetBase;
        private UnitTarget targetResourses;

        [SerializeField]
        private PlasticResManager ResourseManager;
        public void Start() {}
        public override void Abort()
        {
            Issuing = false;
            Completed = false;
        }
        public override void Issue(Target target)
        {
            Issue(target as UnitTarget);
        }
        public virtual void Issue(UnitTarget TargetResources)
        {
            /*if (TargetResources == new UnitTarget(d)) { }*/
            Issuing = true;
            Completed = false;
            time = 0;

            curTarget = TargetResources;
            curTargetTag = "resourses";
            targetResourses = TargetResources;
            targetBase = FindClosestBase();
            agent.destination = targetResourses.Value.Transform.position;
            /*Debug.Log($"curtarget {curTarget}");
            Debug.Log($"resourses {targetResourses}");
            Debug.Log($"base {targetBase}");*/
            Debug.Log("Caster "+Caster);
            Debug.Log("curTarget "+curTarget.Value.Name);
            Mover.OnIssue(curTarget, Caster);
        }
        public void Update()
        {
            if (Issuing)
            {           
                if (targetResourses != null && targetResourses.Value.Alive && targetBase != null && targetBase.Value.Alive)
                {
                    Mover.OnUpdate(Caster);
                    float distanceToTargetSqr = Vector3.SqrMagnitude(transform.position - agent.destination);
                    if (distanceToTargetSqr < agent.stoppingDistance * agent.stoppingDistance)
                    {
                        if (time >= collectTime)
                        {
                            time = 0;
                        }
                        if (time < float.Epsilon)
                        {
                            Debug.Log($"Collecting?..   {targetResourses.Value.Name}     {targetBase.Value.Name}");
                            Debug.Log("curTargetTag " + curTargetTag);
                            if (curTargetTag == "resourses")
                            {
                                Console.WriteLine($"Collecting {curTarget.Value}");
                                targetResourses.Value.Health -= 10;
                                curTarget = targetBase;
                                curTargetTag = "base";
                                Debug.Log("targetBase.Value.Transform.position " + targetBase.Value.Transform.position);
                                agent.destination = targetBase.Value.Transform.position;
                            } else if (curTargetTag == "base")
                            {
                                ResourseManager.CurResCount += 10;
                                curTarget = targetResourses;
                                curTargetTag = "resourses";
                                agent.destination = targetResourses.Value.Transform.position;
                            }
                        }
                    }
                }
                else
                {
                    Completed = true;
                    Issuing = false;
                }
                time += Time.deltaTime;
            }
        }

        private UnitTarget FindClosestBase()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("door"); // дебаг (надеюсь)
           /* Debug.Log(targets[0]);*/
            float closestTargetDistance = float.MaxValue;
            NavMeshPath path = new NavMeshPath();
            foreach (GameObject target in targets)
            {
                if (target)
                {
                    // var door = target.GetComponentInChildren<Transform>();
                    Transform targetPos = target.GetComponent<Transform>();
                    /*Debug.Log("TargetPos "+targetPos.position);*/
                    bool calculate = NavMesh.CalculatePath(targetResourses.Value.Transform.position, target.transform.position, agent.areaMask, path);
                    Debug.Log("Calculate " + calculate);
                    if (calculate)
                    {
                        float distance = Vector3.Distance(targetPos.position, path.corners[0]);
                        /*Debug.Log("Distance "+distance);*/
                        for (int i = 1; i < path.corners.Length; i++)
                        {
                            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                        }
                        if (distance < closestTargetDistance)
                        {
                            closestTargetDistance = distance;
                            Debug.Log("Closest target distance "+closestTargetDistance);
                            Debug.Log("Bse: ", target.transform);
                            return new UnitTarget((Unit)target.GetComponentInParent<Widget>());
                        }
                    }
                }
            }
            return null;
        }
        public override string ToCommandString()
        {
            return $"Collect {curTarget.Value}";
        }
    }      
}

