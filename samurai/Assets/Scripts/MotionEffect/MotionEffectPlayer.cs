using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.Particle;

public class MotionEffectPlayer : MotionEffectBase
{
    public override void OnRoll(Agent1 agent)
    {
        agent.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ROLL);
    }
}