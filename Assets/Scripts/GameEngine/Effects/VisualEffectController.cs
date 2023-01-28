using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.GameEngine.Effects
{
    public class VisualEffectController : MonoBehaviour
    {
        [SerializeField]
        private List<ParticleSystem> fx;
        public void Play()
        {
            foreach (ParticleSystem effect in fx)
            {
                effect.Play();
            }
        }
        public void Stop()
        {
            foreach (ParticleSystem effect in fx)
            {
                effect.Stop();
            }
        }
    }
}