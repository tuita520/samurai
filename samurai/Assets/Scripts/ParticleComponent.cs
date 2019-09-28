using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.Particle;
using Phenix.Unity.Pattern;

[System.Serializable]
public class ParticleProp
{
    public ParticleType particleType;   // 类型
    public GameObject prefab;
    public int poolCapacity;
}

public class ParticleComponent : Singleton<ParticleComponent>
{
    ParticleMgr _particleMgr = new ParticleMgr();

    [SerializeField]
    List<ParticleProp> _particlePropList = new List<ParticleProp>();

    ParticleProp GetProp(ParticleType particleType)
    {
        foreach (var item in _particlePropList)
        {
            if (item.particleType == particleType)
            {
                return item;
            }
        }
        return null;
    }

    private void Start()
    {
        foreach (var item in _particlePropList)
        {            
            _particleMgr.Add((int)item.particleType, item.prefab, item.poolCapacity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _particleMgr.OnUpdate();
    }

    public void Play(ParticleType particleType, Vector3 pos, Vector3 dir)
    {
        ParticleProp prop = GetProp(particleType);
        if (prop == null)
        {
            return;
        }

        _particleMgr.Play((int)particleType, pos, dir);
    }
}
