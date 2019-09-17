using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPGoalIdle : GOAPGoalBase
{
    public GOAPGoalIdle(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.IDLE, agent, goalProps)
    {
        
    }
    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        base.OnEnter(ws);
        UnityEngine.Debug.Log("idle");
    }

    //public override bool IsAborted()
    //{
    //    return false;
    //}

    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {        
        return Agent.BlackBoard.GOAPMaxWeightIdle;
    }
}