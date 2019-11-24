using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.UI;
using UnityEngine.UI;

public class ModelFace : Model
{
    AssetFace _assetFace;   // face模板数据

    public ModelFace(UIType uiType)
        : base((int)uiType)
    {
        
    }

    public override void Init()
    {
        _assetFace = AssetDataMgr.Instance.Get(AssetDataType.FACE) as AssetFace;
    }

    public override void OnUpdate()
    {

    }

    public List<GameObject> GetHeads()
    {
        List<GameObject> heads = new List<GameObject>();
        /*int unknownCount = 0;
        for (int i = AgentType.PLAYER; i < AgentType.Count; i++)
        {
            if (presentMgr.Instance.IsPresent(i))
            {
                agentTypeList.Add(i);
            }
            else
            {
                ++unknownCount;
            }
        }

        for (int i = 0; i < unknownCount; i++)
        {
            agentTypeList.Add(AgentType.NONE);
        }*/

        // 临时测试写法：
        heads.Add(CreateHead(AgentType.PLAYER));
        heads.Add(CreateHead(AgentType.PEASANT));
        heads.Add(CreateHead(AgentType.SWORD_MAN));
        heads.Add(CreateHead(AgentType.NONE));
        heads.Add(CreateHead(AgentType.NONE));

        return heads;
    }

    GameObject CreateHead(AgentType agentType)
    {
        FaceData faceData = _assetFace.Get(agentType);
        GameObject prefab = (AssetDataMgr.Instance.Get(AssetDataType.UI) as AssetUI).Get(UIType.FACE_HEAD).prefab;
        GameObject head = GameObject.Instantiate(prefab);
        head.name = ((int)agentType).ToString();
        head.GetComponent<Image>().sprite = faceData.head;
        return head;
    }

    public float GetDefend(string agentTypeStr)
    {
        AgentType agentType = (AgentType)int.Parse(agentTypeStr);
        return _assetFace.Get(agentType).defend;
    }

    public float GetSpeed(string agentTypeStr)
    {
        AgentType agentType = (AgentType)int.Parse(agentTypeStr);
        return _assetFace.Get(agentType).speed;
    }

    public float GetSword(string agentTypeStr)
    {
        AgentType agentType = (AgentType)int.Parse(agentTypeStr);
        return _assetFace.Get(agentType).sword;
    }

    public float GetStamina(string agentTypeStr)
    {
        AgentType agentType = (AgentType)int.Parse(agentTypeStr);
        return _assetFace.Get(agentType).stamina;
    }

    public float GetFoxy(string agentTypeStr)
    {
        AgentType agentType = (AgentType)int.Parse(agentTypeStr);
        return _assetFace.Get(agentType).foxy;
    }

    public GameObject GetTarget(string agentTypeStr)
    {
        AgentType agentType = (AgentType)int.Parse(agentTypeStr);
        GameObject prefab = _assetFace.Get(agentType).displayPrefab;
        if (prefab == null)
        {
            return null;
        }
        return GameObject.Instantiate(prefab);
    }
}
