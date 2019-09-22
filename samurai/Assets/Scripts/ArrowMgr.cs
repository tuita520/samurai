using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.Pattern;

public class ArrowMgr : Singleton<ArrowMgr>
{
    [SerializeField]
    GameObject _arrowPrefab;

    List<Arrow> _arrows = new List<Arrow>();
    List<int> _finishedList = new List<int>();

    // Update is called once per frame
    void Update()
    {
        _finishedList.Clear();
        for (int i = 0; i < _arrows.Count; i++)
        {
            Arrow arrow = _arrows[i];
            if (arrow.IsFinished())
            {
                _finishedList.Add(i);
                arrow.Hide();
                Arrow.pools.Collect(arrow);
                continue;
            }
            arrow.OnUpdate();
        }
        foreach (var item in _finishedList)
        {
            _arrows.RemoveAt(item);
        }
    }

    public void Spawn(Agent1 agent, Vector3 pos, Vector3 dir, float damage, float speed, float hitMomentum)
    {
        Arrow arrow = Arrow.pools.Get();
        arrow.Init(agent, _arrowPrefab, pos, dir, damage, speed, hitMomentum);
        _arrows.Add(arrow);
        arrow.Show();
    }
}
