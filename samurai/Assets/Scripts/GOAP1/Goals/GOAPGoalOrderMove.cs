using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPGoalOrderMove : GOAPGoalBase
{
    public GOAPGoalOrderMove(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.ORDER_MOVE, agent, goalProps)
    {

    }

    public override bool IsAborted()
    {
        return false;
    }

    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        if (ws.Get((int)WorldStatePropType.ORDER_MOVE))
        {
            return Agent.BlackBoard.GOAPMaxWeightOrderMove;
        }
        else
        {
            OrderDataMove order = Agent.PlayerOrder.GetCurOrder() as OrderDataMove;
            if (order != null)
            {
                ws.Set((int)WorldStatePropType.ORDER_MOVE, true);
                return Agent.BlackBoard.GOAPMaxWeightOrderMove;
            }
        }

        return 0;
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        base.OnExit(ws);
        PlayerOrderPool.moves.Collect(Agent.PlayerOrder.Pop() as OrderDataMove);
    }
}