using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;
using Phenix.Unity.Extend;

public class GOAPActionRollForBackStrike : GOAPActionOrderRoll
{
    public GOAPActionRollForBackStrike(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base(actionType, agent, WSPrecondition, WSEffect)
    {

    }

    protected override Vector3 GetRollDir()
    {
        Vector3 targetPos = Vector3.zero;
        bool found = false;
        if (Agent.BlackBoard.desiredTarget != null)
        {
            targetPos = Agent.BlackBoard.desiredTarget.Position - Agent.Position;
            targetPos = Agent.Position + (targetPos.normalized * Agent.BlackBoard.rollDistance);
            for (int degree = 20; degree < 40; ++degree)
            {
                targetPos = targetPos.Rotate(Vector3.up, degree);
                if (CheckTargetPos(targetPos))
                {
                    found = true;
                    break;
                }
                targetPos = targetPos.Rotate(Vector3.up, -degree);
                if (CheckTargetPos(targetPos))
                {
                    found = true;
                    break;
                }
            }

        }

        if (found == false)
        {
            return Vector3.zero;
        }
        
        return (targetPos - Agent.Position).normalized;
    }

    bool CheckTargetPos(Vector3 targetPos)
    {
        return Agent.Decision.AgentInTheWay(targetPos) == false;        
    }
}