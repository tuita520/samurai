using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.Collection;

public enum OrderType
{
    NONE = -1,
    MOVE,    
    ATTACK,
    ROLL,    
}

public class OrderDataMove : OrderData
{
    public Vector3 dir;

    public OrderDataMove() : base((int)OrderType.MOVE) { }

    public override void Reset()
    {
        dir = Vector3.zero;
    }
}

public class OrderDataAttack : OrderData
{
    public AttackType attackType;

    public OrderDataAttack() : base((int)OrderType.ATTACK) { }
}

public class OrderDataRoll : OrderData
{
    public OrderDataRoll() : base((int)OrderType.ROLL){ }
}

public class PlayerOrderPool
{
    public static Pool<OrderDataMove> moves = new Pool<OrderDataMove>(10, OrderData.Reset);
    public static Pool<OrderDataAttack> attacks = new Pool<OrderDataAttack>(10, OrderData.Reset);
    public static Pool<OrderDataRoll> rolls = new Pool<OrderDataRoll>(10, OrderData.Reset);    
}


public abstract class OrderData
{
    public int orderType;

    protected OrderData(int orderType)
    {
        this.orderType = orderType;
    }

    public virtual void Reset() { }

    public static void Reset(OrderData inst) { inst.Reset(); }
}

public class PlayerOrder : MonoBehaviour
{
    OrderData _topOrder = null;
    Queue<OrderData> _orders2 = new Queue<OrderData>(); // 从next order开始存储

    [SerializeField]
    int _maxCachedOrderCount = 2;

    public bool Add(OrderData order)
    {
        if (_maxCachedOrderCount <= 0)
        {
            return false;
        }

        if (_orders2.Count + (_topOrder == null ? 0 : 1) >= _maxCachedOrderCount)
        {
            return false;
        }

        if (_topOrder == null)
        {
            _topOrder = order;
            return true;
        }

        _orders2.Enqueue(order);
        return true;
    }

    public OrderData GetCurOrder()
    {
        return _topOrder;
    }

    public OrderData GetNextOrder()
    {
        if (_orders2.Count == 0)
        {
            return null;
        }
        return _orders2.Peek();
    }

    public OrderData Pop()
    {
        OrderData ret = _topOrder;
        if (_orders2.Count > 0)
        {
            _topOrder = _orders2.Dequeue();
        }
        else
        {
            _topOrder = null;
        }
        return ret;        
    }

    public List<OrderData> Clear()
    {
        List<OrderData> ret = new List<OrderData>();
        if (_topOrder != null)
        {
            ret.Add(_topOrder);            
            foreach (var item in _orders2)
            {
                ret.Add(item);
            }
            _topOrder = null;
            _orders2.Clear();
        }        
        return ret;
    }

    /*
    Queue<OrderData> _orders = new Queue<OrderData>();
    Queue<OrderData> _orders2 = new Queue<OrderData>(); // 为了获取_orders的第二个元素

    [SerializeField]
    int _maxCachedOrderCount = 2;

    public bool Add(OrderData order)
    {
        if (_orders.Count >= _maxCachedOrderCount)
        {
            return false;
        }
        _orders.Enqueue(order);
        if (_orders.Count > 1)
        {
            _orders2.Enqueue(order);
        }
        return true;
    }

    public OrderData GetCurOrder()
    {
        if (_orders.Count == 0)
        {
            return null;
        }
        return _orders.Peek();
    }    

    public OrderData GetNextOrder()
    {
        if (_orders2.Count == 0)
        {
            return null;
        }
        return _orders2.Peek();
    }

    public OrderData Pop()
    {
        if (_orders2.Count > 0)
        {
            _orders2.Dequeue();
        }
        if (_orders.Count > 0)
        {
            return _orders.Dequeue();
        }
        return null;
    }

    public void Clear()
    {
        _orders.Clear();
        _orders2.Clear();
    }*/

    /*
    public AgentAction GetInputAction()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // phenix 添加
        if (moveDir != Vector3.zero)
        {
            AgentActionMove moveAction = AgentActionFactory.Get(AgentActionType.MOVE, _owner) as AgentActionMove;
            moveAction.moveDir = moveDir;
            AddOrder(moveAction);            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            AgentActionRoll rollAction = AgentActionFactory.Get(AgentActionType.ROLL, _owner) as AgentActionRoll;
            rollAction.direction = transform.forward;
            rollAction.toTarget = null;
            AddOrder(rollAction);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            AgentActionAttackMelee attackMeleeAction = AgentActionFactory.Get(AgentActionType.ATTACK_MELEE, _owner) as AgentActionAttackMelee;
            attackMeleeAction.attackType = AttackType.X;            
            AddOrder(attackMeleeAction);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            AgentActionAttackMelee attackMeleeAction = AgentActionFactory.Get(AgentActionType.ATTACK_MELEE, _owner) as AgentActionAttackMelee;
            attackMeleeAction.attackType = AttackType.O;            
            AddOrder(attackMeleeAction);
        }

        if (_orders.Count > 0)
        {
            return _orders.Dequeue();
        }
        return null;
    }*/
}
