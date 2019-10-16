using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.Audio;

[System.Serializable]
public class SoundData1
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


[System.Serializable]
public class SoundMgr1 : AudioMgr
{
    public List<SoundData> sounds = new List<SoundData>();

    public void Init(AudioSource audioSource)
    {
        this.audioSource = audioSource;
        foreach (var sound in sounds)
        {
            Add((int)sound.soundType, sound.GetClip());
        }
    }
}
