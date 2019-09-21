using Phenix.Unity.AI;
using System.Collections.Generic;

public abstract class GOAPGoalBase : Phenix.Unity.AI.GOAPGoal
{
    //Agent1 _target = null;
    protected Agent1 Agent { get; private set; }    

    protected GOAPGoalBase(int goalType, Agent1 agent, List<WorldStateBitDataGoal> goalProps) 
        : base(goalType, goalProps)
    {
        Agent = agent;
    }

    public override void Reset()
    {
        //_target = null;
    }

    public override void OnEnter(Phenix.Unity.AI.WorldState ws)
    {
        base.OnEnter(ws);
        //_target = Agent.BlackBoard.desiredTarget;
    }

    public override bool IsAborted()
    {
        return false;
        //if (_target == null || _target.BlackBoard.IsAlive == false)
        //{            
        //    return true;
        //}
        
        //return _target != Agent.BlackBoard.desiredTarget;
    }
}