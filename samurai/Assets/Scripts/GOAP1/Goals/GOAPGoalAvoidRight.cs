using UnityEngine;
using Phenix.Unity.AI;
using System.Collections.Generic;

public class GOAPGoalAvoidRight : GOAPGoalBase
{
    public GOAPGoalAvoidRight(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.AVOID_RIGHT, agent, goalProps)
    {

    }
    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        base.OnEnter(ws);
        UnityEngine.Debug.Log("avoid right");
    }
    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        if (Agent.BlackBoard.HasAttackTarget == false)
        {
            return 0;
        }

        if (Agent.BlackBoard.LookAtTarget && Agent.Decision.GetNearestAgent(Direction.Left,
            0.4f, Game1.Instance.AgentMgr.agents) != null)
        {
            return Agent.BlackBoard.GOAPMaxWeightAvoidRight;
        }

        return 0;
    }

}