using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : MonoBehaviour
{
    protected Agent1 Agent { get; private set; }

    public virtual void Reset() { }

    protected virtual void Awake()
    {
        Agent = GetComponent<Agent1>();                
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    protected void UpdateTarget(float distanceRange, List<Agent1> enemies)
    {
        Agent.BlackBoard.desiredTarget = SelectTarget(distanceRange, enemies);
    }

    public virtual Agent1 SelectTarget(float distanceRange, List<Agent1> agents)
    {        
        Agent1 ret = null;
        float maxVal = -1;
        
        for (int i = 0; i < agents.Count; i++)
        {
            float val = 0;
            Agent1 agent = agents[i];

            if (agent.group == Agent.group || agent == Agent)
            {
                continue;
            }

            if (agent.BlackBoard.IsAlive == false)
            {
                continue;
            }

            Vector3 dirToTarget = (agent.Position - Agent.Position);
            float distance = dirToTarget.magnitude;
            dirToTarget.Normalize();

            if (distance > distanceRange)
                continue;

            // 当前目标
            if (agent == Agent.BlackBoard.desiredTarget)
                val += 0.1f;

            // 面对的敌人
            float angle = Vector3.Angle(dirToTarget, Agent.Forward);
            val += 0.2f - ((angle / 180.0f) * 0.2f);

            // 当前目标方向的敌人
            angle = Vector3.Angle(dirToTarget, Agent.BlackBoard.desiredDirection);
            val += 0.5f - ((angle / 180.0f) * 0.5f);

            // 距离近的
            val += 0.2f - ((distance / 5) * 0.2f);

            if (val > maxVal)
            {
                maxVal = val;
                ret = agent;
            }
        }

        return ret;
    }

    public virtual Vector3 GetBestAttackStart(Agent1 target)
    {
        Vector3 dirToTarget = target.Position - Agent.Position;
        dirToTarget.Normalize();

        return target.Position - dirToTarget * Agent.BlackBoard.weaponRange;
    }

    public Agent1 GetNearestAgent(Vector3 lineStart, Vector3 lineEnd, float radius, 
        List<Agent1> agents, bool ignoreSameGroup = false, AgentType agentType = AgentType.NONE)
    {
        float len;
        float nearestLen = radius;
        Agent1 best = null;

        for (int i = 0; i < agents.Count; i++)
        {
            Agent1 e = agents[i];

            if (e == Agent)
            {
                continue;
            }

            if (ignoreSameGroup && e.group == Agent.group)
            {
                continue;
            }

            if (e.BlackBoard.IsAlive == false)
            {
                continue;
            }

            if (agentType != AgentType.NONE && agentType != e.agentType)
            {
                continue;
            }

            len = Mathfx.DistanceFromPointToVector(lineStart, lineEnd, e.Position);
            if (len < nearestLen)
            {                
                nearestLen = len;
                best = e;
            }
        }

        return best;
    }

    public Agent1 GetNearestAgent(Vector3 center, float radius, List<Agent1> agents,
        bool ignoreSameGroup = false, AgentType agentType = AgentType.NONE)
    {
        float len;
        float nearestLen = radius * radius;
        Agent1 best = null;
        for (int i = 0; i < agents.Count; i++)
        {
            Agent1 e = agents[i];

            if (e == Agent)
            {
                continue;
            }

            if (ignoreSameGroup && e.group == Agent.group)
            {
                continue;
            }

            if (e.BlackBoard.IsAlive == false)
            {
                continue;
            }

            if (agentType != AgentType.NONE && agentType != e.agentType)
            {
                continue;
            }

            len = (center - e.Position).sqrMagnitude;
            if (len < nearestLen)
            {                
                nearestLen = len;
                best = e;
            }
        }
        return best;
    }
    public Agent1 GetNearestAgent(Direction direction, float maxRadius, List<Agent1> agents, 
        bool ignoreSameGroup = false, AgentType agentType = AgentType.NONE)
    {
        Vector3 dir;

        if (direction == Direction.Forward)
            dir = Agent.Forward;
        else if (direction == Direction.Backward)
            dir = -Agent.Forward;
        else if (direction == Direction.Right)
            dir = Agent.Right;
        else if (direction == Direction.Left)
            dir = -Agent.Right;
        else
            return null;

        return GetNearestAgent(Agent.Position + dir, Agent.Position + dir * 3, maxRadius, 
            agents, ignoreSameGroup, agentType);
    }

    public virtual void OnDead()
    {
        Agent.BlackBoard.damageResultType = DamageResultType.DEATH;
    }

    public virtual void OnInjury()
    {
        Agent.BlackBoard.damageResultType = DamageResultType.INJURY;
    }

    public virtual void OnKnockDown()
    {
        Agent.BlackBoard.damageResultType = DamageResultType.KNOCK_DOWN;
    }
}
