using System.Collections.Generic;
using Phenix.Unity.AI;
using UnityEngine;

public class GOAPActionReactToDamageBoss : GOAPActionBase
{
    AnimFSMEventInjuryBoss _eventInjury;    
    AnimFSMEventDeath _eventDeath;
    AnimFSMEventAttackMelee _eventAttack;

    Agent1 _attacker = null;
    int _attackerRepeatCount = 0;
    
    DamageResultType _damageResultType = DamageResultType.NONE;

    int _injuryPhrase = 0;
    int maxInjuryPhrase = 3;

    const float _startTrailDelay = 0.5f;
    float _startTrailTimer = 0;

    public GOAPActionReactToDamageBoss(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();
        _injuryPhrase = 0;
        _eventInjury = null;        
        _eventDeath = null;
        _eventAttack = null;
        _attacker = null;
        _attackerRepeatCount = 0;
        _damageResultType = DamageResultType.NONE;
        _startTrailTimer = 0;
    }

    public override void OnEnter()
    {
        _injuryPhrase = 0;
        _attacker = Agent.BlackBoard.Attacker;
        _attackerRepeatCount = Agent.BlackBoard.AttackerRepeatCount;
        _damageResultType = Agent.BlackBoard.damageResultType;
        _startTrailTimer = 0;

        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        Agent.BlackBoard.invulnerable = false;
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
        if (_eventAttack != null)
        {
            _eventAttack.Release();
            _eventAttack = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        if (_eventInjury != null)
        {
            return _eventInjury.IsFinished && (_eventAttack == null || _eventAttack.IsFinished);
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

        if (_eventInjury != null && _eventInjury.IsFinished && _injuryPhrase >= 3 && _eventAttack == null)
        {
            SendRevengeAttackEvent();
        }

        if (_eventAttack != null)
        {
            if (_eventAttack.attackPhaseStart && _startTrailTimer == 0)
            {
                _startTrailTimer = Time.timeSinceLevelLoad + _startTrailDelay;                 
            }

            if (_startTrailTimer > 0 && Time.timeSinceLevelLoad > _startTrailTimer)
            {
                // 显示刀光(方案2：动态生成mesh，参见BossOrochi)
                Agent.PlayWeaponTrail(false);
            }
            
            if (_eventAttack.attackPhaseDone)
            {
                // 关闭刀光(方案2：动态生成mesh，参见BossOrochi)
                Agent.StopWeaponTrail();
            }
        }
    }

    void SendEvent()
    {
        switch (_damageResultType)
        {
            case DamageResultType.INJURY:                
                if (_eventDeath != null)
                {
                    _eventDeath.Release();
                    _eventDeath = null;
                }
                SendInjuryEvent();
                break;            
            case DamageResultType.DEATH:                
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
        if (++_injuryPhrase > maxInjuryPhrase) // 只有3种受伤阶段
        {
            return;
        }

        if (_eventInjury != null)
        {
            _eventInjury.IsFinished = true;
            _eventInjury.Release();
        }
        _eventInjury = AnimFSMEventInjuryBoss.pool.Get();
        _eventInjury.damageType = Agent.BlackBoard.damageType;
        _eventInjury.fromWeapon = Agent.BlackBoard.attackerWeapon;
        _eventInjury.attacker = Agent.BlackBoard.Attacker;
        _eventInjury.impuls = Agent.BlackBoard.impuls;
        Agent.FSMComponent.SendEvent(_eventInjury);        
    }

    void SendDeathEvent()
    {
        _eventDeath = AnimFSMEventDeath.pool.Get();
        _eventDeath.damageType = Agent.BlackBoard.damageType;
        _eventDeath.fromWeapon = Agent.BlackBoard.attackerWeapon;
        _eventDeath.attacker = Agent.BlackBoard.Attacker;
        _eventDeath.impuls = Agent.BlackBoard.impuls;
        Agent.FSMComponent.SendEvent(_eventDeath);
    }

    void SendRevengeAttackEvent()
    {
        HandleSound.PlaySoundBerserk();
        Agent.BlackBoard.invulnerable = true;
        _eventAttack = AnimFSMEventAttackMelee.pool.Get();
        _eventAttack.animAttackData = Agent.AnimSet.ProcessCombo(ComboType.REVENGE);
        _eventAttack.attackDir = Agent.Forward;
        Agent.FSMComponent.SendEvent(_eventAttack);
    }
}