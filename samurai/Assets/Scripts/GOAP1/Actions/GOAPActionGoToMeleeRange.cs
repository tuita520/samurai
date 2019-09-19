using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

/// <summary>
/// 朝目标移动直到武器距离
/// </summary>
public class GOAPActionGoToMeleeRange1 : GOAPActionBase
{
    AnimFSMEventGoTo _eventGoTo;
    
    protected float range;

    public GOAPActionGoToMeleeRange1(GOAPActionType1 actionType, Agent1 agent, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {
        range = Agent.BlackBoard.weaponRange;
    }

    public override void Reset()
    {
        base.Reset();
        _eventGoTo = null;        
    }

    public override void OnEnter()
    {
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventGoTo != null)
        {
            _eventGoTo.Release();
            _eventGoTo = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        if (_eventGoTo == null)
        {
            return true;
        }
        return _eventGoTo.IsFinished || Agent.BlackBoard.DesiredTargetInRange(range);
    }

    protected void SendEvent()
    {
        _eventGoTo = AnimFSMEventGoTo.pool.Get();
        _eventGoTo.moveType = MoveType.FORWARD;
        _eventGoTo.motionType = MotionType.SPRINT;
        _eventGoTo.finalPosition = GetDestination(Agent.BlackBoard.desiredTarget);
        Agent.FSMComponent.SendEvent(_eventGoTo);
    }

    Vector3 GetDestination(Agent1 target)
    {
        if (target == null)
        {
            return Vector3.zero;
        }
        Vector3 dirToTarget = target.Position - Agent.Position;
        dirToTarget.Normalize();

        return target.Position - dirToTarget * range;
    }
}