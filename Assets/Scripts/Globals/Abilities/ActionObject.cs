using Assets.Scripts.Globals.Commands;
using Assets.Scripts.GameEngine.Effects;
using UnityEngine;

namespace Assets.Scripts.Globals.Abilities
{
    public class ActionObject : ScriptableObject
    {
        public virtual bool OnIssue(Target target, Unit caster)
        {
            return true;
        }
        public virtual bool OnAbort(Unit caster) 
        {

            return true;
        }
        public virtual bool OnUpdate(Unit caster)
        {

            return true;
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
