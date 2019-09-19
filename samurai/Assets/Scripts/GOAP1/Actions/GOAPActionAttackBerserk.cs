using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackBerserk : GOAPActionBase
{
    AnimFSMEventAttackMelee _eventAttack;

    public GOAPActionAttackBerserk(GOAPActionType1 actionType, Agent1 agent, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {        

    }

    public override void Reset()
    {
        base.Reset();
        _eventAttack = null;
    }

    public override void OnEnter()
    {        
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

        _eventAttack.animAttackData = Agent.AnimSet.ProcessCombo(ComboType.ATTACK_BERSERK);
        //_eventAttack.hitTimeStart = false;        
        _eventAttack.attackPhaseDone = false;
        Agent.FSMComponent.SendEvent(_eventAttack);        
    }

    public override bool IsFinished()
    {
        return (_eventAttack != null && _eventAttack.IsFinished);        
    }
}