using UnityEngine;

#if UNITY_URP
using UnityEngine.Rendering.Universal;
#endif

#if UNITY_LOCALIZATION
using UnityEngine.Localization.Settings;
#endif

namespace CupkekGames.Core
{
  public class SettingsSystem : MonoBehaviour
  {
    [SerializeField] private SettingsSO _currentSettings;
    public SettingsSO CurrentSettings { get => _currentSettings; }
    [SerializeField] private SettingsSO _defaultSettings;
    public SettingsSO DefaultSettings { get => _defaultSettings; }
    [SerializeField] private CoreEventDatabase _coreEventDatabase;

#if UNITY_URP
    [Header("URP Settings Assets [0 = low, 1 = medium, 2 = high, 3 = ultra]")]
    [SerializeField] private UniversalRenderPipelineAsset[] _renderPipelineAssets; // 0 = low, 1 = medium, 2 = high, 3 = ultra
#endif

    private void Awake()
    {
      CurrentSettings.Data.CopyValuesFrom(DefaultSettings.Data);
      CurrentSettings.Data.LoadFromDisk();
    }

    private void OnEnable()
    {
      _coreEventDatabase.SaveSettingsEvent += SaveSettings;
    }
    private void OnDisable()
    {
      _coreEventDatabase.SaveSettingsEvent -= SaveSettings;
    }
    private void Start()
    {
      // Gameplay Settings
#if UNITY_LOCALIZATION
      ApplyLocale(CurrentSettings.Data);
#endif

      // Graphics Settings
      ApplyTargetFrameRate(CurrentSettings.Data);
      ApplyVSync(CurrentSettings.Data);
      ApplyPostProcessing(CurrentSettings.Data);

#if UNITY_URP
      ApplyAntiAliasingURP(CurrentSettings.Data);
      ApplyShadowsURP(CurrentSettings.Data);
#endif

      ApplyTextureQuality(CurrentSettings.Data);
      ApplyEffectsQuality(CurrentSettings.Data);

      // Audio Settings
      ApplyAudio(CurrentSettings.Data);
    }

    public void SaveSettings()
    {
      CurrentSettings.Data.SaveToDisk();
    }

    public void ApplySettings(SettingsData settingsData)
    {
      Debug.Log("Applying settings");

      // Gameplay Settings
#if UNITY_LOCALIZATION
      ApplyLocale(settingsData);
#endif

      // Graphics Settings
      ApplyResolution(settingsData);
      ApplyFullScreenMode(settingsData);
      ApplyTargetFrameRate(settingsData);
      ApplyVSync(settingsData);
      ApplyPostProcessing(settingsData);

#if UNITY_URP
      ApplyAntiAliasingURP(CurrentSettings.Data);
      ApplyShadowsURP(CurrentSettings.Data);
#endif

      ApplyTextureQuality(settingsData);
      ApplyEffectsQuality(settingsData);

      // Audio Settings
      ApplyAudio(settingsData);
    }

#if UNITY_LOCALIZATION
    public void ApplyLocale(SettingsData settingsData)
    {
      LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(settingsData.LocaleIdentifier);
    }
#endif

    public void ApplyAudio(SettingsData settingsData)
    {
      _coreEventDatabase.MasterVolumeChangeEvent?.Invoke(settingsData.MasterVolume); //raise event for volume change
      _coreEventDatabase.MusicVolumeChangeEvent?.Invoke(settingsData.MusicVolume);//raise event for volume change
      _coreEventDatabase.AmbientVolumeChangeEvent?.Invoke(settingsData.AmbientVolume); //raise event for volume change
      _coreEventDatabase.SFXVolumeChangeEvent?.Invoke(settingsData.SFXVolume); //raise event for volume change
    }

    public void ApplyResolution(SettingsData settingsData)
    {
      Resolution resolution = settingsData.GetResolution();
      Screen.SetResolution(resolution.width, resolution.height, settingsData.FullScreenMode, resolution.refreshRateRatio);
      _coreEventDatabase.ResolutionChangeEvent?.Invoke(resolution.width);
    }

    public void ApplyFullScreenMode(SettingsData settingsData)
    {
      Screen.fullScreenMode = settingsData.FullScreenMode;
    }

    public void ApplyTargetFrameRate(SettingsData settingsData)
    {
      Application.targetFrameRate = (int)settingsData.TargetFrameRate;
    }

    public void ApplyVSync(SettingsData settingsData)
    {
      QualitySettings.vSyncCount = settingsData.VSync ? 1 : 0;
    }

#if UNITY_URP
    public void ApplyAntiAliasingURP(SettingsData settingsData)
    {
      foreach (UniversalRenderPipelineAsset renderPipelineAsset in _renderPipelineAssets)
      {
        renderPipelineAsset.msaaSampleCount = (int)settingsData.AntiAliasing;
      }
    }
#endif

    public void ApplyPostProcessing(SettingsData settingsData)
    {
      // TODO: Implement Post Processing Quality
    }

#if UNITY_URP
    public void ApplyShadowsURP(SettingsData settingsData)
    {
      QualitySettings.renderPipeline = _renderPipelineAssets[(int)settingsData.Shadows];
    }
#endif

    public void ApplyTextureQuality(SettingsData settingsData)
    {
      QualitySettings.globalTextureMipmapLimit = (int)settingsData.TextureQuality;
    }

    public void ApplyEffectsQuality(SettingsData settingsData)
    {
      // TODO: Implement Effects Quality
    }

    private void DebugPlayerPrefs()
    {
      Debug.Log("PlayerPrefs Settings: " + PlayerPrefs.HasKey("settings"));
      Debug.Log("PlayerPrefs Settings: " + PlayerPrefs.GetString("settings"));
    }
  }
}