using Assets.Scripts.Globals.Commands;
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
        public void PlayEffects(Unit caster)
        {
            AttackEffectLinker fxlinker = caster.Transform.GetComponent<AttackEffectLinker>();
            if (fxlinker is not null)
            {
                fxlinker.Play();
            }
        }
        public void StopEffects(Unit caster)
        {
            AttackEffectLinker fxlinker = caster.Transform.GetComponent<AttackEffectLinker>();
            if (fxlinker is not null)
            {
                fxlinker.Stop();
            }
        }
    }
}
