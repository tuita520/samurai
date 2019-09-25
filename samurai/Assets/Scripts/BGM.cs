using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip bgmClip;          // 背景音乐

    public float fadeOutTime    = 0.4f;
    public float fadeInTime     = 0.4f;

    public AudioSource audioSource;

    private void Awake()
    {
        
    }    

    public void FadeIn(AudioClip clip)
    {        
        StopCoroutine("FadeInMusic");
        StopCoroutine("FadeOutMusic");        
        StartCoroutine(FadeInMusic(clip, 1, fadeOutTime, fadeInTime));
    }

    public void FadeOut()
    {        
        StopCoroutine("FadeInMusic");
        StopCoroutine("FadeOutMusic");
        StartCoroutine(FadeOutMusic(fadeOutTime));
    }

    IEnumerator FadeInMusic(AudioClip clip, float musicVolume, float fadeOutTime, float fadeInTime)
    {
        if (audioSource == null)
        {
            yield break;
        }

        if (audioSource.isPlaying)
        {
            if (fadeOutTime == 0)
            {
                audioSource.volume = 0;
                audioSource.Stop();
            }
            else
            {
                float maxVolume = audioSource.volume;
                float volume = audioSource.volume;
                while (volume > 0)
                {
                    volume -= 1 / fadeOutTime * Time.deltaTime * maxVolume;
                    if (volume < 0)
                        volume = 0;
                    audioSource.volume = volume;
                    yield return new WaitForEndOfFrame();
                }
                audioSource.Stop();
            }
        }

        yield return new WaitForEndOfFrame();

        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            if (fadeInTime == 0)
            {
                audioSource.volume = musicVolume;
            }
            else
            {
                float maxVolume = 1;
                float volume = 0;
                while (volume < maxVolume)
                {
                    volume += 1 / fadeInTime * Time.deltaTime * maxVolume;
                    if (volume > maxVolume)
                        volume = maxVolume;
                    audioSource.volume = volume;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }

    /*
    IEnumerator FadeInMusic(float time)
    {
        float volume = 0;
        StopCoroutine("FadeOutMusic");
        Music.Play();

        if (time == 0)
        {
            Music.volume = MaxMusicVolume;
            yield break;
        }


        //Debug.Log("Fade in music");
        while (volume < MaxMusicVolume)
        {
            volume += 1 / time * Time.deltaTime * MaxMusicVolume;
            if (volume > MaxMusicVolume)
                volume = MaxMusicVolume;

            Music.volume = volume;

            yield return new WaitForEndOfFrame();
        }
    }
    */

    IEnumerator FadeOutMusic(float foTime)
    {
        if (audioSource == null || audioSource.clip == null)
        {
            yield break;
        }

        if (foTime == 0)
        {
            audioSource.volume = 0;
            audioSource.Stop();
            yield break;
        }
        
        float volume = audioSource.volume;
        while (volume > 0)
        {
            volume -= 1 / foTime * Time.deltaTime;
            if (volume < 0)
                volume = 0;

            audioSource.volume = volume;

            yield return new WaitForEndOfFrame();
        }
        audioSource.Stop();        
    }
}
