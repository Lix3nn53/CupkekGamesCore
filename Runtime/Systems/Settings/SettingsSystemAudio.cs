using UnityEngine;
using CupkekGames.Core;
using UnityEngine.Audio;

namespace CupkekGames.Core
{
  public class SettingsSystemAudio : MonoBehaviour
  {
    [Header("References")]
    [SerializeField] private SettingsSystem _settingsSystem;

    [Header("Listening on channels")]
    [SerializeField] private CoreEventDatabase _coreEventDatabase = default;

    [Header("Audio control")]
    [SerializeField] private AudioMixer _audioMixer = default;
    private void OnEnable()
    {
      _coreEventDatabase.MasterVolumeChangeEvent += ChangeMasterVolume;
      _coreEventDatabase.MusicVolumeChangeEvent += ChangeMusicVolume;
      _coreEventDatabase.AmbientVolumeChangeEvent += ChangeAmbientVolume;
      _coreEventDatabase.SFXVolumeChangeEvent += ChangeSFXVolume;
    }

    private void OnDisable()
    {
      _coreEventDatabase.MasterVolumeChangeEvent -= ChangeMasterVolume;
      _coreEventDatabase.MusicVolumeChangeEvent -= ChangeMusicVolume;
      _coreEventDatabase.AmbientVolumeChangeEvent -= ChangeAmbientVolume;
      _coreEventDatabase.SFXVolumeChangeEvent -= ChangeSFXVolume;
    }

    public void ChangeMasterVolume(float newVolume)
    {
      SetGroupVolume("MasterVolume", newVolume);
    }
    public void ChangeMusicVolume(float newVolume)
    {
      SetGroupVolume("MusicVolume", newVolume);
    }
    public void ChangeAmbientVolume(float newVolume)
    {
      SetGroupVolume("AmbientVolume", newVolume);
    }
    public void ChangeSFXVolume(float newVolume)
    {
      SetGroupVolume("SFXVolume", newVolume);
    }
    public void SetGroupVolume(string parameterName, float normalizedVolume)
    {
      bool volumeSet = _audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
      if (!volumeSet)
        Debug.LogError("The AudioMixer parameter was not found: " + parameterName);
    }

    public float GetGroupVolume(string parameterName)
    {
      if (_audioMixer.GetFloat(parameterName, out float rawVolume))
      {
        return MixerValueToNormalized(rawVolume);
      }
      else
      {
        Debug.LogError("The AudioMixer parameter was not found");
        return 0f;
      }
    }

    // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
    /// when using UI sliders normalized format
    private float MixerValueToNormalized(float mixerValue)
    {
      // We're assuming the range [-80dB to 0dB] becomes [0 to 1]
      return 1f + (mixerValue / 80f);
    }
    private float NormalizedToMixerValue(float normalizedValue)
    {
      // We're assuming the range [0 to 1] becomes [-80dB to 20dB]
      // This doesn't allow values over 0dB
      normalizedValue = normalizedValue < 0.0001f ? 0.0001f : normalizedValue;
      normalizedValue = normalizedValue > 1f ? 1f : normalizedValue;

      return Mathf.Log10(normalizedValue) * 20;
    }
    public void ApplySettings(SettingsData settingsData)
    {
      ChangeMasterVolume(settingsData.MasterVolume);
      ChangeMusicVolume(settingsData.MusicVolume);
      ChangeAmbientVolume(settingsData.AmbientVolume);
      ChangeSFXVolume(settingsData.SFXVolume);
    }
  }
}