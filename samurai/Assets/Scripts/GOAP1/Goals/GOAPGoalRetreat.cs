﻿using UnityEngine;
using Phenix.Unity.AI;
using System.Collections.Generic;

public class GOAPGoalRetreat : GOAPGoalBase
{
    public GOAPGoalRetreat(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.RETREAT, agent, goalProps)
    {

    }
    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        base.OnEnter(ws);
        UnityEngine.Debug.Log("retreat");
    }
    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        if (InProgress && Agent.BlackBoard.InRollMotion)
        {
            return Agent.BlackBoard.GOAPMaxWeightRetreat;
        }
        if (Agent.BlackBoard.HasAttackTarget == false)
        {
            return 0;
        }
        
        //if (Agent.BlackBoard.Fear < Agent.BlackBoard.maxFear * 0.25f)
        //    return 0;

        // 后面有人
        if (Agent.Decision.GetNearestAgent(Direction.Backward, 0.4f, Game1.Instance.AgentMgr.agents) != null)
            return 0;

        if (ws.Get((int)WorldStatePropType.IN_COMBAT_RANGE) == false)
        {
            return 0;
        }

        if (Agent.BlackBoard.InAttackCD)
        {
            return Mathf.Min(Agent.BlackBoard.GOAPMaxWeightRetreat * Agent.BlackBoard.desiredTarget.BlackBoard.weaponRange
                / Agent.BlackBoard.DistanceToDesiredTarget, Agent.BlackBoard.GOAPMaxWeightRetreat);
        }
        
        float ret = Agent.BlackBoard.GOAPMaxWeightRetreat * (Agent.BlackBoard.Fear * 0.01f);
        if (Agent.BlackBoard.DistanceToDesiredTarget <= Agent.BlackBoard.desiredTarget.BlackBoard.weaponRange)
            ret += 0.2f;        
        if (Agent.BlackBoard.AheadOfDesiredTarget == false)
            ret *= 0.5f;
        if (ret > Agent.BlackBoard.GOAPMaxWeightRetreat)
            ret = Agent.BlackBoard.GOAPMaxWeightRetreat;
        return ret;
    }

    public override float GetCD()
    {
        return 1;
    }
}