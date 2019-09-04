using UnityEngine;
using UnityEngine.Events;

public class SimpleDecision : Decision
{
    public System.Object val;
    public int a;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKey(KeyCode.Alpha1))
        {
            AnimFSMEventGoTo ev = AnimFSMEventGoTo.pool.Get();
            ev.moveType = MoveType.FORWARD;
            ev.motionType = MotionType.RUN;
            ev.finalPosition = Agent.Decision.GetBestAttackStart(Agent.BlackBoard.desiredTarget);
            Agent.FSMComponent.SendEvent(ev);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            AnimFSMEventShowHideSword ev = AnimFSMEventShowHideSword.pool.Get();
            ev.show = true;
            Agent.BlackBoard.weaponState = WeaponState.IN_HAND;
            Agent.FSMComponent.SendEvent(ev);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            AnimFSMEventIdle ev = AnimFSMEventIdle.pool.Get();
            Agent.FSMComponent.SendEvent(ev);
        }        
        if (Input.GetKey(KeyCode.Alpha4))
        {
            AnimFSMEventRotate ev = AnimFSMEventRotate.pool.Get();
            ev.target = Agent.BlackBoard.desiredTarget;
            ev.rotationModifier = 0.8f;
            Agent.FSMComponent.SendEvent(ev);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            AnimFSMEventCombatMove ev = AnimFSMEventCombatMove.pool.Get();
            ev.target = Agent.BlackBoard.desiredTarget;
            ev.minDistanceToTarget = 3;
            ev.moveType = MoveType.FORWARD;
            if (Agent.agentType == AgentType.DOUBLE_SWORDS_MAN || Agent.agentType == AgentType.SWORD_MAN) // 这里暂时偷个懒，另外扩展一个不同参数的goapaction才是更灵活的做法
            {
                ev.motionType = MotionType.WALK;
                ev.totalMoveDistance = Agent.BlackBoard.DistanceToTarget - (Agent.BlackBoard.combatRange * 0.8f);
            }
            else
            {
                ev.totalMoveDistance = UnityEngine.Random.Range((Agent.BlackBoard.DistanceToTarget - (Agent.BlackBoard.combatRange * 0.5f)) * 0.5f,
                    Agent.BlackBoard.DistanceToTarget - (Agent.BlackBoard.combatRange * 0.5f));
            }
            Agent.FSMComponent.SendEvent(ev);
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            AnimFSMEventCombatMove ev = AnimFSMEventCombatMove.pool.Get();
            ev.moveType = MoveType.BACKWARD;
            ev.target = Agent.BlackBoard.desiredTarget;
            ev.minDistanceToTarget = 0;
            if (Agent.agentType == AgentType.DOUBLE_SWORDS_MAN) // 这里暂时偷个懒，另外扩展一个不同参数的goapaction才是更灵活的做法
            {
                ev.motionType = MotionType.RUN;
                ev.totalMoveDistance = UnityEngine.Random.Range(4f, 6f);
            }
            else
            {
                ev.totalMoveDistance = UnityEngine.Random.Range(2f, 5f);
            }
            Agent.FSMComponent.SendEvent(ev);
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            AnimFSMEventCombatMove ev = AnimFSMEventCombatMove.pool.Get();
            ev.moveType = MoveType.LEFTWARD;
            ev.target = Agent.BlackBoard.desiredTarget;
            ev.totalMoveDistance = UnityEngine.Random.Range(2.0f, 4.0f);
            ev.minDistanceToTarget = Agent.BlackBoard.DistanceToTarget * 0.7f;
            Agent.FSMComponent.SendEvent(ev);
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            AnimFSMEventCombatMove ev = AnimFSMEventCombatMove.pool.Get();
            ev.moveType = MoveType.RIGHTWARD;
            ev.target = Agent.BlackBoard.desiredTarget;
            ev.totalMoveDistance = UnityEngine.Random.Range(2.0f, 4.0f);
            ev.minDistanceToTarget = Agent.BlackBoard.DistanceToTarget * 0.7f;
            Agent.FSMComponent.SendEvent(ev);
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            AnimFSMEventShowHideSword ev = AnimFSMEventShowHideSword.pool.Get();
            ev.show = false;
            Agent.BlackBoard.weaponState = WeaponState.NOT_IN_HANDS;
            Agent.FSMComponent.SendEvent(ev);
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            AnimFSMEventAttackMelee ev = AnimFSMEventAttackMelee.pool.Get();
            ev.target = Agent.BlackBoard.desiredTarget;
            ev.attackType = AttackType.X;
            ev.animAttackData = Agent.GetComponent<AnimSet>().GetFirstAttackAnim(Agent.BlackBoard.weaponSelected, 
                ev.attackType);
            Agent.FSMComponent.SendEvent(ev);
        }
        if (Input.GetKey(KeyCode.Minus))
        {
            AnimFSMEventAttackWhirl ev = AnimFSMEventAttackWhirl.pool.Get();
            ev.data = Agent.GetComponent<AnimSet>().GetWhirlAttackAnim();
            Agent.FSMComponent.SendEvent(ev);
        }
    }
}
