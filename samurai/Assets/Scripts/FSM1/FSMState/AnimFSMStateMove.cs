using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;

public class AnimFSMStateMove : AnimFSMState
{
    AnimFSMEventMove _eventMove;

    Quaternion _finalRotation = new Quaternion();
    Quaternion _startRotation = new Quaternion();
    float _rotationProgress;

    public AnimFSMStateMove(Agent1 agent)
        : base((int)AnimFSMStateType.MOVE, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        PlayAnim(GetMoveMotionType());
    }

    public override void OnExit()
    {        
        Agent.BlackBoard.speed = 0;
    }

    protected override void Initialize(FSMEvent ev)
    {
        _eventMove = ev as AnimFSMEventMove;
        Agent.BlackBoard.desiredDirection = _eventMove.moveDir;
        _finalRotation.SetLookRotation(Agent.BlackBoard.desiredDirection);
        _startRotation = Agent.Transform.rotation;
        Agent.BlackBoard.motionType = GetMoveMotionType();
        _rotationProgress = 0;
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventMove)
        {
            //if (_eventMove != null)
            //{
            //    _eventMove.IsFinished = true;
            //}
            Initialize(ev);            
            return true;
        }

        return false;
    }

    public override void OnUpdate()
    {
        _rotationProgress += Time.deltaTime * Agent.BlackBoard.rotationSmooth;
        _rotationProgress = Mathf.Min(_rotationProgress, 1);
        Quaternion q = Quaternion.Slerp(_startRotation, _finalRotation, _rotationProgress);
        Agent.Transform.rotation = q;

        if (Quaternion.Angle(q, _finalRotation) > 40.0f)
            return;

        float maxSpeed = Mathf.Max(Agent.BlackBoard.maxWalkSpeed, Agent.BlackBoard.maxRunSpeed * 
            Agent.BlackBoard.moveSpeedModifier);
        float curSmooth = Agent.BlackBoard.speedSmooth * Time.deltaTime;
        Agent.BlackBoard.speed = Mathf.Lerp(Agent.BlackBoard.speed, maxSpeed, curSmooth);
        Agent.BlackBoard.moveDir = Agent.BlackBoard.desiredDirection;

        // MOVE
        if (TransformTools.MoveOnGround(Agent.Transform, Agent.CharacterController,
                Agent.BlackBoard.moveDir * Agent.BlackBoard.speed * Time.deltaTime, true) == false)                
        {
            IsFinished = true;
            _eventMove.IsFinished = true;
        }

        MotionType motion = GetMoveMotionType();        
        if (motion != Agent.BlackBoard.motionType)
            PlayAnim(motion);
    }

    void PlayAnim(MotionType motion)
    {
        Agent.BlackBoard.motionType = motion;        
        string animName = Agent.AnimSet.GetMoveAnim(
            Agent.BlackBoard.motionType, MoveType.FORWARD, Agent.BlackBoard.weaponSelected,
            Agent.BlackBoard.weaponState);
        Tools.PlayAnimation(Agent.AnimEngine, animName, 0.2f);
    }

    private MotionType GetMoveMotionType()
    {
        if (Agent.BlackBoard.speed > Agent.BlackBoard.maxRunSpeed * 1.5f)
            return MotionType.SPRINT;
        else if (Agent.BlackBoard.speed > Agent.BlackBoard.maxWalkSpeed * 1.5f)
            return MotionType.RUN;

        return MotionType.WALK;
    }

}