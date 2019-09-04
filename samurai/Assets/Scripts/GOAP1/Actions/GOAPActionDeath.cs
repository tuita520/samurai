using System.Collections.Generic;
using Phenix.Unity.AI;
/*
public class GOAPActionDeath1 : GOAPActionBase
{
    AnimFSMEventDeath _eventDeath;

    public GOAPActionDeath1(Agent1 agent, FSMComponent fsm,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitData> WSEffect)
        : base((int)GOAPActionType1.DEATH, agent, fsm, WSPrecondition, WSEffect)
    {

    }

    public override void OnEnter()
    {
        SendEvent();
    }

    public override void OnExit()
    {
        if (_eventDeath != null)
        {
            _eventDeath.Release();
            _eventDeath = null;
        }
    }

    public override bool IsFinished()
    {
        if (_eventDeath == null)
        {
            return true;
        }
        return _eventDeath.IsFinished;
    }

    void SendEvent()
    {
        _eventDeath = AnimFSMEventDeath.pool.Get();
        _eventDeath.damageType = Agent.BlackBoard.damageType;
        _eventDeath.fromWeapon = Agent.BlackBoard.attackerWeapon;
        _eventDeath.attacker = Agent.BlackBoard.attacker;
        _eventDeath.impuls = Agent.BlackBoard.impuls;
        FSMComponent.FSM.SendEvent(_eventDeath);
    }
}*/