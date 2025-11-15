using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager Instance { get; private set; }
    [SerializeField] private AudioMixer audioMixer;


    private void Awake()
    {
        Instance = this;
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }

    public float GetMasterVolume()
    {
        float level;
        audioMixer.GetFloat("MasterVolume", out level);
        return Mathf.Pow(10f, level / 20f);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
    }

    public float GetSoundFXVolume()
    {
        float level;
        audioMixer.GetFloat("SoundFXVolume", out level);
        return Mathf.Pow(10f, level / 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }
    
    public float GetMusicVolume()
    {
        float level;
        audioMixer.GetFloat("MusicVolume", out level);
        return Mathf.Pow(10f, level / 20f);
    }
}
