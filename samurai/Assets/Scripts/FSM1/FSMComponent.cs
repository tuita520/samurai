using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public class FSMComponent : MonoBehaviour
{
    Agent1 _agent;
    AnimFSM _fsm;

    public List<AnimFSMStateType> FSMStates = new List<AnimFSMStateType>();
    public AnimFSMStateType defFSMState;

    //public AnimFSM FSM { get { return _fsm; } }

    // Use this for initialization
    void Start()
    {
        _agent = GetComponent<Agent1>();
        List<FSMState> states = new List<FSMState>();
        FSMState defState = null;
        foreach (var stateType in FSMStates)
        {
            FSMState state = null;
            switch (stateType)
            {                
                case AnimFSMStateType.IDLE:
                    state = new AnimFSMStateIdle(_agent);                    
                    break;
                case AnimFSMStateType.GOTO:
                    state = new AnimFSMStateGoTo(_agent);
                    break;
                case AnimFSMStateType.COMBAT_MOVE:
                    state= new AnimFSMStateCombatMove(_agent);
                    break;
                case AnimFSMStateType.ROTATE:
                    state = new AnimFSMStateRotate(_agent);
                    break;
                case AnimFSMStateType.ATTACK_MELEE:
                    state = new AnimFSMStateAttackMelee(_agent);
                    break;
                case AnimFSMStateType.ATTACK_WHIRL:
                    state = new AnimFSMStateAttackWhirl(_agent);
                    break;
                case AnimFSMStateType.INJURY:
                    state = new AnimFSMStateInjury(_agent);
                    break;
                case AnimFSMStateType.DEATH:
                    state = new AnimFSMStateDeath(_agent);
                    break;
                case AnimFSMStateType.KNOCK_DOWN:
                    state = new AnimFSMStateKnockDown(_agent);
                    break;                
                case AnimFSMStateType.PLAY_ANIM:
                    state = new AnimFSMStatePlayAnim(_agent);
                    break;
                case AnimFSMStateType.MOVE:
                    state = new AnimFSMStateMove(_agent);
                    break;
                case AnimFSMStateType.ROLL:
                    state = new AnimFSMStateRoll(_agent);
                    break;
                case AnimFSMStateType.BLOCK:
                    state = new AnimFSMStateBlock(_agent);
                    break;
                case AnimFSMStateType.ATTACK_CROSS:
                    state = new AnimFSMStateAttackCross(_agent);
                    break;
                case AnimFSMStateType.ATTACK_ROLL:
                    state = new AnimFSMStateAttackRoll(_agent);
                    break;
                case AnimFSMStateType.GOTO_TARGET:
                    state = new AnimFSMStateGoToTarget(_agent);
                    break;
                default:
                    break;
            }

            if (state != null)
            {
                states.Add(state);
                if (state.StateType == (int)defFSMState)
                {
                    defState = state;
                }                
            }            
        }
        _fsm = new AnimFSM(states, defState);        
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.OnUpdate();
    }

    public void SendEvent(AnimFSMEvent ev)
    {
        ev.onEventFinished += _agent.UpdateCombatProps;
        _fsm.SendEvent(ev);
    }
}
