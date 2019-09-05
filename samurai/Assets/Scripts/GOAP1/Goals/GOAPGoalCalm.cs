using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPGoalCalm : GOAPGoalBase
{
    public GOAPGoalCalm(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.CALM, agent, goalProps)
    {
        
    }
    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        base.OnEnter(ws);
        UnityEngine.Debug.Log("calm");
    }
    //public override bool IsAborted()
    //{
    //    return false;
    //}

    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        if (ws.Get((int)WorldStatePropType.IN_IDLE) && 
            Agent.BlackBoard.WeaponInHand &&
            (Agent.BlackBoard.HasAttackTarget == false))
        {
            return Agent.BlackBoard.GOAPMaxWeightCalm;
        }
        return 0;
    }
}