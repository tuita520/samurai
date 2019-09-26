using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.Utilities;

[System.Serializable]
public class SoundData
{
    public SoundType soundType;
    public AudioClip[] clips;

    public AudioClip GetClip()
    {
        if (clips.Length == 0)
        {
            return null;
        }
        return clips[Random.Range(0, clips.Length)];
    }
}

public class SoundMgr : MonoBehaviour
{
    public List<SoundData> sounds = new List<SoundData>();

    Dictionary<SoundType, AudioClip> _sounds = new Dictionary<SoundType, AudioClip>();

    // Use this for initialization
    void Start()
    {
        foreach (var sound in sounds)
        {
            _sounds.Add(sound.soundType, sound.GetClip());
        }
    }

    /// <summary>
    /// 简单播一发
    /// </summary>    
    public void PlayOneShot(AudioSource audioSource, SoundType soundType)
    {
        if (_sounds.ContainsKey(soundType) == false)
        {
            return;
        }
        AudioTools.Instance.PlayOneShot(audioSource, _sounds[soundType]);        
    }

    /// <summary>
    /// 音量渐起
    /// </summary>    
    public void PlayIn(AudioSource audioSource, SoundType soundType, float fadeInTime)
    {
        if (_sounds.ContainsKey(soundType) == false)
        {
            return;
        }

        AudioTools.Instance.PlayIn(audioSource, _sounds[soundType], fadeInTime);
    }
    
    /// <summary>
    /// 音量渐落
    /// </summary>    
    public void PlayOut(AudioSource audioSource, SoundType soundType, float fadeOutTime)
    {
        if (_sounds.ContainsKey(soundType) == false)
        {
            return;
        }

        AudioTools.Instance.PlayOut(audioSource, _sounds[soundType], fadeOutTime);        
    }

    /// <summary>
    /// 循环播放clip
    /// </summary>    
    public void PlayLoop(AudioSource audioSource, SoundType soundType)
    {
        if (_sounds.ContainsKey(soundType) == false)
        {
            return;
        }
        AudioTools.Instance.PlayLoop(audioSource, _sounds[soundType]);
    }

    /// <summary>
    /// 在指定时长内循环播放clip
    /// </summary>    
    public void PlayLoop(AudioSource audioSource, SoundType soundType, float time,
        float fadeInTime = 0, float fadeOutTime = 0)
    {
        if (_sounds.ContainsKey(soundType) == false)
        {
            return;
        }
        AudioTools.Instance.PlayLoop(audioSource, _sounds[soundType], time, fadeInTime, fadeOutTime);
    }

    /// <summary>
    /// 过渡播放(当前clip渐落之后，新clip渐起)
    /// </summary>    
    public void PlayCross(AudioSource audioSource, SoundType soundType, float fadeOutTime, float fadeInTime)
    {
        if (_sounds.ContainsKey(soundType) == false)
        {
            return;
        }
        AudioTools.Instance.PlayCross(audioSource, _sounds[soundType], fadeOutTime, fadeInTime);
    }    
}