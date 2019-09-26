using UnityEngine;
using Phenix.Unity.Sprite;
using Phenix.Unity.Collection;

public class SpriteBloodBig : SpriteBase
{
    public static Pool<SpriteBloodBig> pool = new Pool<SpriteBloodBig>(20, Reset);

    float _scaleVal = 3;          // scale数值，0表示无效 
    float _scaleTime = 5;         // scale时长（秒），0表示持久 

    public override void Release()
    {
        pool.Collect(this);
    }

    public void Init(int spriteCode, float lifeTime, Material mat, Vector3 pos, Vector3 dir, 
        float scaleVal, float scaleTime)
    {
        base.Init(spriteCode,lifeTime, mat, pos, dir);
        _scaleVal = scaleVal;
        _scaleTime = scaleTime;        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_scaleVal > 0 && _scaleTime > 0)
        {
            Quad.transform.localScale = Vector3.Lerp(Vector3.one,
                    new Vector3(_scaleVal, _scaleVal, Quad.transform.localScale.z),
                    Mathf.Min(PassTime / _scaleTime, 1));
        }
    }
}
