using UnityEngine;
using Phenix.Unity.AI;

public class AnimFSMStateInjury : AnimFSMState
{
    AnimFSMEventInjury _eventInjury;

    Quaternion _finalRotation;
    Quaternion _startRotation;

    float _rotationProgress;
    float _moveTime;
    float _currentMoveTime;
    bool _positionOK = false;
    bool _rotationOk = false;

    Vector3 _impuls;
    float _endOfStateTime;

    public AnimFSMStateInjury(Agent1 agent)
        : base((int)AnimFSMStateType.INJURY, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        Agent.BlackBoard.motionType = MotionType.NONE;
        Agent.BlackBoard.moveDir = Vector3.zero;
        Agent.BlackBoard.speed = 0;
    }

    public override void OnExit()
    {
        Agent.BlackBoard.motionType = MotionType.NONE;
    }

    protected override void Initialize(FSMEvent ev = null)
    {
        _eventInjury = ev as AnimFSMEventInjury;        

        string animName = Agent.AnimSet.GetInjuryAnim(_eventInjury.fromWeapon, _eventInjury.damageType);
        Phenix.Unity.Utilities.AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.1f);

        _endOfStateTime = Agent.AnimEngine[animName].length + Time.timeSinceLevelLoad;
        Agent.BlackBoard.motionType = MotionType.NONE;
        _moveTime = Agent.AnimEngine[animName].length * 0.5f;
        _currentMoveTime = 0;

        if (_eventInjury.attacker != null && Agent.IsPlayer == false)
        {
            Vector3 dir = _eventInjury.attacker.Position - Agent.Transform.position;
            dir.Normalize();
            _finalRotation.SetLookRotation(dir);
            _startRotation = Agent.Transform.rotation;
            _rotationProgress = 0;
            _rotationOk = false;
        }
        else
        {
            _rotationOk = true;
        }

        _impuls = _eventInjury.impuls * 10;
        _positionOK = _impuls == Vector3.zero;
        Agent.BlackBoard.motionType = MotionType.INJURY;
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventInjury)
        {
            if (_eventInjury != null)
                _eventInjury.IsFinished = true;
            Initialize(ev);
            return true;
        }        
        return false;
    }

    public override void OnUpdate()
    {
        if (_rotationOk == false)
        {
            _rotationProgress += Time.deltaTime * Agent.BlackBoard.rotationSmooth;

            if (_rotationProgress >= 1)
            {
                _rotationProgress = 1;
                _rotationOk = true;
            }

            _rotationProgress = Mathf.Min(_rotationProgress, 1);
            Quaternion q = Quaternion.Lerp(_startRotation, _finalRotation, _rotationProgress);
            Agent.Transform.rotation = q;
        }

        if (_positionOK == false)
        {
            _currentMoveTime += Time.deltaTime;
            if (_currentMoveTime >= _moveTime)
            {
                _currentMoveTime = _moveTime;
                _positionOK = true;
            }

            float progress = Mathf.Max(0, Mathf.Min(1.0f, _currentMoveTime / _moveTime));
            Vector3 impuls = Vector3.Lerp(_impuls, Vector3.zero, progress);
            if (Phenix.Unity.Utilities.TransformTools.Instance.MoveOnGround(Agent.Transform, 
                Agent.CharacterController, impuls * Time.deltaTime, true) == false)
            //if (MoveEx(impuls * Time.deltaTime) == false)
            {
                _positionOK = true;
            }
        }

        if (_endOfStateTime <= Time.timeSinceLevelLoad)
        {
            IsFinished = true;
            _eventInjury.IsFinished = true;
        }
    }

}