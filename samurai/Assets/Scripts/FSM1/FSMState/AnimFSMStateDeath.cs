using UnityEngine;
using Phenix.Unity.AI;

public class AnimFSMStateDeath : AnimFSMState
{
    AnimFSMEventDeath _eventDeath;

    Vector3 _startPosition;
    Vector3 _finalPosition;
    Quaternion _finalRotation;
    Quaternion _startRotation;

    float _rotationProgress;
    float _moveTime;
    float _curMoveTime;
    bool _positionOK = false;
    bool _rotationOk = false;

    public AnimFSMStateDeath(Agent1 agent)
        : base((int)AnimFSMStateType.DEATH, agent)
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
        _eventDeath = ev as AnimFSMEventDeath;

        string animName = Agent.AnimSet.GetDeathAnim(_eventDeath.fromWeapon, _eventDeath.damageType);
        Phenix.Unity.Utilities.Tools.PlayAnimation(Agent.AnimEngine, animName, 0.1f);        

        Agent.BlackBoard.motionType = MotionType.NONE;
        _startPosition = Agent.Transform.position;

        if (_eventDeath.attacker != null)
        {
            _finalPosition = _startPosition + _eventDeath.attacker.Forward;

            _startRotation = Agent.Transform.rotation;
            _finalRotation.SetLookRotation(-_eventDeath.attacker.Forward);

            _positionOK = false;
            _rotationOk = false;

            _rotationProgress = 0;
        }
        else
        {
            _startPosition = Agent.Transform.position;
            _finalPosition = _startPosition + _eventDeath.impuls;

            _positionOK = false;
            _rotationOk = true;
        }

        _curMoveTime = 0;
        _moveTime = Agent.AnimEngine[animName].length * 0.6f;

        //Owner.Invoke("SpawnBlood", AnimEngine[animName].length);
        Agent.BlackBoard.motionType = MotionType.DEATH;
        Agent.DisableCollisions();
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
            _curMoveTime += Time.deltaTime;
            if (_curMoveTime >= _moveTime)
            {
                _curMoveTime = _moveTime;
                _positionOK = true;
            }

            float progress = Mathf.Min(1.0f, _curMoveTime / _moveTime);
            Vector3 finalPos = Mathfx.Sinerp(_startPosition, _finalPosition, progress);
            
            if (Phenix.Unity.Utilities.TransformTools.MoveOnGround(Agent.Transform, Agent.CharacterController, 
                finalPos - Agent.Transform.position, true) == false)
            {
                _positionOK = true;
            }
        }

        if (_rotationOk && _positionOK)
        {
            _eventDeath.IsFinished = true;
            _eventDeath.Release();            
            HandleDeadBody.DoHandleDeadBody(Agent);
        }
    }

}