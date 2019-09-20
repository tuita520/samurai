using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;
/*
public class AnimFSMStateGoToTarget : AnimFSMState
{
    AnimFSMEventGoToTarget _eventGoToTarget;
    float _maxSpeed;
    string _animName;

    Quaternion _finalRotation = new Quaternion();
    Quaternion _startRotation = new Quaternion();
    float _rotationProgress;

    public AnimFSMStateGoToTarget(Agent1 agent)
        : base((int)AnimFSMStateType.GOTO_TARGET, agent)
    {

    }

    public override void OnEnter(FSMEvent ev)
    {
        base.OnEnter(ev);
        _animName = null;
        PlayAnim(_eventGoToTarget.motionType);
    }

    public override void OnExit()
    {
        Agent.AnimEngine[_animName].speed = 1;
        Agent.BlackBoard.speed = 0;          
    }

    public override void OnUpdate()
    {
        float dist = (_eventGoToTarget.target.Position - Agent.Transform.position).sqrMagnitude;       

        if (_eventGoToTarget.motionType == MotionType.SPRINT)
        {
            if (dist < 0.5f * 0.5f)
                _maxSpeed = Agent.BlackBoard.maxWalkSpeed;
        }
        else
        {
            if (dist < 1.5f * 1.5f)
                _maxSpeed = Agent.BlackBoard.maxWalkSpeed;
        }

        _rotationProgress += Time.deltaTime * Agent.BlackBoard.rotationSmooth;
        _rotationProgress = Mathf.Min(_rotationProgress, 1);
        Quaternion q = Quaternion.Slerp(_startRotation, _finalRotation, _rotationProgress);
        Agent.Transform.rotation = q;

        float curSmooth = Agent.BlackBoard.speedSmooth * Time.deltaTime;
        Agent.BlackBoard.speed = Mathf.Lerp(Agent.BlackBoard.speed, _maxSpeed, curSmooth);

        Vector3 dir = _eventGoToTarget.target.Position - Agent.Transform.position;
        dir.y = 0;
        dir.Normalize();
        Agent.BlackBoard.moveDir = dir;
        _finalRotation.SetLookRotation(dir);

        // MOVE
        if (TransformTools.MoveOnGround(Agent.transform, Agent.CharacterController, 
            Agent.BlackBoard.moveDir * Agent.BlackBoard.speed * Time.deltaTime, true) == false)
        {
            IsFinished = true;
        }
        else if ((_eventGoToTarget.target.Position - Agent.Transform.position).sqrMagnitude <= 
            Agent.BlackBoard.SqrWeaponRange)
        {
            IsFinished = true;
            _eventGoToTarget.IsFinished = true;
        }
        else
        {
            MotionType motion = GetMotionType();
            if (motion != Agent.BlackBoard.motionType)
                PlayAnim(motion);
        }

    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventGoToTarget)
        {
            //if (_eventGoTo != null)
            //{
            //    _eventGoTo.IsFinished = true;
            //}
            Initialize(ev);
            return true;
        }

        if (ev is AnimFSMEventShowHideSword)
        {
            ev.IsFinished = true;
            PlayAnim(GetMotionType());
            return true;
        }

        return false;
    }    

    protected override void Initialize(FSMEvent ev)
    {
        _eventGoToTarget = ev as AnimFSMEventGoToTarget;

        Vector3 dir = _eventGoToTarget.target.Position - Agent.Transform.position;
        dir.y = 0;
        dir.Normalize();
        if (dir != Vector3.zero)
            _finalRotation.SetLookRotation(dir);

        _startRotation = Agent.Transform.rotation;

        Agent.BlackBoard.motionType = GetMotionType();

        if (_eventGoToTarget.motionType == MotionType.SPRINT)
        {
            _maxSpeed = Agent.BlackBoard.maxSprintSpeed;
        }
        else if (_eventGoToTarget.motionType == MotionType.RUN)
            _maxSpeed = Agent.BlackBoard.maxRunSpeed;
        else
            _maxSpeed = Agent.BlackBoard.maxWalkSpeed;

        _rotationProgress = 0;
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
}*/