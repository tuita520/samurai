using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionGoToTarget : GOAPActionBase
{
    AnimFSMEventGoToTarget _eventGoToTarget;
    float _giveUpTimer = 0;
    const float trackTime = 5;

    public GOAPActionGoToTarget(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)GOAPActionType1.GOTO_TARGET, agent, fsm, WSPrecondition, WSEffect)
    {
        
    }

    public override void Reset()
    {
        base.Reset();
        _eventGoToTarget = null;        
    }

    public override void OnEnter()
    {
        _giveUpTimer = Time.timeSinceLevelLoad + trackTime;
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventGoToTarget != null)
        {
            _eventGoToTarget.Release();
            _eventGoToTarget = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        if (_eventGoToTarget == null)
        {
            return true;
        }
        return _eventGoToTarget.IsFinished || Agent.BlackBoard.DesiredTargetInWeaponRange;
    }

    void SendEvent()
    {        
        if (Agent.BlackBoard.HasAttackTarget == false)
        {
            return;
        }

        _eventGoToTarget = AnimFSMEventGoToTarget.pool.Get();
        _eventGoToTarget.moveType = MoveType.FORWARD;
        _eventGoToTarget.motionType = MotionType.SPRINT;
        _eventGoToTarget.target = Agent.BlackBoard.desiredTarget;
        FSMComponent.SendEvent(_eventGoToTarget);
    }

    public override bool IsAborted()
    {
        return Time.timeSinceLevelLoad > _giveUpTimer;
    }
}