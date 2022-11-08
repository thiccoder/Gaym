using System;
using System.Collections;
using UnityEngine;

namespace GameEngine.Effects
{
    public class Explosive : MonoBehaviour
    {
        public Transform explosionPrefab;
        public float detonationImpactVelocity = 10;
        public float sizeMultiplier = 1;
        public bool reset = true;
        public float resetTimeDelay = 10;

        private bool m_Exploded;
        private ObjectResetter m_ObjectResetter;


       private void Start()
        {
            m_ObjectResetter = GetComponent<ObjectResetter>();
        }


        private IEnumerator OnCollisionEnter(Collision col)
        {
            if (enabled)
            {
                if (col.contacts.Length > 0)
                {
                    float velocityAlongCollisionNormal =
                        Vector3.Project(col.relativeVelocity, col.contacts[0].normal).magnitude;

                    if (velocityAlongCollisionNormal > detonationImpactVelocity || m_Exploded)
                    {
                        if (!m_Exploded)
                        {
                            Instantiate(explosionPrefab, col.contacts[0].point,
                                        Quaternion.LookRotation(col.contacts[0].normal));
                            m_Exploded = true;

                            SendMessage("Immobilize");

                            if (reset)
                            {
                                m_ObjectResetter.DelayedReset(resetTimeDelay);
                            }
                        }
                    }
                }
            }

            yield return null;
        }


        public void Reset()
        {
            m_Exploded = false;
        }
    }
}
