using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPGoalOrderDodge : GOAPGoalBase
{
    public GOAPGoalOrderDodge(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.ORDER_DODGE, agent, goalProps)
    {

    }

    //public override bool IsAborted()
    //{
    //    return false;
    //}

    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        if (ws.Get((int)WorldStatePropType.ORDER_ROLL))
        {
            return Agent.BlackBoard.GOAPMaxWeightOrderDodge;
        }
        else
        {
            OrderDataRoll order = Agent.PlayerOrder.GetCurOrder() as OrderDataRoll;
            if (order != null)
            {
                ws.Set((int)WorldStatePropType.ORDER_ROLL, true);
                return Agent.BlackBoard.GOAPMaxWeightOrderDodge;
            }
        }

        return 0;
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        base.OnExit(ws);
        PlayerOrderPool.rolls.Collect(Agent.PlayerOrder.Pop() as OrderDataRoll);
    }
}