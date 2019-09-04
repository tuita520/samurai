using UnityEngine;

public class PlayerInput1 : MonoBehaviour
{
    Agent1 _agent;

    void Awake()
    {
        _agent = GetComponent<Agent1>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))                
        {            
            OrderDataRoll order = PlayerOrderPool.rolls.Get();
            if (_agent.PlayerOrder.Add(order) == false)
            {
                PlayerOrderPool.rolls.Collect(order);
            }
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            OrderDataAttack order = PlayerOrderPool.attacks.Get();
            order.attackType = AttackType.X;
            if (_agent.PlayerOrder.Add(order) == false)
            {
                PlayerOrderPool.attacks.Collect(order);
            }
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            OrderDataAttack order = PlayerOrderPool.attacks.Get();
            order.attackType = AttackType.O;
            if (_agent.PlayerOrder.Add(order) == false)
            {
                PlayerOrderPool.attacks.Collect(order);
            }
        }

        // move的检测必须放在roll和attack之后，否则roll和attack会由于PlayerOrder的_maxCachedOrderCount限制被move挤占掉
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveDir != Vector3.zero)
        {
            OrderDataMove order = PlayerOrderPool.moves.Get();
            order.dir = moveDir;
            if (_agent.PlayerOrder.Add(order) == false)
            {
                PlayerOrderPool.moves.Collect(order);
            }
        }
    }

}
