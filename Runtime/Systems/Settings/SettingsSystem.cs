using UnityEngine;

namespace CupkekGames.Core
{
  public class SettingsSystem : MonoBehaviour
  {
    [SerializeField] private SettingsSO _currentSettings;
    public SettingsSO CurrentSettings { get => _currentSettings; }
    [SerializeField] private SettingsSO _defaultSettings;
    public SettingsSO DefaultSettings { get => _defaultSettings; }
    [SerializeField] private CoreEventDatabase _coreEventDatabase;
    // Components
    [SerializeField] private SettingsSystemAudio _componentAudio;
    public SettingsSystemAudio Audio => _componentAudio;
    [SerializeField] private SettingsSystemGraphics _componentGraphics;
    public SettingsSystemGraphics Graphics => _componentGraphics;
#if UNITY_LOCALIZATION
    [SerializeField] private SettingsSystemLocalization _componentLocalization;
    public SettingsSystemLocalization Localization => _componentLocalization;
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
      ApplySettings(CurrentSettings.Data);
    }

    public void SaveSettings()
    {
      CurrentSettings.Data.SaveToDisk();
    }

    public void ApplySettings(SettingsData settingsData)
    {
      Debug.Log("Applying settings");

#if UNITY_LOCALIZATION
      _componentLocalization.ApplySettings(settingsData);
#endif

      _componentGraphics.ApplySettings(settingsData);
      _componentAudio.ApplySettings(settingsData);
    }

#if UNITY_LOCALIZATION
#endif



    private void DebugPlayerPrefs()
    {
      Debug.Log("PlayerPrefs Settings: " + PlayerPrefs.HasKey("settings"));
      Debug.Log("PlayerPrefs Settings: " + PlayerPrefs.GetString("settings"));
    }
  }
}