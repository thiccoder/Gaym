using Assets.Scripts.Globals.Commands;
using Assets.Scripts.GameEngine.Effects;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    public class ActionObject : ScriptableObject
    {
        public virtual void OnIssue(Target target, Unit caster)
        {

        }
        public virtual void OnAbort(Unit caster) 
        {
            
        }
        public virtual void OnUpdate(Unit caster)
        {

        }
        public void PlayEffects(Unit caster)
        {
            VisualEffectLinker fxlinker = caster.Transform.GetComponent<VisualEffectLinker>();
            if (fxlinker is not null)
            {
                fxlinker.Play();
            }
        }
        public void StopEffects(Unit caster)
        {
            VisualEffectLinker fxlinker = caster.Transform.GetComponent<VisualEffectLinker>();
            if (fxlinker is not null)
            {
                fxlinker.Stop();
            }
        }
    }
}
