using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
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
