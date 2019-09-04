using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionReactToDamage : GOAPActionBase
{
    AnimFSMEventInjury _eventInjury;
    AnimFSMEventKnockDown _eventKnockDown;
    AnimFSMEventDeath _eventDeath;

    Agent1 _attacker = null;
    int _attackerRepeatCount = 0;
    DamageResultType _damageResultType = DamageResultType.NONE;


    public GOAPActionReactToDamage(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)GOAPActionType1.REACT_TO_DAMAGE, agent, fsm, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();
        _eventInjury = null;
        _eventKnockDown = null;
        _eventDeath = null;
        _attacker = null;
        _attackerRepeatCount = 0;
        _damageResultType = DamageResultType.NONE;
    }

    public override void OnEnter()
    {
        _attacker = Agent.BlackBoard.Attacker;
        _attackerRepeatCount = Agent.BlackBoard.AttackerRepeatCount;
        _damageResultType = Agent.BlackBoard.damageResultType;

        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventInjury != null)
        {
            _eventInjury.Release();
            _eventInjury = null;
        }
        if (_eventKnockDown != null)
        {
            _eventKnockDown.Release();
            _eventKnockDown = null;
        }
        if (_eventDeath != null)
        {
            _eventDeath.Release();
            _eventDeath = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        if (_eventInjury != null)
        {
            return _eventInjury.IsFinished;
        }
        else if (_eventKnockDown != null)
        {
            return _eventKnockDown.IsFinished;
        }
        else if (_eventDeath != null)
        {
            return _eventDeath.IsFinished;
        }
        return true;
    }

    public override void OnUpdate()
    {
        if (_damageResultType < Agent.BlackBoard.damageResultType)
        {
            _damageResultType = Agent.BlackBoard.damageResultType;
            _attacker = Agent.BlackBoard.Attacker;
            _attackerRepeatCount = Agent.BlackBoard.AttackerRepeatCount;
            SendEvent();
        }
        else if (_damageResultType == Agent.BlackBoard.damageResultType)
        {
            if (_attacker != Agent.BlackBoard.Attacker)
            {
                _attacker = Agent.BlackBoard.Attacker;
                _attackerRepeatCount = Agent.BlackBoard.AttackerRepeatCount;
                SendEvent();
            }
            else if (_attackerRepeatCount != Agent.BlackBoard.AttackerRepeatCount)
            {
                _attackerRepeatCount = Agent.BlackBoard.AttackerRepeatCount;
                SendEvent();
            }

        }
    }

    void SendEvent()
    {
        switch (_damageResultType)
        {
            case DamageResultType.INJURY:
                if (_eventKnockDown != null)
                {
                    _eventKnockDown.Release();
                    _eventKnockDown = null;
                }
                if (_eventDeath != null)
                {
                    _eventDeath.Release();
                    _eventDeath = null;
                }
                SendInjuryEvent();
                break;
            case DamageResultType.KNOCK_DOWN:
                if (_eventInjury != null)
                {
                    _eventInjury.Release();
                    _eventInjury = null;
                }
                if (_eventDeath != null)
                {
                    _eventDeath.Release();
                    _eventDeath = null;
                }
                SendKnockDownEvent();
                break;
            case DamageResultType.DEATH:
                if (_eventKnockDown != null)
                {
                    _eventKnockDown.Release();
                    _eventKnockDown = null;
                }
                if (_eventInjury != null)
                {
                    _eventInjury.Release();
                    _eventInjury = null;
                }
                SendDeathEvent();
                break;
            default:
                return;
        }
    }

    void SendInjuryEvent()
    {
        _eventInjury = AnimFSMEventInjury.pool.Get();
        _eventInjury.damageType = Agent.BlackBoard.damageType;
        _eventInjury.fromWeapon = Agent.BlackBoard.attackerWeapon;
        _eventInjury.attacker = Agent.BlackBoard.Attacker;
        _eventInjury.impuls = Agent.BlackBoard.impuls;        
        FSMComponent.SendEvent(_eventInjury);
    }

    void SendKnockDownEvent()
    {
        _eventKnockDown = AnimFSMEventKnockDown.pool.Get();
        _eventKnockDown.fromWeapon = Agent.BlackBoard.attackerWeapon;
        _eventKnockDown.attacker = Agent.BlackBoard.Attacker;
        _eventKnockDown.impuls = Agent.BlackBoard.impuls;
        _eventKnockDown.lyingTime = Agent.BlackBoard.maxKnockdownTime * Random.Range(0.7f, 1);
        FSMComponent.SendEvent(_eventKnockDown);
    }

    void SendDeathEvent()
    {
        _eventDeath = AnimFSMEventDeath.pool.Get();
        _eventDeath.damageType = Agent.BlackBoard.damageType;
        _eventDeath.fromWeapon = Agent.BlackBoard.attackerWeapon;
        _eventDeath.attacker = Agent.BlackBoard.Attacker;
        _eventDeath.impuls = Agent.BlackBoard.impuls;
        FSMComponent.SendEvent(_eventDeath);
    }
}