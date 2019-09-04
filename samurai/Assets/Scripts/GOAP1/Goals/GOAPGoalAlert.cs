using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public class GOAPGoalAlert : GOAPGoalBase
{
    public GOAPGoalAlert(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.ALERT, agent, goalProps)
    {

    }

    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        base.OnEnter(ws);
        Debug.Log("alert");
    }

    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {        
        if (Agent.BlackBoard.HasAttackTarget == false)
        {
            return 0;
        }

        if (ws.Get((int)WorldStatePropType.LOOKING_AT_TARGET))
        {
            return 0;
        }

        if (ws.Get((int)WorldStatePropType.IN_WEAPON_RANGE))
        {
            return 0;
        }

        return Mathf.Min(Agent.BlackBoard.GOAPMaxWeightAlert, Agent.BlackBoard.ForwardAngleToTarget * 0.01f);
    }
}