using UnityEngine;
using Phenix.Unity.AI;
using System.Collections.Generic;

public class GOAPGoalPress : GOAPGoalBase
{
    public GOAPGoalPress(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.PRESS, agent, goalProps)
    {

    }
    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        base.OnEnter(ws);
        UnityEngine.Debug.Log("press");
    }
    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        if (Agent.BlackBoard.HasAttackTarget == false)
        {
            return 0;
        }

        // 前面有人
        if (Agent.Decision.GetNearestAgent(Direction.Forward, 0.4f, Game1.Instance.AgentMgr.agents) != null)
            return 0;

        if (ws.Get((int)WorldStatePropType.IN_COMBAT_RANGE))
        {
            return 0;
        }
        
        return Mathf.Min(Agent.BlackBoard.GOAPMaxWeightPress, 
            (Agent.BlackBoard.DistanceToDesiredTarget - Agent.BlackBoard.combatRange) * 0.3f);
    }

}