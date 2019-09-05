using UnityEngine;
using Phenix.Unity.AI;
using System.Collections.Generic;

public class GOAPGoalAttack : GOAPGoalBase
{
    public GOAPGoalAttack(Agent1 agent, List<WorldStateBitDataGoal> goalProps) 
        : base((int)GOAPGoalType1.ATTACK_TARGET, agent, goalProps)
    {

    }

    public override void Reset()
    {
        base.Reset();     
    }
    
    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        if (Agent.BlackBoard.InAttackMotion)
        {
            return Agent.BlackBoard.GOAPMaxWeightAttackTarget;
        }

        if (Agent.BlackBoard.HasAttackTarget == false)
        {
            return 0;
        }

        if (Agent.BlackBoard.InAttackCD)
        {            
            return 0;
        }

        // 前面有其它人挡着
        Agent1 someone = Agent.Decision.GetNearestAgent(Direction.Forward, 0.4f, Game1.Instance.AgentMgr.agents);
        if (someone != null && someone != Agent.BlackBoard.desiredTarget)
        {            
            return 0;
        }
        
        float attackValue = 0;
        if (Agent.BlackBoard.maxRage > 0)
            attackValue = Agent.BlackBoard.Rage / Agent.BlackBoard.maxRage;
        if (Agent.BlackBoard.maxBerserk > 0)
            attackValue = Mathf.Max(attackValue, Agent.BlackBoard.Berserk / Agent.BlackBoard.maxBerserk);
        if (attackValue < 0.25f)
        {            
            return 0;
        }
        
        return Mathf.Min(Agent.BlackBoard.GOAPMaxWeightAttackTarget,
            Agent.BlackBoard.GOAPMaxWeightAttackTarget * attackValue);
    }

    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        UnityEngine.Debug.Log("attack");
        base.OnEnter(ws);        
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        base.OnExit(ws);
        Agent.BlackBoard.nextAttackTimer = Time.timeSinceLevelLoad + Agent.BlackBoard.attackCD;
    }
}