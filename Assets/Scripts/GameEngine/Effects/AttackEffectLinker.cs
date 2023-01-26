using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.GameEngine.Effects
{
    public class AttackEffectLinker : MonoBehaviour
    {
        [SerializeField]
        private GameObject EffectPrefab;
        private EffectController fx;
        void Start()
        {
            fx = Instantiate(EffectPrefab).GetComponent<EffectController>();
            fx.transform.parent = transform;
            fx.transform.localPosition = Vector3.zero;
            fx.Stop();
        }
        public void Play()
        {
            fx.Play();
        }
        public void Stop()
        {
            fx.Stop();
        }
    }
}