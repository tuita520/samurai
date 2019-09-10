using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;

public class AnimFSMStateFlash : AnimFSMState
{
    AnimFSMEventFlash _eventFlash;    

    string _animName;
    Quaternion _finalRotation = new Quaternion();
    Quaternion _startRotation = new Quaternion();
    float _currentRotationTime;
    float _rotationTime;

    public AnimFSMStateFlash(Agent1 agent)
        : base((int)AnimFSMStateType.FLASH, agent)
    {

    }

    public override void OnEnter(FSMEvent ev)
    {
        base.OnEnter(ev);
        _animName = null;
        PlayAnim(_eventFlash.motionType);
    }

    public override void OnExit()
    {
        Agent.AnimEngine[_animName].speed = 1;
        Agent.BlackBoard.speed = 0;          
    }

    public override void OnUpdate()
    {
        if (_eventFlash.target == null)
        {
            IsFinished = true;
            _eventFlash.IsFinished = true;
            return;
        }

        _currentRotationTime += Time.deltaTime * (360 * Agent.BlackBoard.rotationSmooth);
        if (_currentRotationTime >= _rotationTime)
        {
            _currentRotationTime = _rotationTime;
        }

        float progress = _currentRotationTime / _rotationTime;
        Quaternion q = Quaternion.Lerp(_startRotation, _finalRotation, Mathfx.Hermite(0, 1, progress));
        Agent.Transform.rotation = q;
        if (_rotationTime <= Time.timeSinceLevelLoad)
        {
            Agent.Transform.position = _eventFlash.finalPosition;
            Agent.Transform.LookAt(_eventFlash.target.Transform.position);
            IsFinished = true;
            _eventFlash.IsFinished = true;
        }
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventFlash)
        {
            //if (_eventGoTo != null)
            //{
            //    _eventGoTo.IsFinished = true;
            //}
            Initialize(ev);
            return true;
        }

        return false;
    }    

    protected override void Initialize(FSMEvent ev)
    {
        _eventFlash = ev as AnimFSMEventFlash;
        if (_eventFlash == null || _eventFlash.target == null)
        {
            IsFinished = true;
            _eventFlash.IsFinished = true;
            return;
        }

        Vector3 dir = _eventFlash.finalPosition - Agent.Transform.position;
        dir.y = 0;
        dir.Normalize();
        if (dir != Vector3.zero)
            _finalRotation.SetLookRotation(dir);

        _startRotation = Agent.Transform.rotation;
        Agent.BlackBoard.motionType = GetMotionType();

        _rotationTime = Vector3.Angle(Agent.Transform.forward, _eventFlash.target.Position - Agent.Position) / 
            (360 * Agent.BlackBoard.rotationSmooth);
        if (_rotationTime == 0)
        {
            Agent.Transform.position = _eventFlash.finalPosition;
            Agent.Transform.LookAt(_eventFlash.target.Transform.position);
            IsFinished = true;
            _eventFlash.IsFinished = true;
        }
    }

    private void PlayAnim(MotionType motion)
    {
        Agent.BlackBoard.motionType = motion;
        _animName = Agent.AnimSet.GetMoveAnim(Agent.BlackBoard.motionType, MoveType.FORWARD, 
            Agent.BlackBoard.weaponSelected, Agent.BlackBoard.weaponState);
        Tools.PlayAnimation(Agent.AnimEngine, _animName, 0.2f);
    }

    MotionType GetMotionType()
    {
        if (Agent.BlackBoard.speed > Agent.BlackBoard.maxRunSpeed * 1.5f)
            return MotionType.SPRINT;
        else if (Agent.BlackBoard.speed > Agent.BlackBoard.maxWalkSpeed * 1.5f)
            return MotionType.RUN;

        return MotionType.WALK;
    }
}