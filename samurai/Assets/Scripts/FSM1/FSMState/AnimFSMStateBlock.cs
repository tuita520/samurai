using UnityEngine;
using Phenix.Unity.AI;
using Phenix.Unity.Utilities;

public class AnimFSMStateBlock : AnimFSMState
{
    AnimFSMEventBlock _eventBlock;    

    Quaternion _finalRotation;
    Quaternion _startRotation;
    Vector3 _startPosition;
    Vector3 _finalPosition;
    float _curRotationTime;
    float _rotationTime;
    float _curMoveTime;
    float _moveTime;
    float _endOfStateTime;
    float _blockHoldEndTime;

    bool _rotationOK = false;
    bool _positionOK = false;

    BlockState _blockState;

    public AnimFSMStateBlock(Agent1 agent)
        : base((int)AnimFSMStateType.BLOCK, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        Agent.BlackBoard.motionType = MotionType.BLOCK;
        Agent.BlackBoard.moveDir = Vector3.zero;
        Agent.BlackBoard.speed = 0;
    }

    public override void OnExit()
    {
        Agent.BlackBoard.motionType = MotionType.NONE;
    }
       

    protected override void Initialize(FSMEvent ev = null)
    {
        _eventBlock = ev as AnimFSMEventBlock;

        string animName = Agent.AnimSet.GetBlockAnim(BlockState.START, Agent.BlackBoard.weaponSelected);

        _startRotation = Agent.Transform.rotation;
        _startPosition = Agent.Transform.position;

        Vector3 dir = _eventBlock.attacker.Position - Agent.Transform.position;
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
        _moveTime = 0;

        _rotationOK = _rotationTime == 0;
        _positionOK = _moveTime == 0;

        _curRotationTime = 0;
        _curMoveTime = 0;

        AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.05f);

