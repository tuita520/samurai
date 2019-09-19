using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionIdle : GOAPActionBase
{
    AnimFSMEventIdle _eventIdle;

    public GOAPActionIdle(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();
        _eventIdle = null;
    }

    public override void OnEnter()
    {
        Reset();
        SendIdleEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventIdle != null)
        {
            _eventIdle.Release();
            _eventIdle = null;
        }
        base.OnExit(ws);
    }    

    public override bool IsFinished()
    {
        return _eventIdle != null && _eventIdle.IsFinished;
    }

    void SendIdleEvent()
    {
        _eventIdle = AnimFSMEventIdle.pool.Get();
        Agent.FSMComponent.SendEvent(_eventIdle);
    }
}