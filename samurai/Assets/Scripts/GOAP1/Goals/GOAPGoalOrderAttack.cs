using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPGoalOrderAttack : GOAPGoalBase
{
    public GOAPGoalOrderAttack(Agent1 agent, List<WorldStateBitDataGoal> goalProps)
        : base((int)GOAPGoalType1.ORDER_ATTACK, agent, goalProps)
    {

    }

    public override bool IsAborted()
    {
        return false;
    }

    public override float GetWeight(Phenix.Unity.AI.WorldState ws)
    {
        if (ws.Get((int)WorldStatePropType.ORDER_ATTACK))
        {
            return Agent.BlackBoard.GOAPMaxWeightOrderAttack;
        }
        else
        {
            OrderDataAttack order = Agent.PlayerOrder.GetCurOrder() as OrderDataAttack;
            if (order != null)
            {
                ws.Set((int)WorldStatePropType.ORDER_ATTACK, true);
                return Agent.BlackBoard.GOAPMaxWeightOrderAttack;
            }
        }

        return 0;
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        base.OnExit(ws);
        PlayerOrderPool.attacks.Collect(Agent.PlayerOrder.Pop() as OrderDataAttack);
    }
}