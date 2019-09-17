using System.Collections.Generic;
using Phenix.Unity.AI;

public class AStarGOAP : AStar<GOAPAStarNode>
{
    public Phenix.Unity.AI.GOAPGoal goal;
    public List<Phenix.Unity.AI.GOAPAction> actions;

    protected override bool Arrived(GOAPAStarNode node)
    {
        if (node == null)
        {
            return false;
        }
        return goal.IsFinished(node.nodeWS);        
    }

    protected override void Neighbors(GOAPAStarNode node, ref List<GOAPAStarNode> neighbors)
    {
        neighbors.Clear();
        foreach (var action in actions)
        {
            if (node.adaptedAction == action)
            {
                continue;
            }
            if (action.CheckWorldStatePrecondition(node.nodeWS))
            {
                GOAPAStarNode newNode = GOAPAStarNode.pool.Get();
                newNode.adaptedAction = action;
                newNode.goalType = goal.GOAPGoalType;

                newNode.nodeWS = node.nodeWS.Clone();
                action.ApplyWorldStateEffect(newNode.nodeWS);

                neighbors.Add(newNode);
            }            
        }
    }

    protected override void Release(GOAPAStarNode node)
    {
        GOAPAStarNode.pool.Collect(node);
    }

    protected override int Cost(GOAPAStarNode node)
    {
        return node.Cost();
    }

    protected override int Precedence(GOAPAStarNode node)
    {
        return node.Precedence();
    }

    protected override bool IsSame(GOAPAStarNode a, GOAPAStarNode b)
    {
        if (a != null && b != null)
        {
            return (a.goalType == b.goalType && a.adaptedAction == b.adaptedAction);
        }
        return a == b;        
    }
}