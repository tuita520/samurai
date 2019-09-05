using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionOrderAttackMelee : GOAPActionBase
{
    AnimFSMEventAttackMelee _eventAttack;    

    public GOAPActionOrderAttackMelee(Agent1 agent, FSMComponent fsm,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)GOAPActionType1.ORDER_ATTACK_MELEE, agent, fsm, WSPrecondition, WSEffect)
    {
        
    }

    public override void Reset()
    {
        base.Reset();
        _eventAttack = null;
    }

    public override void OnEnter()
    {
        /*ComboMgr comboMgr = Agent.GetComponent<ComboMgr>();
        if (comboMgr)
        {
            comboMgr.Reset();
        }*/
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
        //_eventAttack.hitTimeStart = false;
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
        }
        else
        {
            _eventAttack.animAttackData = Agent.AnimSet.ProcessOrderCombo((Agent.PlayerOrder.GetCurOrder() as OrderDataAttack).attackType/*_eventAttack.attackType*/);
        }
        /*ComboMgr comboMgr = Agent.transform.GetComponent<ComboMgr>();
        if (comboMgr != null)
        {
            if (_eventAttack.target != null && _eventAttack.target.BlackBoard.IsKnockedDown)
            {
                _eventAttack.animAttackData = Agent.AnimSet.GetFirstAttackAnim(
                    Agent.BlackBoard.weaponSelected, AttackType.FATALITY);
                _eventAttack.attackType = AttackType.FATALITY;
                comboMgr.Reset();
            }
            else
                _eventAttack.animAttackData = comboMgr.ProcessCombo(_eventAttack.attackType);
        }
        else
        {
            _eventAttack.animAttackData = Agent.GetComponent<AnimSet>().GetFirstAttackAnim(
                Agent.BlackBoard.weaponSelected, _eventAttack.attackType);
        }        */

        FSMComponent.SendEvent(_eventAttack);
    }
}