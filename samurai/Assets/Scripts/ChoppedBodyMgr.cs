using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.Pattern;

public enum ChoppedBodyType
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
public class ChoppedBodyInfo
{
    public AgentType agentType; // agent类型
    public List<ChoppedBodyData> data = new List<ChoppedBodyData>();
}

[System.Serializable]
public class ChoppedBodyData
{
    public ChoppedBodyType choppedBodyType; // 尸块类型
    public List<GameObject> prefabs = new List<GameObject>();
}

public class ChoppedBodyMgr : Singleton<ChoppedBodyMgr>
{
    public List<ChoppedBodyInfo> choppedBodies = new List<ChoppedBodyInfo>();

    public GameObject Get(AgentType agentType, Transform trans, ChoppedBodyType choppedBodyType)
    {        
        for (int i = 0; i < choppedBodies.Count; ++i)
        {
            if (choppedBodies[i].agentType == agentType)
            {
                ChoppedBodyInfo bodyInfo = choppedBodies[i];
                for (int ii = 0; ii < bodyInfo.data.Count; ++ii)
                {
                    ChoppedBodyData bodyData = bodyInfo.data[ii];
                    if (bodyData.choppedBodyType == choppedBodyType)
                    {
                        if (bodyData.prefabs.Count == 0)
                        {
                            return null;
                        }

                        GameObject bodyPrefab = bodyData.prefabs[bodyData.prefabs.Count - 1];
                        if (bodyData.prefabs.Count > 1)
                        {
                            ChoppedBody choppedBody = bodyPrefab.GetComponent<ChoppedBody>();
                            choppedBody.Activate(trans);
                            bodyData.prefabs.Remove(bodyPrefab);                            
                        }
                        else if(bodyData.prefabs.Count > 0)
                        {
                            // 必须留一个用来Instantiate
                            bodyPrefab = GameObject.Instantiate(bodyPrefab);
                            ChoppedBody choppedBody = bodyPrefab.GetComponent<ChoppedBody>();                            
                            choppedBody.Activate(trans);
                        }
                        return bodyPrefab;
                    }                    
                }                
            }
        }       

        return null;
    }

    public void Collect(GameObject choppedBodyObject, AgentType agentType, ChoppedBodyType choppedBodyType)
    {
        if (choppedBodyObject == null)
        {
            return;
        }
        ChoppedBody choppedBody = choppedBodyObject.GetComponent<ChoppedBody>();
        choppedBody.Deactivate();

        for (int i = 0; i < choppedBodies.Count; ++i)
        {
            if (choppedBodies[i].agentType == agentType)
            {
                ChoppedBodyInfo bodyInfo = choppedBodies[i];
                for (int ii = 0; ii < bodyInfo.data.Count; ++ii)
                {
                    ChoppedBodyData bodyData = bodyInfo.data[ii];
                    if (bodyData.choppedBodyType == choppedBodyType)
                    {
                        bodyData.prefabs.Add(choppedBodyObject);
                    }
                }
            }
        }
    }
}