        _endOfStateTime = Time.timeSinceLevelLoad + Agent.AnimEngine[animName].length * 0.9f;
        _blockHoldEndTime = Time.timeSinceLevelLoad + _eventBlock.holdTime;
        _blockState = BlockState.START;
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventBreakBlock)
        {
            if ((ev as AnimFSMEventBreakBlock).success)
            {
                InitializeBlockSuccess();
            }
            else
            {
                InitializeBlockFailed();
            }
            ev.IsFinished = true;
            AnimFSMEventBreakBlock.pool.Collect(ev as AnimFSMEventBreakBlock); // 这里可以直接回收
            ev = null;
            return true;
        }

        return false;
    }

    public override void OnUpdate()
    {
        UpdateFinalRotation();

        if (_rotationOK == false)
        {
            _curRotationTime += Time.deltaTime;
            if (_curRotationTime >= _rotationTime)
            {
                _curRotationTime = _rotationTime;
                _rotationOK = true;
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
            if (TransformTools.Instance.MoveOnGround(Agent.Transform, Agent.CharacterController,
                    finalPos - Agent.Transform.position, true) == false)
            {
                _positionOK = true;
            }
        }

        switch (_blockState)
        {
            case BlockState.START:
                if (_endOfStateTime <= Time.timeSinceLevelLoad)
                    InitializeHold();
                break;
            case BlockState.HOLD:
                if (_endOfStateTime <= Time.timeSinceLevelLoad)
                {
                    InitializeEnd();
                }
                /*else if (Agent.BlackBoard.blockResult == BlockResult.FAIL)
                {
                    InitializeBlockFailed();
                    Agent.BlackBoard.blockResult = BlockResult.NONE;
                }
                else if (Agent.BlackBoard.blockResult == BlockResult.SUCCESS)
                {
                    InitializeBlockSuccess();
                    Agent.BlackBoard.blockResult = BlockResult.NONE;
                }*/
                break;
            case BlockState.BLOCK_SUCCESS:                
                if (_endOfStateTime <= Time.timeSinceLevelLoad)
                {
                    if (Time.timeSinceLevelLoad < _blockHoldEndTime)
                        InitializeHold();
                    else
                        InitializeEnd();
                }
                break;
            case BlockState.END:
                if (_endOfStateTime <= Time.timeSinceLevelLoad)
                {
                    IsFinished = true;
                    _eventBlock.IsFinished = true;
                }
                break;
            case BlockState.BLOCK_FAIL:
                if (_endOfStateTime <= Time.timeSinceLevelLoad)
                {
                    IsFinished = true;
                    _eventBlock.IsFinished = true;
                }
                break;
        }
    }

    private void InitializeHold()
    {
        string animName = Agent.AnimSet.GetBlockAnim(BlockState.HOLD, Agent.BlackBoard.weaponSelected);
        AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.05f);        
        _endOfStateTime = _blockHoldEndTime;

        _blockState = BlockState.HOLD;
        Agent.BlackBoard.motionType = MotionType.BLOCK;
    }

    private void InitializeBlockSuccess()
    {
        string animName = Agent.AnimSet.GetBlockAnim(global::BlockState.BLOCK_SUCCESS, Agent.BlackBoard.weaponSelected);
        AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.05f);
        
        _startRotation = Agent.Transform.rotation;
        _startPosition = Agent.Transform.position;

        Vector3 dir = _eventBlock.attacker.Position - Agent.Transform.position;
        float angle = 0;

        if (dir.sqrMagnitude > 0.1f * 0.1f)
        {
            dir.Normalize();
            angle = Vector3.Angle(Agent.Transform.forward, dir);
        }
        else
            dir = Agent.Transform.forward;

        _finalRotation.SetLookRotation(dir);
        _finalPosition = _startPosition - dir * 0.75f;

        _rotationTime = angle / 500.0f;
        _moveTime = 0.1f;

        _rotationOK = _rotationTime == 0;
        _positionOK = _moveTime == 0;

        _curRotationTime = 0;
        _curMoveTime = 0;

        AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.05f);

        _endOfStateTime = Time.timeSinceLevelLoad + Agent.AnimEngine[animName].length * 0.9f;

        _blockState = BlockState.BLOCK_SUCCESS;        
        Agent.BlackBoard.motionType = MotionType.BLOCK;
    }


    private void InitializeBlockFailed()
    {        
        string animName = Agent.AnimSet.GetBlockAnim(BlockState.BLOCK_FAIL, Agent.BlackBoard.weaponSelected);
        AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.05f);        

        _startRotation = Agent.Transform.rotation;
        _startPosition = Agent.Transform.position;

        Vector3 dir = _eventBlock.attacker.Position - Agent.Transform.position;
        float angle = 0;

        if (dir.sqrMagnitude > 0.1f * 0.1f)
        {
            dir.Normalize();
            angle = Vector3.Angle(Agent.Transform.forward, dir);
        }
        else
            dir = Agent.Transform.forward;

        _finalRotation.SetLookRotation(dir);
        _finalPosition = _startPosition - dir * 2;

        _rotationTime = angle / 500.0f;
        _moveTime = Agent.AnimEngine[animName].length * 0.8f;

        _rotationOK = _rotationTime == 0;
        _positionOK = _moveTime == 0;

        _curRotationTime = 0;
        _curMoveTime = 0;

        AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.05f);

        _endOfStateTime = Time.timeSinceLevelLoad + Agent.AnimEngine[animName].length * 0.9f;

        _blockState = BlockState.BLOCK_FAIL;
        Agent.BlackBoard.motionType = MotionType.INJURY;
    }

    private void InitializeEnd()
    {
        string animName = Agent.AnimSet.GetBlockAnim(global::BlockState.END, Agent.BlackBoard.weaponSelected);
        AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.05f);

        _endOfStateTime = Time.timeSinceLevelLoad + Agent.AnimEngine[animName].length * 0.9f;
        Agent.BlackBoard.motionType = MotionType.NONE;

        _blockState = BlockState.END;
    }

    void UpdateFinalRotation()
    {
        if (_eventBlock.attacker == null)
            return;

        Vector3 dir = _eventBlock.attacker.Position - Agent.Position;
        dir.Normalize();

        _finalRotation.SetLookRotation(dir);
        _startRotation = Agent.Transform.rotation;
        float angle = Vector3.Angle(Agent.Transform.forward, dir);

        if (angle > 0)
        {
            _rotationTime = angle / (360.0f * Agent.BlackBoard.rotationSmooth);
            _rotationOK = false;
            _curRotationTime = 0;            
        }

        PlayRotateAnim(angle, dir);
    }

    void PlayRotateAnim(float angleToTarget, Vector3 targetDir)
    {
        if (Agent.BlackBoard.motionType != MotionType.BLOCK)
        {
            return;
        }
        string animName;
        if (angleToTarget > 0)
        {
            if (Vector3.Dot(targetDir, Agent.Transform.right) > 0)
                animName = Agent.AnimSet.GetRotateAnim(Agent.BlackBoard.motionType, RotationType.RIGHT);
            else
                animName = Agent.AnimSet.GetRotateAnim(Agent.BlackBoard.motionType, RotationType.LEFT);
            AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.01f, QueueMode.CompleteOthers);
        }        
        else
        {
            animName = Agent.AnimSet.GetBlockAnim(BlockState.HOLD, Agent.BlackBoard.weaponSelected);
            AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.01f);
        }
        
    }
}