using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;

public class AnimFSMStateCombatMove : AnimFSMState
{
    AnimFSMEventCombatMove _eventCombatMove;
    float _maxSpeed;
    float _movedDistance;

    Quaternion _finalRotation = new Quaternion();
    Quaternion _startRotation = new Quaternion();

    float _rotationProgress;

    public AnimFSMStateCombatMove(Agent1 agent)
        : base((int)AnimFSMStateType.COMBAT_MOVE, agent)
    {

    }

    public override void OnExit()
    {
        Agent.BlackBoard.speed = 0;             
    }

    public override void OnUpdate()
    {
        UpdateFinalRotation();

        _rotationProgress += Time.deltaTime * Agent.BlackBoard.rotationSmoothInMove;
        _rotationProgress = Mathf.Min(_rotationProgress, 1);
        Quaternion q = Quaternion.Slerp(_startRotation, _finalRotation, _rotationProgress);
        Agent.Transform.rotation = q;

        if (Quaternion.Angle(q, _finalRotation) > 40.0f)
            return;

        float curSmooth = Agent.BlackBoard.speedSmooth * Time.deltaTime;
        Agent.BlackBoard.speed = Mathf.Lerp(Agent.BlackBoard.speed, _maxSpeed, curSmooth);        

        float dist = Agent.BlackBoard.speed * Time.deltaTime;

        if (TransformTools.MoveOnGround(Agent.transform, Agent.CharacterController, Agent.BlackBoard.moveDir * dist, true))
        {
            _movedDistance += dist;
            if (_movedDistance > _eventCombatMove.totalMoveDistance)
            {
                IsFinished = true;
                _eventCombatMove.IsFinished = true;
            }
        }
        else if (_eventCombatMove.minDistanceToTarget > Agent.BlackBoard.DistanceToDesiredTarget)            
        {
            IsFinished = true;
            _eventCombatMove.IsFinished = true;
        }
        else
        {
            IsFinished = true;
        }
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventCombatMove)
        {
            if (_eventCombatMove != null)
            {
                _eventCombatMove.IsFinished = true;
            }
            Initialize(ev);
            return true;
        }

        if (ev is AnimFSMEventIdle)
        {
            (ev as AnimFSMEventIdle).IsFinished = true;
            IsFinished = true;
            return true;
        }

        if (ev is AnimFSMEventShowHideSword)
        {
            ev.IsFinished = true;
            UpdateMoveType();
            return true;
        }

        return false;
    }

    protected override void Initialize(FSMEvent ev)
    {
        _eventCombatMove = ev as AnimFSMEventCombatMove;
        UpdateFinalRotation();
        Agent.BlackBoard.motionType = MotionType.WALK;
        _rotationProgress = 0;
        _movedDistance = 0;
        Agent.BlackBoard.moveType = MoveType.NONE;
        UpdateMoveType();

        if (_eventCombatMove.moveType == MoveType.FORWARD)
            Agent.BlackBoard.moveDir = Agent.Forward;
        else if (_eventCombatMove.moveType == MoveType.BACKWARD)
            Agent.BlackBoard.moveDir = -Agent.Forward;
        else if (_eventCombatMove.moveType == MoveType.RIGHTWARD)
            Agent.BlackBoard.moveDir = Agent.Right;
        else if (_eventCombatMove.moveType == MoveType.LEFTWARD)
            Agent.BlackBoard.moveDir = -Agent.Right;

        if (Agent.IsPlayer == false && Agent.BlackBoard.moveType == MoveType.BACKWARD)
        {
            Agent.BlackBoard.Fear = Agent.BlackBoard.minFear;
        }
    }

    void UpdateFinalRotation()
    {
        if (_eventCombatMove.target == null)
            return;

        Vector3 dir = _eventCombatMove.target.Position - Agent.Position;
        dir.Normalize();
        _finalRotation.SetLookRotation(dir);
        _startRotation = Agent.Transform.rotation;

        if (_startRotation != _finalRotation)
            _rotationProgress = 0;
    }

    void UpdateMoveType()
    {
        Agent.BlackBoard.moveType = _eventCombatMove.moveType;
        AnimationTools.PlayAnim(Agent.AnimEngine, Agent.AnimSet.GetMoveAnim(_eventCombatMove.motionType, Agent.BlackBoard.moveType, 
            Agent.BlackBoard.weaponSelected, Agent.BlackBoard.weaponState), 0.2f);
        _maxSpeed = _eventCombatMove.motionType == MotionType.RUN ? Agent.BlackBoard.maxRunSpeed : 
            Agent.BlackBoard.maxCombatMoveSpeed;
    }
}