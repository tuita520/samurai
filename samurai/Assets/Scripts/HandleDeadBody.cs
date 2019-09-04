using UnityEngine;

public class RespawnData
{
    public Agent1 agent;

    public RespawnData(Agent1 agent) { this.agent = agent; }
}

public class HandleDeadBody
{
    public static void DoHandleDeadBody(Agent1 agent)
    {
        //FSMComponent fsm = respawnData.agent.GetComponent<FSMComponent>();
        //fsm.enabled = false;
        //respawnData.agent.Invoke("ReturnHuman", 延时)
        //respawnData.agent.gameObject.SetActive(false); 
        agent.FSMComponent.enabled = false;
        agent.Decision.enabled = false;
        if (agent.IsPlayer)
        {
            //respawnData.agent.PlayerOrder.enabled = false;
            //respawnData.agent.PlayerInput.enabled = false;
        }
    }    
}