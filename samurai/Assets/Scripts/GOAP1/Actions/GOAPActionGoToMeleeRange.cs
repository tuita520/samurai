using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionGoToMeleeRange1 : GOAPActionBase
{
    AnimFSMEventGoTo _eventGoTo;
    Vector3 _finalPos;

    public GOAPActionGoToMeleeRange1(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)GOAPActionType1.GOTO_MELEE_RANGE, agent, fsm, WSPrecondition, WSEffect)
    {
        
    }

    public override void Reset()
    {
        base.Reset();
        _eventGoTo = null;
        _finalPos = Vector3.zero;
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
        return _eventGoTo.IsFinished || Agent.BlackBoard.InWeaponRange;
    }

    void SendEvent()
    {
        _finalPos = GetBestAttackStart(Agent.BlackBoard.desiredTarget);
        if (_finalPos == Vector3.zero)
        {
            return;
        }

        _eventGoTo = AnimFSMEventGoTo.pool.Get();
        _eventGoTo.moveType = MoveType.FORWARD;
        _eventGoTo.motionType = MotionType.SPRINT;
        _eventGoTo.finalPosition = _finalPos;
        FSMComponent.SendEvent(_eventGoTo);
    }

    Vector3 GetBestAttackStart(Agent1 target)
    {
        if (target == null)
        {
            return Vector3.zero;
        }
        Vector3 dirToTarget = target.Position - Agent.Position;
        dirToTarget.Normalize();

        return target.Position - dirToTarget * Agent.BlackBoard.weaponRange/* * Random.Range(0.5f, 1)*/;
    }
}