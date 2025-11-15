using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider soundFXVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    private void Start()
    {
        masterVolumeSlider.value = SoundMixerManager.Instance.GetMasterVolume();
        masterVolumeSlider.onValueChanged.AddListener((value) =>
        {
            SoundMixerManager.Instance.SetMasterVolume(value);
        });

        soundFXVolumeSlider.value = SoundMixerManager.Instance.GetSoundFXVolume();
        soundFXVolumeSlider.onValueChanged.AddListener((value) =>
        {
            SoundMixerManager.Instance.SetSoundFXVolume(value);
        });

        musicVolumeSlider.value = SoundMixerManager.Instance.GetMusicVolume();
        musicVolumeSlider.onValueChanged.AddListener((value) =>
        {
            SoundMixerManager.Instance.SetMusicVolume(value);
        });
    }
}

