using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;
/*
public class GOAPActionKnockDown : GOAPActionBase
{
    AnimFSMEventKnockDown _eventKnockDown;

    public GOAPActionKnockDown(Agent1 agent, FSMComponent fsm,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitData> WSEffect)
        : base((int)GOAPActionType1.KNOCK_DOWN, agent, fsm, WSPrecondition, WSEffect)
    {

    }

    public override void OnEnter()
    {
        SendEvent();
    }

    public override void OnExit()
    {
        if (_eventKnockDown != null)
        {
            _eventKnockDown.Release();
            _eventKnockDown = null;
        }
    }

    public override bool IsFinished()
    {
        if (_eventKnockDown == null)
        {
            return true;
        }
        return _eventKnockDown.IsFinished;
    }

    void SendEvent()
    {
        _eventKnockDown = AnimFSMEventKnockDown.pool.Get();        
        _eventKnockDown.fromWeapon = Agent.BlackBoard.attackerWeapon;
        _eventKnockDown.attacker = Agent.BlackBoard.attacker;
        _eventKnockDown.impuls = Agent.BlackBoard.impuls;
        _eventKnockDown.time = Agent.BlackBoard.maxKnockdownTime * Random.Range(0.7f, 1);
        FSMComponent.FSM.SendEvent(_eventKnockDown);
    }
}*/