using UnityEngine;
using Phenix.Unity.Sprite;
using Phenix.Unity.Collection;

public class SpriteBloodBig : SpriteBase
{
    public static Pool<SpriteBloodBig> pool = new Pool<SpriteBloodBig>(20, Reset);

    const float _scaleVal = 3;          // scale数值，0表示无效 
    const float _scaleTime = 3;         // scale时长（秒），0表示持久 

    public override void Release()
    {
        pool.Collect(this);
    }

    public static void Create(Vector3 pos, Vector3 dir)
    {
        SpriteBloodBig bloodBig = pool.Get();
        bloodBig.Init((int)SpriteType.BLOOD_BIG, pos, dir);
        SpriteMgr.Instance.Add(bloodBig);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_scaleVal > 0 && _scaleTime > 0)
        {
            QuadObject.transform.localScale = Vector3.Lerp(Vector3.one,
                    new Vector3(_scaleVal, _scaleVal, QuadObject.transform.localScale.z),
                    Mathf.Min(PassTime / _scaleTime, 1));
        }
    }
}
