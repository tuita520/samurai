using UnityEngine;
using Phenix.Unity.AI;

public class AnimFSMStatePlayAnim : AnimFSMState
{
    AnimFSMEventPlayAnim _eventPlayAnim;

    float _endOfStateTime;    
    Quaternion _finalRotation;
    Quaternion _startRotation;
    float _curRotationTime;
    float _rotationTime;
    bool _rotationOk = false;

    public AnimFSMStatePlayAnim(Agent1 agent)
        : base((int)AnimFSMStateType.PLAY_ANIM, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        Agent.BlackBoard.motionType = MotionType.ANIMATION_DRIVE;
        Agent.BlackBoard.moveDir = Vector3.zero;
        Agent.BlackBoard.speed = 0;
    }

    protected override void Initialize(FSMEvent ev = null)
    {
        _eventPlayAnim = ev as AnimFSMEventPlayAnim;
        _rotationOk = true;
        Phenix.Unity.Utilities.AnimationTools.PlayAnim(Agent.AnimEngine, _eventPlayAnim.animName, 0.05f);        
        _endOfStateTime = (Agent.AnimEngine[_eventPlayAnim.animName].length * 0.9f) + Time.timeSinceLevelLoad;
        Agent.BlackBoard.invulnerable = _eventPlayAnim.invulnerable;
    }

    public override void OnExit()
    {
        if (_eventPlayAnim.invulnerable)
        {
            Agent.BlackBoard.invulnerable = false;
        }
    }

    public override void OnUpdate()
    {
        if (_eventPlayAnim.lookAtTarget)
        {
            UpdateFinalRotation();
            if (_rotationOk == false)
            {
                _curRotationTime += Time.deltaTime;
                if (_curRotationTime >= _rotationTime)
                {
                    _curRotationTime = _rotationTime;
                    _rotationOk = true;
                }

                float progress = _curRotationTime / _rotationTime;
                Quaternion q = Quaternion.Lerp(_startRotation, _finalRotation, progress);
                Agent.Transform.rotation = q;
            }
        }

        if (_endOfStateTime <= Time.timeSinceLevelLoad)
        {
            IsFinished = true;
            _eventPlayAnim.IsFinished = true;
        }
    }

    void UpdateFinalRotation()
    {
        if (Agent.BlackBoard.desiredTarget == null)
            return;

        Vector3 dir = Agent.BlackBoard.desiredTarget.Position - Agent.Position;
        dir.Normalize();

        _finalRotation.SetLookRotation(dir);
        _startRotation = Agent.Transform.rotation;
        float angle = Vector3.Angle(Agent.Transform.forward, dir);

        if (angle > 0)
        {
            _rotationTime = angle / 100.0f;
            _rotationOk = false;
            _curRotationTime = 0;
        }
    }

}