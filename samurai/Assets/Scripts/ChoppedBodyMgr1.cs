using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.Pattern;
using Phenix.Unity.Collection;

public enum ChoppedBodyType1
{
    NONE = -1,
    LEGS = 0,
    BEHEADED,
    HALF_BODY,
    SLICE_FRONT_BACK,
    SLICE_LEFT_RIGHT,
    MAX,
}

[System.Serializable]
public class ChoppedBodyInfo1
{
    public AgentType agentType; // agent类型
    public List<ChoppedBodyData1> data = new List<ChoppedBodyData1>();
}

[System.Serializable]
public class ChoppedBodyData1
{
    public ChoppedBodyType1 choppedBodyType; // 尸块类型
    public GameObject prefab;

    [HideInInspector]
    public GameObjectPool pool;    
}

public class ChoppedBodyMgr1 : Singleton<ChoppedBodyMgr1>
{
    public List<ChoppedBodyInfo1> choppedBodies = new List<ChoppedBodyInfo1>();

    private void Start()
    {
        foreach (var bodyInfo in choppedBodies)
        {
            foreach (var bodyData in bodyInfo.data)
            {
                bodyData.pool = new GameObjectPool(5, bodyData.prefab);
            }            
        }
    }

    public void ShowChoppedBody(AgentType agentType, Transform trans, ChoppedBodyType1 choppedBodyType)
    {        
        for (int i = 0; i < choppedBodies.Count; ++i)
        {
            if (choppedBodies[i].agentType == agentType)
            {
                ChoppedBodyInfo1 bodyInfo = choppedBodies[i];
                for (int ii = 0; ii < bodyInfo.data.Count; ++ii)
                {
                    ChoppedBodyData1 bodyData = bodyInfo.data[ii];
                    if (bodyData.choppedBodyType == choppedBodyType)
                    {
                        ChoppedBody choppedBody = bodyData.pool.Get().GetComponent<ChoppedBody>();
                        choppedBody.Activate(trans);
                    }                    
                }                
            }
        }
    }

    public void Collect(GameObject choppedBodyObject, AgentType agentType, ChoppedBodyType1 choppedBodyType)
    {
        if (choppedBodyObject == null)
        {
            return;
        }
        
        for (int i = 0; i < choppedBodies.Count; ++i)
        {
            if (choppedBodies[i].agentType == agentType)
            {
                ChoppedBodyInfo1 bodyInfo = choppedBodies[i];
                for (int ii = 0; ii < bodyInfo.data.Count; ++ii)
                {
                    ChoppedBodyData1 bodyData = bodyInfo.data[ii];
                    if (bodyData.choppedBodyType == choppedBodyType)
                    {
                        bodyData.pool.Collect(choppedBodyObject);
                    }
                }
            }
        }
    }
}
