using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public class GOAPGoalShow : GOAPGoalBase
{
    public GOAPGoalShow(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.SHOW, agent, goalProps)
    {

    }
    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        base.OnEnter(ws);
        UnityEngine.Debug.Log("show");
    }
    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        return 0;
        if (Agent.BlackBoard.HasAttackTarget == false)
        {
            return 0;
        }
        if (Agent.BlackBoard.DesiredTargetInWeaponRange)
        {
            return 0;
        }
        if (Agent.BlackBoard.motionType != MotionType.NONE &&
            Agent.BlackBoard.motionType != MotionType.ANIMATION_DRIVE)
        {
            return 0;
        }

        if (ws.Get((int)WorldStatePropType.WILL_PLAY_ANIM))
        {
            return Agent.BlackBoard.GOAPMaxWeightShow;
        }        
        else if (Random.Range(0, 100) < 1)
        {
            ws.Set((int)WorldStatePropType.WILL_PLAY_ANIM, true);
        }

        return 0;
    }
}