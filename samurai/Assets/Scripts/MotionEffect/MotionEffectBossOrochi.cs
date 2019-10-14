using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.Particle;

public class MotionEffectBossOrochi : MotionEffectBase
{
    Coroutine _shakeCoroutine;
    Vector3 _oriPosBeforeShake = Vector3.zero;    

    public override void OnTauntBegin(Agent1 agent)
    {
        _oriPosBeforeShake = Game1.Instance.world.position;
        _shakeCoroutine = TransformTools.Instance.Shake(Game1.Instance.world, Vector3.one, 0.8f, 0, 10f, 0.2f);
        if (agent.particleSystemTauntTust)
        {
            ParticleTools.Instance.Play(agent.particleSystemTauntTust, 0, 0.8f);
        }
    }

    public override void OnTauntEnd(Agent1 agent)
    {
        TransformTools.Instance.StopShake(Game1.Instance.world, _shakeCoroutine, _oriPosBeforeShake);
    }
}