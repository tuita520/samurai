using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.Sprite;
using Phenix.Unity.Pattern;

[System.Serializable]
public class SpriteProp
{
    public SpriteType spriteType;       // 类型
    public float lifeTime = 0;          // 存在时长（秒），0表示持久
    public Material[] materials;        // 材质（使用时任选其一）
    public float scaleVal = 3;          // scale数值，0表示无效 
    public float scaleTime = 5;         // scale时长（秒），0表示持久 
}

public class SpriteComponent : Singleton<SpriteComponent>
{
    SpriteMgr _spriteMgr = new SpriteMgr();

    [SerializeField]
    List<SpriteProp> _spritePropList = new List<SpriteProp>();
    
    SpriteProp GetProp(SpriteType spriteType)
    {
        foreach (var item in _spritePropList)
        {
            if (item.spriteType == spriteType)
            {
                return item;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        _spriteMgr.OnUpdate();
    }

    public void CreateSprite(SpriteType spriteType, Vector3 pos, Vector3 dir)
    {
        SpriteProp prop = GetProp(spriteType);
        if (prop == null)
        {
            return;
        }

        SpriteBase sprite = null;
        switch (prop.spriteType)
        {
            case SpriteType.BLOOD:
                sprite = SpriteBlood.pool.Get();
                (sprite as SpriteBlood).Init((int)prop.spriteType, prop.lifeTime, prop.materials[Random.Range(0, prop.materials.Length)],
                    pos, dir);
                break;
            case SpriteType.BLOOD_BIG:
                sprite = SpriteBloodBig.pool.Get();
                (sprite as SpriteBloodBig).Init((int)prop.spriteType, prop.lifeTime, prop.materials[Random.Range(0, prop.materials.Length)],
                    pos, dir, prop.scaleVal, prop.scaleTime);
                break;
            default:
                break;
        }

        if (sprite != null)
        {
            _spriteMgr.Add(sprite);
        }
    }
}
