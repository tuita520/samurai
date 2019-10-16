using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    Agent1 _agent;

    [SerializeField]
    SoundMgr1 _soundMgr;

    public SoundMgr1 SoundMgr { get { return _soundMgr; } }

    // Use this for initialization
    void Start()
    {
        _agent = GetComponent<Agent1>();        
        _soundMgr.Init(_agent.AudioSource);
    }
}
