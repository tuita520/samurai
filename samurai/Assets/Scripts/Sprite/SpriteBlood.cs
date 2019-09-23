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

    public static void Create(Vector3 pos, Vector3 dir)
    {
        SpriteBlood blood = pool.Get();
        blood.Init((int)SpriteType.BLOOD, pos, dir);
        SpriteMgr.Instance.Add(blood);
    }
}
