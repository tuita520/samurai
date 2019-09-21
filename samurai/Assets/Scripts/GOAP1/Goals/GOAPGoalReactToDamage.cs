using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPGoalReactToDamage1 : GOAPGoalBase
{
    public GOAPGoalReactToDamage1(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.REACT_TO_DAMAGE, agent, goalProps)
    {

    }
    
    //public override bool IsAborted()
    //{
    //    return false;
    //}

    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        if (ws.Get((int)WorldStatePropType.REACT_TO_DAMAGE))
        {
            return 1f;
        }

        return 0;
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {        
        Agent.BlackBoard.damageResultType = DamageResultType.NONE;
        base.OnExit(ws);
    }
}