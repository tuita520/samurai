using UnityEngine;
using Phenix.Unity.Sprite;
using Phenix.Unity.Collection;

public class SpriteBlood : SpriteBase
{
    public static Pool<SpriteBlood> pool = new Pool<SpriteBlood>(20, Reset);

    public override void Release()
    {
        pool.Collect(this);
    }

    public void Init(int spriteCode, float lifeTime, Material mat, Vector3 pos, Vector3 dir)
    {
        base.Init(spriteCode, lifeTime, mat, pos, dir);        
    }
}
