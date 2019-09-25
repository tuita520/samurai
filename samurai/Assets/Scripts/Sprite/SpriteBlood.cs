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

    public static void Create(Transform trans)
    {
        Create(new Vector3(trans.localPosition.x, trans.localPosition.y + 0.5f, trans.localPosition.z),
                new Vector3(90, Random.Range(0, 180), 0));
    }
}
