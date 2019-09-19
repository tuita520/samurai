using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

/// <summary>
/// 在持续时间内，朝目标移动直到combat距离
/// </summary>
public class GOAPActionChase : GOAPActionGoToTarget
{
    public GOAPActionChase(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base(actionType, agent, WSPrecondition, WSEffect)
    {
        range = Agent.BlackBoard.combatRange;
    }
}