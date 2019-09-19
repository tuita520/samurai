using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public class GOAPActionOrderRoll : GOAPActionBase
{
    AnimFSMEventRoll _eventRoll;
    Vector3 _rollDir = Vector3.zero;

    public GOAPActionOrderRoll(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {

    }


    public override void Reset()
    {
        base.Reset();
        _eventRoll = null;
    }

    public override void OnEnter()
    {
        _rollDir = GetRollDir();
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventRoll != null)
        {
            _eventRoll.Release();
            _eventRoll = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        return _eventRoll != null && _eventRoll.IsFinished;
    }

    void SendEvent()
    {
        if (_rollDir == Vector3.zero)
        {
            return;
        }
        _eventRoll = AnimFSMEventRoll.pool.Get();
        _eventRoll.direction = _rollDir;
        Agent.FSMComponent.SendEvent(_eventRoll);
    }

    protected virtual Vector3 GetRollDir()
    {
        return Agent.BlackBoard.desiredDirection;
    }

    public override bool IsAborted()
    {
        return _rollDir == Vector3.zero;
    }
}