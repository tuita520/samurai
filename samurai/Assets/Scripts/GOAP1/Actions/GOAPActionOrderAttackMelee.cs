using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionOrderAttackMelee : GOAPActionBase
{
    AnimFSMEventAttackMelee _eventAttack;    

    public GOAPActionOrderAttackMelee(GOAPActionType1 actionType, Agent1 agent,
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

    public override bool IsFinished()
    {
        return _eventAttack != null && _eventAttack.IsFinished;
    }

    public override void OnUpdate()
    {
        if (_eventAttack == null || _eventAttack.IsFinished || _eventAttack.attackPhaseDone == false)
        {
            return;
        }

        OrderData nextOrder = Agent.PlayerOrder.GetNextOrder();
        if (nextOrder != null && nextOrder.orderType == (int)OrderType.ATTACK)
        {
            PlayerOrderPool.attacks.Collect(Agent.PlayerOrder.Pop() as OrderDataAttack);
            _eventAttack.Release();
            SendEvent();
        }        
    }

    void SendEvent()
    {
        _eventAttack = AnimFSMEventAttackMelee.pool.Get();

        //_eventAttack.attackType = (Agent.PlayerOrder.GetCurOrder() as OrderDataAttack).attackType;        
        _eventAttack.attackPhaseStart = false;
        _eventAttack.attackPhaseDone = false;

        Agent.BlackBoard.desiredTarget = _eventAttack.target = Agent.Decision.SelectTarget(
            Agent.BlackBoard.combatRange, Game1.Instance.AgentMgr.agents);
        if (_eventAttack.target)
        {
            Agent.BlackBoard.desiredDirection = Agent.BlackBoard.desiredTarget.Position - Agent.Position;
            Agent.BlackBoard.desiredDirection.Normalize();
            _eventAttack.attackDir = Agent.BlackBoard.desiredDirection;
        }
        else
        {
            _eventAttack.attackDir = Agent.Forward;
        }

        if (_eventAttack.target != null && _eventAttack.target.BlackBoard.IsKnockedDown)
        {            
            _eventAttack.animAttackData = Agent.AnimSet.ProcessCombo(ComboType.JUMP_KILL);
            //_eventAttack.attackType = OrderAttackType.FATALITY;
            Agent.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.BERSERK);
        }
        else
        {
            _eventAttack.animAttackData = Agent.AnimSet.ProcessOrderCombo((Agent.PlayerOrder.GetCurOrder() as OrderDataAttack).attackType/*_eventAttack.attackType*/);
            if (_eventAttack.animAttackData.fullCombo)
            {
                Agent.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.BERSERK);
            }
            else if (UnityEngine.Random.Range(0, 100) < 20)
            {
                Agent.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ATTACK_PREPARE);
            }
        }        

        Agent.FSMComponent.SendEvent(_eventAttack);
        NotifyUI(Agent.BlackBoard.desiredTarget);   
    }

    void NotifyUI(Agent1 newTarget)
    {
        MsgTargetChanged msg = MsgTargetChanged.pool.Get();
        msg.newAgent = newTarget;
        UIFacadeComponent.Instance.SendMsg(msg);
    }
}