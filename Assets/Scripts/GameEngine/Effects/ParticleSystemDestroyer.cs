using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameEngine.Effects
{
    public class ParticleSystemDestroyer : MonoBehaviour
    {
        public float minDuration = 8;
        public float maxDuration = 10;

        private float m_MaxLifetime;
        private bool m_EarlyStop;


        private IEnumerator Start()
        {
            var systems = GetComponentsInChildren<ParticleSystem>();

            foreach (var system in systems)
            {
                m_MaxLifetime = Mathf.Max(system.main.startLifetime.constant, m_MaxLifetime);
            }


            float stopTime = Time.time + Random.Range(minDuration, maxDuration);

            while (Time.time < stopTime && !m_EarlyStop)
            {
                yield return null;
            }
            Debug.Log("stopping " + name);

            foreach (var system in systems)
            {
                var emission = system.emission;
                emission.enabled = false;
            }
            BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);

            yield return new WaitForSeconds(m_MaxLifetime);

            Destroy(gameObject);
        }


        public void Stop()
        {
            m_EarlyStop = true;
        }
    }
}
