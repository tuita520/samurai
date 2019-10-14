using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.Extend;
using Phenix.Unity.AI;

public class AnimFSMStateRoll : AnimFSMState
{
    AnimFSMEventRoll _eventRoll;

    Quaternion _finalRotation;
    Quaternion _startRotation;
    Vector3 _startPosition;
    Vector3 _finalPosition;
    float _currentRotationTime;
    float _rotationTime;
    float _currentMoveTime;
    float _moveTime;
    float _endOfStateTime;
    
    bool _rotationOk = false;
    bool _positionOK = false;    

    public AnimFSMStateRoll(Agent1 agent)
        : base((int)AnimFSMStateType.ROLL, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        Agent.BlackBoard.invulnerable = true;        
    }

    public override void OnExit()
    {
        Agent.BlackBoard.speed = 0;
        Agent.BlackBoard.invulnerable = false;        
    }

    protected override void Initialize(FSMEvent ev)
    {
        _eventRoll = ev as AnimFSMEventRoll;
        _currentMoveTime = 0;
        _currentRotationTime = 0;
        _startRotation = Agent.Transform.rotation;
        _startPosition = Agent.Transform.position;

        Vector3 finalDir;
        if (_eventRoll.toTarget != null)
        {
            finalDir = _eventRoll.toTarget.Position - Agent.Transform.position;
            finalDir.Normalize();

            _finalPosition = _eventRoll.toTarget.Position - finalDir * Agent.BlackBoard.weaponRange;
        }
        else
        {
            finalDir = _eventRoll.direction;
            _finalPosition = _startPosition + _eventRoll.direction * Agent.BlackBoard.rollDistance;
        }

        string animName = Agent.AnimSet.GetRollAnim(Agent.BlackBoard.weaponSelected, Agent.BlackBoard.weaponState);
        AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.1f);

        _finalRotation.SetLookRotation(finalDir);
        _rotationTime = Vector3.Angle(Agent.Transform.forward, finalDir) / 1000.0f;
        _moveTime = Agent.AnimEngine[animName].length * 0.85f;
        _endOfStateTime = Agent.AnimEngine[animName].length * 0.9f + Time.timeSinceLevelLoad;

        _rotationOk = _rotationTime == 0;
        _positionOK = false;

        Agent.BlackBoard.motionType = MotionType.ROLL;

        if (Agent.BlackBoard.showMotionEffect)
        {
            ParticleTools.Instance.Play(Agent.particleSystemRollTust, Agent.gameObject.ForwardRadian(), 0f);
        }        
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventRoll)
        {
            /*if (_eventRoll != null)
            {
                _eventRoll.IsFinished = true;
            }
            Initialize(ev);*/
            ev.IsFinished = true;
            return true;
        }       

        return false;
    }

    public override void OnUpdate()
    {
        if (_rotationOk == false)
        {
            _currentRotationTime += Time.deltaTime;

            if (_currentRotationTime >= _rotationTime)
            {
                _currentRotationTime = _rotationTime;
                _rotationOk = true;
            }

            float progress = _currentRotationTime / _rotationTime;
            Quaternion q = Quaternion.Lerp(_startRotation, _finalRotation, progress);
            Agent.Transform.rotation = q;
        }

        if (_positionOK == false)// && (RotationOk || (Quaternion.Angle(Owner.Transform.rotation, FinalRotation) > 40.0f))
        {
            _currentMoveTime += Time.deltaTime;
            if (_currentMoveTime >= _moveTime)
            {
                _currentMoveTime = _moveTime;
                _positionOK = true;
            }

            float progress = _currentMoveTime / _moveTime;
            Vector3 finalPos = Mathfx.Hermite(_startPosition, _finalPosition, progress);
            //MoveTo(finalPos);
            if (TransformTools.Instance.MoveOnGround(Agent.Transform, Agent.CharacterController,
                   finalPos - Agent.Transform.position, false) == false)
            {
                _positionOK = true;
                ParticleTools.Instance.Stop(Agent.particleSystemRollTust);
            }
        }

        if (_endOfStateTime <= Time.timeSinceLevelLoad)
        {
            IsFinished = true;
            _eventRoll.IsFinished = true;            
        }
    }

}