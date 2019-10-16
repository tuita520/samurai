using UnityEngine;
using Phenix.Unity.Pattern;

public class SoundCenter : Singleton<SoundCenter>
{
    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    SoundMgr1 _soundMgr;

    public SoundMgr1 SoundMgr { get { return _soundMgr; } }

    // Use this for initialization
    void Start()
    {        
        _soundMgr.Init(_audioSource);
    }
}
