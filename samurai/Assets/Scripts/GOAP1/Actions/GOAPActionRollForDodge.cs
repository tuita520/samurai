using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;
using Phenix.Unity.Extend;

public class GOAPActionRollForDodge : GOAPActionOrderRoll
{
    public GOAPActionRollForDodge(GOAPActionType1 actionType, Agent1 agent,
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
            targetPos = Agent.Position - Agent.BlackBoard.desiredTarget.Position;
            targetPos = Agent.Position + (targetPos.normalized * Agent.BlackBoard.rollDistance);
            // 尝试N次
            for (int i = 0; i < 10; ++i)
            {
                // 随机角度
                int degree = Random.Range(0, 2) == 0 ? 1 : -1;
                degree *= Random.Range(0, 60);
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