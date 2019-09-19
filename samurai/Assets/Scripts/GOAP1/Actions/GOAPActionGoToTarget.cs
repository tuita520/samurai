using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

/// <summary>
/// 在持续时间内，朝目标移动直到武器距离
/// </summary>
public class GOAPActionGoToTarget : GOAPActionGoToMeleeRange1
{
    float _giveUpTimer = 0;
    protected float giveUpTime = 5;

    float _aimTimer = 0;
    protected float aimInterval = 0.5f;
    

    public GOAPActionGoToTarget(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base(actionType, agent, WSPrecondition, WSEffect)
    {
        range = Agent.BlackBoard.weaponRange;
    }

    public override void Reset()
    {
        base.Reset();
        _giveUpTimer = 0;
        _aimTimer = 0;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _giveUpTimer = Time.timeSinceLevelLoad + giveUpTime;        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Time.timeSinceLevelLoad > _aimTimer)
        {
            _aimTimer = Time.timeSinceLevelLoad + aimInterval;
            SendEvent();
        }
    }
    public override bool IsAborted()
    {
        return Time.timeSinceLevelLoad > _giveUpTimer;
    }
}