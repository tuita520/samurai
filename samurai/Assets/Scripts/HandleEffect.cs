using UnityEngine;
using Phenix.Unity.Utilities;

public class HandleEffect
{
    public static void ShowTrail(Agent1 agent, AnimAttackData data, float trailTime)
    {
        if (agent.BlackBoard.showWeaponTrail == false || data.trail == null)
            return;
        
        data.trailParenTrans.position = agent.Transform.position + Vector3.up * 0.15f;
        data.trailParenTrans.rotation = Quaternion.AngleAxis(agent.Transform.rotation.eulerAngles.y, Vector3.up);

        data.trail.SetActive(true);
        MaterialTools.Instance.FadeOut(data.renderer, "_TintColor", trailTime, data.renderer.gameObject);
    }
}