using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;

public class AnimFSMStateRotate : AnimFSMState
{
    AnimFSMEventRotate _eventRotate;

    Quaternion _finalRotation;
    Quaternion _startRotation;
    //float _currentRotationTime;
    //float _rotationTime;    
    float _rotationProgress;
    string _animName;

    public AnimFSMStateRotate(Agent1 agent)
        : base((int)AnimFSMStateType.ROTATE, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        Agent.BlackBoard.motionType = MotionType.NONE;
        Agent.BlackBoard.moveDir = Vector3.zero;
        Agent.BlackBoard.speed = 0;        
    }       

    protected override void Initialize(FSMEvent ev = null)
    {
        _eventRotate = ev as AnimFSMEventRotate;
        //_currentRotationTime = 0;
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

        if (Vector3.Dot(finalDir, Agent.Transform.right) > 0)
            _animName = Agent.AnimSet.GetRotateAnim(Agent.BlackBoard.motionType, RotationType.RIGHT);
        else
            _animName = Agent.AnimSet.GetRotateAnim(Agent.BlackBoard.motionType, RotationType.LEFT);

        Tools.PlayAnimation(Agent.AnimEngine, _animName, 0.01f, QueueMode.CompleteOthers);
        
        _finalRotation.SetLookRotation(finalDir);
        /*_rotationTime = Vector3.Angle(Agent.Transform.forward, finalDir) / (360 * Agent.BlackBoard.rotationSmooth);

        if (_rotationTime == 0)
        {
            IsFinished = true;
            _eventRotate.IsFinished = true;
        }

        float animLen = Agent.AnimEngine[_animName].length;
        int multi = Mathf.CeilToInt(_rotationTime / animLen);       

        _rotationTime = animLen * multi + Time.timeSinceLevelLoad;*/
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
        /*_currentRotationTime += Time.deltaTime * (360 * Agent.BlackBoard.rotationSmooth);
        if (_currentRotationTime >= _rotationTime)
        {
            _currentRotationTime = _rotationTime;
        }

        float progress = _currentRotationTime / _rotationTime;
        Quaternion q = Quaternion.Lerp(_startRotation, _finalRotation, Mathfx.Hermite(0, 1, progress));
        Agent.Transform.rotation = q;
        if (_rotationTime <= Time.timeSinceLevelLoad)
        {
            IsFinished = true;
            _eventRotate.IsFinished = true;
        }*/
        _rotationProgress += (Time.deltaTime * Agent.BlackBoard.rotationSmooth);
        Agent.Transform.rotation = Quaternion.Lerp(_startRotation, _finalRotation, _rotationProgress/*Mathfx.Hermite(0, 1, progress)*/);         
        if (_rotationProgress > 1)
        {
            IsFinished = true;
            _eventRotate.IsFinished = true;
            _rotationProgress = 1;
        }
    }

}