using Assets.Scripts.Globals.Commands;
using Assets.Scripts.GameEngine.Effects;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    public class ActionObject : ScriptableObject
    {
        public virtual bool OnIssue(Target target, Unit caster)
        {
            PlayEffects(caster);
            return true;
        }
        public virtual bool OnAbort(Unit caster) 
        {
            StopEffects(caster);
            return true;
        }
        public virtual bool OnUpdate(Unit caster)
        {
            return true;
        }
        public void PlayEffects(Unit caster)
        {
            if (caster.Transform.TryGetComponent<VisualEffectLinker>(out var fxlinker))
            {
                fxlinker.Play();
            }
        }
        public void StopEffects(Unit caster)
        {
            if (caster.Transform.TryGetComponent<VisualEffectLinker>(out var fxlinker))
            {
                fxlinker.Stop();
            }
        }
    }
}
