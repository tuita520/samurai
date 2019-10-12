using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;

public class AnimFSMStateAttackBow : AnimFSMState
{    
    AnimFSMEventAttackBow _eventAttack;
    
    Quaternion _finalRotation;
    Quaternion _startRotation;
    float _curRotationTime;
    float _rotationTime;
    float _endOfStateTime;
    float _fireTime; // 发射时间
    bool _rotationOk = false;

    public AnimFSMStateAttackBow(Agent1 agent)
        : base((int)AnimFSMStateType.ATTACK_BOW, agent)
    {

    }

    public override void OnExit()
    {
        Time.timeScale = 1.0f;
    }

    protected override void Initialize(FSMEvent ev)
    {
        _eventAttack = ev as AnimFSMEventAttackBow;
        
        _startRotation = Agent.Transform.rotation;
        _eventAttack.attackPhaseDone = false;
        _fireTime = 0;

        float angle = 0;
        if (_eventAttack.target != null)
        {
            Vector3 dir = _eventAttack.target.Position - Agent.Transform.position;
            if (dir.sqrMagnitude > 0.1f * 0.1f)
            {
                dir.Normalize();
                angle = Vector3.Angle(Agent.Transform.forward, dir);
            }
            else
            {
                dir = Agent.Transform.forward;
            }

            _finalRotation.SetLookRotation(dir);
            _rotationTime = angle / 180.0f;
        }
        else
        {            
            _finalRotation.SetLookRotation(_eventAttack.attackDir);
            _rotationTime = Vector3.Angle(Agent.Transform.forward, _eventAttack.attackDir) / 1040.0f;
        }        

        _rotationOk = (_rotationTime == 0);
        _curRotationTime = 0;

        _endOfStateTime = Time.timeSinceLevelLoad +
            Mathf.Max(Agent.AnimEngine[_eventAttack.animAttackData.animName].length / Agent.AnimEngine[_eventAttack.animAttackData.animName].speed,
            _eventAttack.animAttackData.attackEndTime);

        _fireTime = Time.timeSinceLevelLoad + _eventAttack.animAttackData.hitTime;
        Agent.BlackBoard.motionType = MotionType.ATTACK;

        AnimationTools.PlayAnim(Agent.AnimEngine, _eventAttack.animAttackData.animName, 0.1f);
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventAttackBow)
        {
            if (_eventAttack != null)
            {
                _eventAttack.IsFinished = true;
            }
            Initialize(ev);
            return true;
        }

        return false;
    }

    public override void OnUpdate()
    {
        UpdateFinalRotation();

        //Debug.DrawLine(Transform.position + Vector3.up * 1.5f, Transform.position + Vector3.up * 1.5f + Transform.forward * 5);

        if (_rotationOk == false)
        {
            //Debug.Log("rotate");
            _curRotationTime += Time.deltaTime;

            if (_curRotationTime >= _rotationTime)
            {
                _curRotationTime = _rotationTime;
                _rotationOk = true;
            }

            float progress = _curRotationTime / _rotationTime;
            Agent.Transform.rotation = Quaternion.Lerp(_startRotation, _finalRotation, progress);             
        }

        if (_fireTime > 0 && _fireTime <= Time.timeSinceLevelLoad)
        {
            _fireTime = 0;
            ArrowMgr.Instance.Spawn(Agent, Agent.Transform.position + Vector3.up * 1.5f, Agent.Transform.forward, 
                _eventAttack.animAttackData.hitDamage, 20, _eventAttack.animAttackData.hitMomentum);
            //Agent.SoundPlayHit();
        }

        if (_endOfStateTime <= Time.timeSinceLevelLoad)
        {
            IsFinished = true;
            _eventAttack.IsFinished = true;
        }
    }
    
    void UpdateFinalRotation()
    {
        if (_eventAttack.target == null)
            return;

        Vector3 dir = _eventAttack.target.Position - Agent.Position;
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