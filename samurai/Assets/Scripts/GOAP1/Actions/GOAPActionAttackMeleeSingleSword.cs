using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackMeleeSingleSword : GOAPActionBase
{
    AnimFSMEventAttackMelee _eventAttack;

    public GOAPActionAttackMeleeSingleSword(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)GOAPActionType1.ATTACK_MELEE_SINGLE_SWORD, agent, fsm, WSPrecondition, WSEffect)
    {        

    }

    public override void Reset()
    {
        base.Reset();
        _eventAttack = null;
    }

    public override void OnEnter()
    {
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

        _eventAttack.animAttackData = Agent.AnimSet.ProcessCombo(ComboType.SINGLE_SWORD);
        //_eventAttack.hitTimeStart = false;        
        _eventAttack.attackPhaseDone = false;
        FSMComponent.SendEvent(_eventAttack);        
    }

    public override bool IsFinished()
    {
        return (_eventAttack != null && _eventAttack.IsFinished);        
    }
}