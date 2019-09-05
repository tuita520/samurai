using UnityEngine;
using Phenix.Unity.AI;
using System.Collections.Generic;

public class GOAPGoalAvoidLeft : GOAPGoalBase
{
    public GOAPGoalAvoidLeft(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.AVOID_LEFT, agent, goalProps)
    {

    }
    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        base.OnEnter(ws);
        UnityEngine.Debug.Log("avoid left");
    }
    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        if (Agent.BlackBoard.HasAttackTarget == false)
        {
            return 0;
        }      

        if (Agent.BlackBoard.LookAtDesiredTarget && Agent.Decision.GetNearestAgent(Direction.Right, 
            0.4f, Game1.Instance.AgentMgr.agents) != null)
        {
            return Agent.BlackBoard.GOAPMaxWeightAvoidLeft;
        }

        return 0;
    }

}