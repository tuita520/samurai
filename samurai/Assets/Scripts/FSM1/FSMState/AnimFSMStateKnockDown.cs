using UnityEngine;
using Phenix.Unity.AI;

public class AnimFSMStateKnockDown : AnimFSMState
{
    enum KnockdownStatus
    {
        FALL_DOWN,
        LYING,                
        GET_UP,
        DEATH,
    }

    AnimFSMEventKnockDown _eventKnockDown;
    AnimFSMEventDeath _eventDeath;

    Quaternion _finalRotation;
    Quaternion _startRotation;
    Vector3 _startPosition;
    Vector3 _finalPosition;
    float _curRotationTime;
    float _rotationTime;
    float _curMoveTime;
    float _moveTime;
    float _endOfStateTime;
    float _lyingEndTime;

    bool _rotationOk = false;
    bool _positionOK = false;

    KnockdownStatus _status;

    public AnimFSMStateKnockDown(Agent1 agent)
        : base((int)AnimFSMStateType.KNOCK_DOWN, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        Agent.BlackBoard.motionType = MotionType.KNOCK_DOWN;
        Agent.BlackBoard.moveDir = Vector3.zero;
        Agent.BlackBoard.speed = 0;
    }

    public override void OnExit()
    {
        Agent.BlackBoard.motionType = MotionType.NONE;
    }
       

    protected override void Initialize(FSMEvent ev = null)
    {
        _eventKnockDown = ev as AnimFSMEventKnockDown;

        string animName = Agent.AnimSet.GetKnockdowAnim(KnockdownState.Down, Agent.BlackBoard.weaponSelected);
        _startRotation = Agent.Transform.rotation;
        _startPosition = Agent.Transform.position;
        Vector3 dir = _eventKnockDown.attacker.Position - Agent.Transform.position;
        float angle = 0;

        if (dir.sqrMagnitude > 0.1f * 0.1f)
        {
            dir.Normalize();
            angle = Vector3.Angle(Agent.Transform.forward, dir);
        }
        else
            dir = Agent.Transform.forward;

        _finalRotation.SetLookRotation(dir);
        _rotationTime = angle / 500.0f;
        _finalPosition = _startPosition + _eventKnockDown.impuls;
        _moveTime = Agent.AnimEngine[animName].length * 0.4f;

        _rotationOk = (_rotationTime == 0);
        _positionOK = (_moveTime == 0);
        _curRotationTime = 0;
        _curMoveTime = 0;

        Phenix.Unity.Utilities.AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.05f);        

        _endOfStateTime = Time.timeSinceLevelLoad + Agent.AnimEngine[animName].length * 0.9f;
        _lyingEndTime = _endOfStateTime + _eventKnockDown.lyingTime;

        _status = KnockdownStatus.FALL_DOWN;
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventKnockDown)
        {
            (ev as AnimFSMEventKnockDown).IsFinished = true;            
            return true;
        }
        if (ev is AnimFSMEventDeath)
        {
            _eventDeath = ev as AnimFSMEventDeath;
            InitializeDeath();
            return true;
        }
        return false;
    }

    public override void OnUpdate()
    {
        if (_status == KnockdownStatus.DEATH)
            return;

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

        if (_positionOK == false)
        {
            _curMoveTime += Time.deltaTime;
            if (_curMoveTime >= _moveTime)
            {
                _curMoveTime = _moveTime;
                _positionOK = true;
            }

            float progress = _curMoveTime / _moveTime;
            Vector3 finalPos = Mathfx.Sinerp(_startPosition, _finalPosition, progress);
            if (Phenix.Unity.Utilities.TransformTools.MoveOnGround(Agent.Transform, Agent.CharacterController,
                finalPos - Agent.Transform.position, true) == false)
            {
                _positionOK = true;
            }
        }

        switch (_status)
        {
            case KnockdownStatus.FALL_DOWN:
                if (_endOfStateTime <= Time.timeSinceLevelLoad)
                    InitializeLying();
                break;
            case KnockdownStatus.LYING:
                if (_endOfStateTime <= Time.timeSinceLevelLoad)
                    InitializeGetUp();
                break;            
            case KnockdownStatus.GET_UP:
                if (_endOfStateTime <= Time.timeSinceLevelLoad)
                {
                    IsFinished = true;
                    if (_eventKnockDown != null)
                    {
                        _eventKnockDown.IsFinished = true;
                    }                    
                }
                break;
            case KnockdownStatus.DEATH:
                if (_endOfStateTime <= Time.timeSinceLevelLoad)
                {
                    IsFinished = true;
                    if (_eventDeath != null)
                    {
                        _eventDeath.IsFinished = true;
                        _eventDeath.Release();                        
                    }
                }
                break;
        }
    }

    void InitializeDeath()
    {
        string animName = Agent.AnimSet.GetKnockdowAnim(KnockdownState.Fatality, Agent.BlackBoard.weaponSelected);
        Phenix.Unity.Utilities.AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.1f);      
        _endOfStateTime = Time.timeSinceLevelLoad + Agent.AnimEngine[animName].length * 0.9f;
        _status = KnockdownStatus.DEATH;
        Agent.BlackBoard.motionType = MotionType.DEATH;
    }

    void InitializeLying()
    {
        string animName = Agent.AnimSet.GetKnockdowAnim(KnockdownState.Loop, Agent.BlackBoard.weaponSelected);
        Phenix.Unity.Utilities.AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.05f);
        _endOfStateTime = _lyingEndTime;
        _status = KnockdownStatus.LYING;
        Agent.DisableCollisions();
    }

    void InitializeGetUp()
    {
        string animName = Agent.AnimSet.GetKnockdowAnim(KnockdownState.Up, Agent.BlackBoard.weaponSelected);
        Phenix.Unity.Utilities.AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.05f);
        _endOfStateTime = Time.timeSinceLevelLoad + Agent.AnimEngine[animName].length * 0.9f;
        _status = KnockdownStatus.GET_UP;
        Agent.EnableCollisions();
    }

    void UpdateFinalRotation()
    {
        Vector3 dir = _eventKnockDown.attacker.Position - Agent.Position;
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