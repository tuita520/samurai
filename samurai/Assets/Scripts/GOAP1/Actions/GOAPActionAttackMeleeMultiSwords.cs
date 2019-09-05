using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackMeleeMultiSwords : GOAPActionBase
{
    AnimFSMEventAttackMelee _eventAttack;
    int _remainAttackCount;    

    public GOAPActionAttackMeleeMultiSwords(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)GOAPActionType1.ATTACK_MELEE_MULTI_SWORDS, agent, fsm, WSPrecondition, WSEffect)
    {        

    }

    public override void Reset()
    {
        base.Reset();
        _eventAttack = null;
        _remainAttackCount = 0;        
    }

    public override void OnEnter()
    {
        int attackCount = Agent.AnimSet.GetComboAttackCount(ComboType.MULTI_SWORDS);
        if (attackCount == 0)
        {
            return;
        }
        _remainAttackCount = UnityEngine.Random.Range(1, attackCount+1);
        Agent.AnimSet.ResetComboProgress();
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
        
        _eventAttack.animAttackData = Agent.AnimSet.ProcessCombo(ComboType.MULTI_SWORDS);
        //_eventAttack.hitTimeStart = false;
        _eventAttack.attackPhaseDone = false;
        FSMComponent.SendEvent(_eventAttack);
        --_remainAttackCount;
    }

    public override void OnUpdate()
    {
        if (_eventAttack == null)
        {
            return;
        }
        if (_eventAttack.attackPhaseDone && _remainAttackCount > 0)
        {
            //Owner.SoundPlayPrepareAttack();
            _eventAttack.Release();
            SendEvent();            
        }       
    }

    public override bool IsFinished()
    {
        return (_eventAttack != null && _eventAttack.IsFinished && _remainAttackCount == 0);        
    }
}