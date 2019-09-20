using Phenix.Unity.Collection;

public class GOAPAStarNode
{
    public static Pool<GOAPAStarNode> pool = new Pool<GOAPAStarNode>();

    public static int[,] precedenceMap = new int[/*ActionType*/,/*GoalType*/]
    {
        /*     action↓ \ goal→       ALERT, AVOID_LEFT, AVOID_RIGHT, PRESS, RETREAT, ATTACK_TARGET, SHOW, REACT_TO_DAMAGE, IDLE, ORDER_MOVE, ORDER_ATTACK, ORDER_DODGE, CALM, BLOCK   */
        /*GOTO_MELEE_RANGE*/         {   100,    100,          100,      100,    100,     10,          100,     100,         100,        100,          100,         100,   100,   100    },
        /*COMBAT_RUN_FORWARD*/       {   100,    100,          100,      100,    100,     100,         100,     100,         100,        100,          100,         100,   100,   100    },
        /*COMBAT_RUN_BACKWARD*/      {   100,    100,          100,      100,    100,     100,         100,     100,         100,        100,          100,         100,   100,   100    },
        /*COMBAT_MOVE_FORWARD*/      {   100,    100,          100,      0,      100,     100,         100,     100,         100,        100,          100,         100,   100,   100    },
        /*COMBAT_MOVE_BACKWARD*/     {   100,    100,          100,      100,    0,       100,         100,     100,         100,        100,          100,         100,   100,   100    },
        /*COMBAT_MOVE_LEFT*/         {   100,    0,            100,      100,    100,     100,         100,     100,         100,        100,          100,         100,   100,   100    },
        /*COMBAT_MOVE_RIGHT*/        {   100,    100,          0,        100,    100,     100,         100,     100,         100,        100,          100,         100,   100,   100    },
        /*LOOK_AT_TARGET*/           {   0,      10,           10,       10,     10,      20,          100,     100,         100,        100,          100,         100,   100,   10     },
        /*SHOW_SWORD*/               {   10,     20,           20,       20,     20,      30,          10,      100,         100,        100,          10,          10,    100,   20     },
        /*ATTACK_MELEE_MULTI_SWORDS*/{   100,    100,          100,      100,    100,     1,           100,     100,         100,        100,          100,         100,   100,   100    },
        /*ATTACK_WHIRL*/             {   100,    100,          100,      100,    100,     0,           100,     100,         100,        100,          100,         100,   100,   100    },
        /*REACT_TO_DAMAGE*/          {   100,    100,          100,      100,    100,     100,         100,     0,           100,        100,          100,         100,   100,   100    },
        /*PLAY_ANIM*/                {   100,    100,          100,      100,    100,     100,         0,       100,         100,        100,          100,         100,   100,   100    },
        /*IDLE*/                     {   20,     30,           30,       30,     30,      40,          20,      100,         0,          100,          20,          20,    10,    30     },
        /*ORDER_MOVE*/               {   100,    100,          100,      100,    100,     100,         100,     100,         100,        0,            100,         100,   100,   100    },
        /*ORDER_ATTACK_MELEE*/       {   100,    100,          100,      100,    100,     100,         100,     100,         100,        100,          0,           100,   100,   100    },
        /*ORDER_ROLL*/               {   100,    100,          100,      100,    100,     100,         100,     100,         100,        100,          100,         0,     100,   100    },
        /*HIDE_SWORD*/               {   100,    100,          100,      100,    100,     100,         100,     100,         100,        100,          100,         100,   0,     100    },
        /*BLOCK*/                    {   100,    100,          100,      100,    100,     100,         100,     100,         100,        100,          100,         100,   100,   0      },
        /*ATTACK_COUNTER*/           {   100,    100,          100,      100,    100,     0,           100,     100,         100,        100,          100,         100,   100,   100    },
        /*ATTACK_BERSERK*/           {   100,    100,          100,      100,    100,     0,           100,     100,         100,        100,          100,         100,   100,   100    },
        /*ATTACK_CROSS*/             {   100,    100,          100,      100,    100,     1,           100,     100,         100,        100,          100,         100,   100,   100    },
        /*ATTACK_ROLL*/              {   100,    100,          100,      100,    100,     0,           100,     100,         100,        100,          100,         100,   100,   100    },
        /*GOTO_TARGET*/              {   100,    100,          100,      100,    100,     10,          100,     100,         100,        100,          100,         100,   100,   100    },
        /*FLASH*/                    {   100,    100,          100,      100,    100,     5,           100,     100,         100,        100,          100,         100,   100,   100    },
        /*REACT_TO_DAMAGE_BOSS*/     {   100,    100,          100,      100,    100,     100,         100,     0,           100,        100,          100,         100,   100,   100    },
        /*ATTACK_MELEE_SINGLE_SWORD*/{   100,    100,          100,      100,    100,     1,           100,     100,         100,        100,          100,         100,   100,   100    },
        /*LOOK_AT_TARGET_MOVE*/      {   0,      10,           10,       10,     10,      20,          100,     100,         100,        100,          100,         100,   100,   10     },
        /*CHASE*/                    {   100,    100,          100,      100,    100,     15,          100,     100,         100,        100,          100,         100,   100,   100    },
        /*ATTACK_SAMURAI*/           {   100,    100,          100,      100,    100,     1,           100,     100,         100,        100,          100,         100,   100,   100    },
        /*ATTACK_JUMP*/              {   100,    100,          100,      100,    100,     0,           100,     100,         100,        100,          100,         100,   100,   100    },
        /*ROLL_FOR_BACK_STRIKE*/     {   100,    100,          100,      100,    100,     15,          100,     100,         100,        100,          100,         100,   100,   100    },
        /*ROLL_FOR_DODGE*/           {   100,    100,          100,      100,    0,       100,         100,     100,         100,        100,          100,         100,   100,   100    },        
    };        

    public Phenix.Unity.AI.WorldState nodeWS;
    public Phenix.Unity.AI.GOAPAction adaptedAction;
    public int goalType;
    //public List<Phenix.Unity.AI.GOAPAction> actions;

    public int Cost()
    {
        return 0;
    }

    public int Precedence()
    {
        return precedenceMap[(int)adaptedAction.GOAPActionType, (int)goalType];
    }
        
    /*public static bool operator <(GOAPAStarNode a, GOAPAStarNode b)
    {
        if (a == null || b == null)
        {
            return false;
        }
        return a.Cost() + a.Precedence() < b.Cost() + b.Precedence();
    }

    public static bool operator >(GOAPAStarNode a, GOAPAStarNode b)
    {
        if (a == null || b == null)
        {
            return false;
        }
        return a.Cost() + a.Precedence() > b.Cost() + b.Precedence();
    }
    
    public static bool operator ==(GOAPAStarNode a, GOAPAStarNode b)
    {
        if (a == null)
            return b == null;
        return a.Equals(b);
    }

    public static bool operator !=(GOAPAStarNode a, GOAPAStarNode b)
    {
        return !(a == b);
    }

    public override bool Equals(System.Object o)
    {
        GOAPAStarNode other = o as GOAPAStarNode;
        if (other == null)
        {
            return false;
        }
        return (goalType == other.goalType && adaptedAction == other.adaptedAction);
    }

    public override int GetHashCode()
    {
        return (this as object).GetHashCode();
    }*/
}
