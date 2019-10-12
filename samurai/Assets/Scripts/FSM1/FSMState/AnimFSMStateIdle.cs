using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;

public class AnimFSMStateIdle : AnimFSMState
{
    AnimFSMEventShowHideSword _eventSword;
    float _timeToFinishWeapon;  // 拔刀/收刀时间
    
    public AnimFSMStateIdle(Agent1 agent)
        : base((int)AnimFSMStateType.IDLE, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        Agent.BlackBoard.inIdle = true;
        PlayIdleAnim();
    }

    public override void OnExit()
    {
        Agent.BlackBoard.inIdle = false;
    }

    protected override void Initialize(FSMEvent ev)
    {
        Agent.BlackBoard.motionType = MotionType.NONE;
        Agent.BlackBoard.moveDir = Vector3.zero;
        Agent.BlackBoard.speed = 0;
        _timeToFinishWeapon = 0;
        _eventSword = null;

        if (ev is AnimFSMEvent)
        {
            (ev as AnimFSMEvent).IsFinished = true;
        }
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventIdle)
        {
            (ev as AnimFSMEventIdle).IsFinished = true;
            return true;
        }

        if (ev is AnimFSMEventShowHideSword)
        {
            if ((ev as AnimFSMEventShowHideSword).show == true)
            {                
                string animName = Agent.AnimSet.GetShowWeaponAnim(Agent.BlackBoard.weaponSelected);
                _timeToFinishWeapon = Time.timeSinceLevelLoad + Agent.AnimEngine[animName].length * 0.8f;                
                AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.1f);
            }
            else
            {
                string animName = Agent.AnimSet.GetHideWeaponAnim(Agent.BlackBoard.weaponSelected);
                _timeToFinishWeapon = Time.timeSinceLevelLoad + (Agent.AnimEngine[animName].length * 0.9f);
                AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.1f);
            }
            _eventSword = ev as AnimFSMEventShowHideSword;
            return true;
        }

        return false;
    }

    public override void OnUpdate()
    {
        if (_eventSword != null && _timeToFinishWeapon < Time.timeSinceLevelLoad) // 拔刀或者收刀完毕
        {
            Agent.BlackBoard.weaponState = _eventSword.show ? WeaponState.IN_HAND : WeaponState.NOT_IN_HANDS;
            _eventSword.IsFinished = true;
            _eventSword = null;
            _timeToFinishWeapon = 0;
            AnimationTools.PlayAnim(Agent.AnimEngine, Agent.AnimSet.GetIdleAnim(
                Agent.BlackBoard.weaponSelected, Agent.BlackBoard.weaponState), 0.2f); // 播放待机动作         
        }

        if (Agent.IsPlayer == false)
        {
            //Owner.BlackBoard.Vigor = Owner.BlackBoard.Vigor + 0.2f;
        }
    }

    void PlayIdleAnim()
    {
        //Debug.Log(Owner.AnimSet.GetIdleAnim(Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState).ToString());
        string name = Agent.AnimSet.GetIdleAnim(Agent.BlackBoard.weaponSelected, Agent.BlackBoard.weaponState);
        AnimationTools.PlayAnim(Agent.AnimEngine, name, 0.2f);
    }
}