using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackMeleeMultiSwords : GOAPActionBase
{
    AnimFSMEventAttackMelee _eventAttack;
    int _remainAttackCount;
    ComboType _comboType;

    protected AnimFSMEventAttackMelee EventAttack { get { return _eventAttack; } }

    public GOAPActionAttackMeleeMultiSwords(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)actionType, agent, WSPrecondition, WSEffect)
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
        _comboType = GetComboType();
        int attackCount = Agent.AnimSet.GetComboAttackCount(_comboType);
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
            _eventAttack.target = Agent.BlackBoard.HasAttackTarget ? Agent.BlackBoard.desiredTarget : null;
        }
        else
        {
            _eventAttack.attackDir = Agent.Forward;
        }

        _eventAttack.animAttackData = Agent.AnimSet.ProcessCombo(_comboType);        
        _eventAttack.attackPhaseDone = false;
        Agent.FSMComponent.SendEvent(_eventAttack);
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

    protected virtual ComboType GetComboType()
    {
        return ComboType.MULTI_SWORDS;
    }
}