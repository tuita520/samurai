using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionHideSword : GOAPActionBase
{    
    AnimFSMEventShowHideSword _eventSword;

    float idleBeginTime;
    float hideSwordDelay;

    public GOAPActionHideSword(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();        
        _eventSword = null;
        idleBeginTime = hideSwordDelay = 0;
    }

    public override void OnEnter()
    {
        Reset();        
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventSword != null)
        {
            _eventSword.Release();
            _eventSword = null;
        }
        base.OnExit(ws);
    }

    public override void OnUpdate()
    {
        if (idleBeginTime == 0)
        {
            idleBeginTime = Time.timeSinceLevelLoad;
            hideSwordDelay = Random.Range(2, 5);
        }
        else if (Time.timeSinceLevelLoad > idleBeginTime + hideSwordDelay)
        {
            if (_eventSword == null)
            {
                SendHideSwordEvent();
            }
        }
    }

    public override bool IsFinished()
    {
        return _eventSword != null && _eventSword.IsFinished;
    }

    void SendHideSwordEvent()
    {
        _eventSword = AnimFSMEventShowHideSword.pool.Get();
        _eventSword.show = false;
        Agent.FSMComponent.SendEvent(_eventSword);
    }
}