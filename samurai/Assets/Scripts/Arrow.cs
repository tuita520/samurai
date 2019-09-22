using UnityEngine;
using Phenix.Unity.Collection;

public class Arrow
{
    public static Pool<Arrow> pools = new Pool<Arrow>(10);
    const float flyTime = 3; // 飞行时长（秒）

    Agent1 _agent;

    Transform _transform;
    float _damage;
    float _speed;
    float _hitMomentum;
    float _endTimer;
    bool _hit;    

    public void Init(Agent1 agent, GameObject prefab, Vector3 pos, Vector3 dir, 
        float damage, float speed, float hitMomentum)
    {
        _agent = agent;
        if (_transform == null)
        {
            _transform = GameObject.Instantiate(prefab).GetComponent<Transform>();
        }        
        _transform.position = pos;
        _transform.forward = dir;
        _damage = damage;
        _speed = speed;
        _hitMomentum = hitMomentum;
        _hit = false;
        _endTimer = Time.timeSinceLevelLoad + flyTime;        
    }

    public void OnUpdate()
    {
        if (IsFinished())
        {
            return;
        }

        _transform.position += _transform.forward * _speed * Time.deltaTime;
        _hit = HandleAttackResult.DoRangeDamage(_agent, _transform, _damage, _hitMomentum); 
    }

    public void Show()
    {
        _transform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _transform.gameObject.SetActive(false);
    }

    public bool IsFinished()
    {
        return _hit || Time.timeSinceLevelLoad >= _endTimer;
    }
}