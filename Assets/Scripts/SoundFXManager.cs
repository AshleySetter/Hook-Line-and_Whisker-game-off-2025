using System;
using Unity.VisualScripting;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource loopingSoundFXObject;

    private void Awake()
    {
        Instance = this;
    }

    public Action PlayLoopingSoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float minPitch = 1, float maxPitch = 1)
    {
        // spawn in gameObject
        AudioSource audioSource = Instantiate(loopingSoundFXObject, spawnTransform.position, Quaternion.identity);
        GameObject audioSourceGameObject = audioSource.gameObject;
        // assign the audioClip
        audioSource.clip = audioClip;

        // assign volume
        audioSource.volume = volume;

        // assign random pitch shift
        audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);

        // play sound
        audioSource.Play();

        // get length of sound fx clip, accounting for change in length due to pitch change
        float clipLength = audioSource.clip.length * audioSource.pitch;

        // return function to destroy the clip when it should stop playing
        return () =>
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
            if (audioSourceGameObject != null)
            {
                Destroy(audioSourceGameObject, clipLength);
            }
        };
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float minPitch = 1, float maxPitch = 1)
    {
        // spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // assign the audioClip
        audioSource.clip = audioClip;

        // assign volume
        audioSource.volume = volume;

        // assign random pitch shift
        audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);

        // play sound
        audioSource.Play();

        // get length of sound fx clip, accounting for change in length due to pitch change
        float clipLength = audioSource.clip.length * audioSource.pitch;

        // destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float pitch)
    {
        PlaySoundFXClip(audioClip, spawnTransform, volume, pitch, pitch);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume, float minPitch = 1, float maxPitch = 1)
    {
        int rand = UnityEngine.Random.Range(0, audioClips.Length);
        PlaySoundFXClip(audioClips[rand], spawnTransform, volume, minPitch, maxPitch);
    }
}
