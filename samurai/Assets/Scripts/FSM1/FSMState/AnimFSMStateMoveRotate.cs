using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;

public class AnimFSMStateMoveRotate : AnimFSMState
{
    AnimFSMEventMoveRotate _eventRotate;

    Quaternion _finalRotation;
    Quaternion _startRotation;
    
    float _rotationProgress;
    float _rotationTime;    
    string _animName;

    public AnimFSMStateMoveRotate(Agent1 agent)
        : base((int)AnimFSMStateType.MOVE_ROTATE, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        Agent.BlackBoard.motionType = MotionType.WALK;
        base.OnEnter(ev);        
        Agent.BlackBoard.moveDir = Vector3.zero;
        Agent.BlackBoard.speed = 0;        
    }

    public override void OnExit()
    {        
        Agent.BlackBoard.speed = 0;
        Agent.BlackBoard.motionType = MotionType.NONE;
    }

    protected override void Initialize(FSMEvent ev = null)
    {
        _eventRotate = ev as AnimFSMEventMoveRotate;
        
        _rotationProgress = 0;
        _startRotation = Agent.Transform.rotation;
        Vector3 finalDir;
        if (_eventRotate.target != null)
        {
            finalDir = (_eventRotate.target.Position + (_eventRotate.target.BlackBoard.moveDir * 
                _eventRotate.target.BlackBoard.speed * 0.5f)) - Agent.Transform.position;
            finalDir.Normalize();
        }
        else if (_eventRotate.direction != Vector3.zero)
        {
            finalDir = _eventRotate.direction;
        }
        else
            finalDir = Agent.Transform.forward;

        _animName = Agent.AnimSet.GetMoveAnim(Agent.BlackBoard.motionType, MoveType.FORWARD,
            Agent.BlackBoard.weaponSelected, Agent.BlackBoard.weaponState);
        AnimationTools.PlayAnim(Agent.AnimEngine, _animName, 0.2f);
        
        _finalRotation.SetLookRotation(finalDir);       
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventRotate)
        {
            if (_eventRotate != null)
                _eventRotate.IsFinished = true;
            Initialize(ev);
            return true;
        }
        return false;
    }

    public override void OnUpdate()
    {
        _rotationProgress += (Time.deltaTime * Agent.BlackBoard.rotationSmooth);
        if (_rotationProgress >= 1)
        {
            _rotationProgress = 1;
            IsFinished = true;
            _eventRotate.IsFinished = true;
        }
        
        Agent.Transform.rotation = Quaternion.Lerp(_startRotation, _finalRotation, _rotationProgress);
        Agent.BlackBoard.moveDir = Agent.Forward;

        float curSmooth = Agent.BlackBoard.speedSmooth * Time.deltaTime;
        Agent.BlackBoard.speed = Mathf.Lerp(Agent.BlackBoard.speed, Agent.BlackBoard.maxWalkSpeed, curSmooth);
        TransformTools.Instance.MoveOnGround(Agent.transform, Agent.CharacterController,
            Agent.BlackBoard.moveDir * Agent.BlackBoard.speed * Time.deltaTime, true);

        /*_currentRotationTime += Time.deltaTime;
        if (_currentRotationTime >= _rotationTime)
        {
            _currentRotationTime = _rotationTime;
            IsFinished = true;
            _eventRotate.IsFinished = true;
            return;
        }

        float progress = _currentRotationTime / _rotationTime;
        Agent.Transform.rotation = Quaternion.Lerp(_startRotation, _finalRotation, progress);
        Agent.BlackBoard.moveDir = Agent.Forward;

        float curSmooth = Agent.BlackBoard.speedSmooth * Time.deltaTime;
        Agent.BlackBoard.speed = Mathf.Lerp(Agent.BlackBoard.speed, Agent.BlackBoard.maxWalkSpeed, curSmooth);
        TransformTools.MoveOnGround(Agent.transform, Agent.CharacterController,
            Agent.BlackBoard.moveDir * Agent.BlackBoard.speed * Time.deltaTime, true);*/
    }

}