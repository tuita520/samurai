using UnityEngine;
using Phenix.Unity.AI;
using Phenix.Unity.Utilities;

public class AnimFSMStateInjuryBoss : AnimFSMState
{
    AnimFSMEventInjuryBoss _eventInjury;

    int _curInjuryPhaseIDX;
    float _endOfStateTime;

    public AnimFSMStateInjuryBoss(Agent1 agent)
        : base((int)AnimFSMStateType.INJURY_BOSS, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        Agent.BlackBoard.motionType = MotionType.INJURY;
        Agent.BlackBoard.moveDir = Vector3.zero;
        Agent.BlackBoard.speed = 0;
    }

    public override void OnExit()
    {
        Agent.BlackBoard.motionType = MotionType.NONE;
    }

    protected override void Initialize(FSMEvent ev = null)
    {
        _eventInjury = ev as AnimFSMEventInjuryBoss;

        _curInjuryPhaseIDX = 0;
        string animName = Agent.AnimSet.GetInjuryPhaseAnim(_curInjuryPhaseIDX);
        AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.1f);
        _endOfStateTime = Agent.AnimEngine[animName].length + Time.timeSinceLevelLoad;
        Agent.BlackBoard.motionType = MotionType.NONE;
    }

    void InitializeNext(FSMEvent ev)
    {      
        _eventInjury = ev as AnimFSMEventInjuryBoss;
        string animName = Agent.AnimSet.GetInjuryPhaseAnim(++_curInjuryPhaseIDX);
        AnimationTools.PlayAnim(Agent.AnimEngine, animName, 0.1f);
        _endOfStateTime = Agent.AnimEngine[animName].length + Time.timeSinceLevelLoad;
        Agent.BlackBoard.motionType = MotionType.NONE;
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventInjuryBoss)
        {
            InitializeNext(ev);
            return true;
        }
        return false;
    }

    public override void OnUpdate()
    {
        if (_endOfStateTime <= Time.timeSinceLevelLoad)
        {
            IsFinished = true;            
            _eventInjury.IsFinished = true;
        }
    }
}