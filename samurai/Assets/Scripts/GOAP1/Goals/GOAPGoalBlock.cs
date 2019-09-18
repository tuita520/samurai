using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public class GOAPGoalBlock1 : GOAPGoalBase
{
    public GOAPGoalBlock1(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.BLOCK, agent, goalProps)
    {

    }
    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        base.OnEnter(ws);
        UnityEngine.Debug.Log("block");
    }
    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        return 0.985f;
        if (Agent.BlackBoard.HasAttackTarget == false)
        {
            return 0;
        }
        if (Agent.BlackBoard.DesiredTargetInCombatRange == false)
        {
            return 0;
        }
        if (Agent.BlackBoard.LookAtDesiredTarget == false)
        {
            return 0;
        }
        if (Agent.BlackBoard.AheadOfDesiredTarget == false)
        {
            return 0;
        }

        return Mathf.Min(Agent.BlackBoard.GOAPMaxWeightBlock,
            Agent.BlackBoard.GOAPMaxWeightBlock * Agent.BlackBoard.Block * 0.01f);
    }
}