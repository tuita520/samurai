using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackMeleeMultiSwords : GOAPActionBase
{
    AnimFSMEventAttackMelee _eventAttack;
    int _numberOfAttacks;
    int _curAttackNumber;
    //AttackType _curAttacktype;

    public GOAPActionAttackMeleeMultiSwords(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)GOAPActionType1.ATTACK_MELEE_MULTI_SWORDS, agent, fsm, WSPrecondition, WSEffect)
    {        

    }

    public override void Reset()
    {
        base.Reset();
        _eventAttack = null;
        _curAttackNumber = _numberOfAttacks = 0;
        //_curAttacktype = AttackType.NONE;
    }

    public override void OnEnter()
    {
        //_curAttacktype = AttackType.X;
        _numberOfAttacks = UnityEngine.Random.Range(1, 4);
        _curAttackNumber = 1;
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventAttack != null)
        {
            _eventAttack.Release();
            _eventAttack = null;
        }
        base.OnExit(ws);
    }

    void SendEvent()
    {
        _eventAttack = AnimFSMEventAttackMelee.pool.Get();
        _eventAttack.attackType = AttackType.X; // AttackType.O也行

        if (Agent.BlackBoard.desiredTarget)
        {
            Agent.BlackBoard.desiredDirection = Agent.BlackBoard.desiredTarget.Position - Agent.Position;
            Agent.BlackBoard.desiredDirection.Normalize();
            _eventAttack.attackDir = Agent.BlackBoard.desiredDirection;
        }
        else
        {
            _eventAttack.attackDir = Agent.Forward;
        }

        //_eventAttack.animAttackData = Agent.GetComponent<AnimSet>().GetFirstAttackAnim(
        //  Agent.BlackBoard.weaponSelected, _eventAttack.attackType);
        _eventAttack.animAttackData = Agent.AnimSet.ProcessCombo(ComboType.MULTI_SWORDS, ref _curAttackNumber);
        _eventAttack.hitDone = false;
        _eventAttack.attackPhaseDone = false;
        FSMComponent.SendEvent(_eventAttack);        
    }

    public override void OnUpdate()
    {
        if (_eventAttack == null)
        {
            return;
        }
        if (_eventAttack.attackPhaseDone && _curAttackNumber <= _numberOfAttacks)
        {
            //Owner.SoundPlayPrepareAttack();
            _eventAttack.Release();
            SendEvent();            
        }
        /*if (_eventAttack.attackPhaseDone && _numberOfAttacks > 0)
        {
            if (_curAttacktype == AttackType.X)
                _curAttacktype = AttackType.O;
            else
                _curAttacktype = AttackType.X;

            //Owner.SoundPlayPrepareAttack();
            _eventAttack.Release();
            SendEvent();
            --_numberOfAttacks;
        }*/
    }

    public override bool IsFinished()
    {
        return (_eventAttack != null && _eventAttack.IsFinished && _numberOfAttacks == 0);        
    }
}