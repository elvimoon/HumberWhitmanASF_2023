using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundsSlider;
    [SerializeField] Slider masterSlider;

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SOUNDS = "SoundsVolume";
    const string MIXER_Master = "MasterVolume";
    void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundsSlider.onValueChanged.AddListener(SetSoundsVolume);
    }
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    void SetSoundsVolume(float value)
    {
        mixer.SetFloat(MIXER_SOUNDS, Mathf.Log10(value) * 20);
    }

    void SetMasterVolume(float value)
    {
        mixer.SetFloat(MIXER_Master, Mathf.Log10(value) * 20);
    }
}
