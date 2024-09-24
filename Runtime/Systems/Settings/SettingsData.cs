using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace CupkekGames.Core
{
  public class SettingsData : ISerializable
  {
    // This is test data, written according to TestScript.cs class
    // This will change according to whatever data that needs to be stored

    // The variables need to be public, else we would have to write trivial getter/setter functions.

    // Gameplay settings
    public LocaleIdentifier LocaleIdentifier = new LocaleIdentifier("en");
    public Locale Locale => LocalizationSettings.AvailableLocales.GetLocale(LocaleIdentifier);
    public int LocaleIndex => LocalizationSettings.AvailableLocales.Locales.IndexOf(Locale);

    // Audio settings
    public float MasterVolume;
    public float MusicVolume;
    public float AmbientVolume;
    public float SFXVolume;

    // Graphics settings
    public uint RefreshRateDenominator = 0;
    public uint RefreshRateNumerator = 0;
    public int ResolutionWidth = 0;
    public int ResolutionHeight = 0;
    public Resolution GetResolution()
    {
      if (ResolutionWidth == 0 || ResolutionHeight == 0)
      {
        return Screen.currentResolution;
      }

      return new Resolution
      {
        width = ResolutionWidth,
        height = ResolutionHeight,
        refreshRateRatio = new RefreshRate
        {
          denominator = RefreshRateDenominator,
          numerator = RefreshRateNumerator
        }
      };
    }

    public void SetResolution(Resolution resolution)
    {
      ResolutionWidth = resolution.width;
      ResolutionHeight = resolution.height;
      RefreshRateDenominator = resolution.refreshRateRatio.denominator;
      RefreshRateNumerator = resolution.refreshRateRatio.numerator;
    }

    public FullScreenMode FullScreenMode;
    public bool VSync;
    public TargetFrameRateEnum TargetFrameRate = TargetFrameRateEnum.Sixty;

    public enum TargetFrameRateEnum
    {
      Unlimited = -1,
      Thirty = 30,
      Sixty = 60,
      OneTwenty = 120,
      OneFortyFour = 144
    }

    public AntiAliasingEnum AntiAliasing;

    public enum AntiAliasingEnum
    {
      Off = 1,
      Two = 2,
      Four = 4,
      Eight = 8
    }

    public PostProcessingEnum PostProcessing;

    public enum PostProcessingEnum
    {
      Low = 0,
      Medium = 1,
      High = 2,
      Ultra = 3
    }

    public ShadowsEnum Shadows;

    public enum ShadowsEnum
    {
      Low = 0,
      Medium = 1,
      High = 2,
      Ultra = 3
    }

    public TextureQualityEnum TextureQuality; // 0 = original size, 1 = half size, 2 = quarter size, 3 = eighth size

    public enum TextureQualityEnum
    {
      Original = 0,
      Half = 1,
      Quarter = 2,
      Eighth = 3
    }

    public EffectsQualityEnum EffectsQuality;

    public enum EffectsQualityEnum
    {
      Low = 0,
      Medium = 1,
      High = 2,
      Ultra = 3
    }

    public void CopyValuesFrom(SettingsData copy)
    {
      LocaleIdentifier = copy.LocaleIdentifier;
      MasterVolume = copy.MasterVolume;
      MusicVolume = copy.MusicVolume;
      AmbientVolume = copy.AmbientVolume;
      SFXVolume = copy.SFXVolume;
      ResolutionWidth = copy.ResolutionWidth;
      ResolutionHeight = copy.ResolutionHeight;
      RefreshRateDenominator = copy.RefreshRateDenominator;
      RefreshRateNumerator = copy.RefreshRateNumerator;
      FullScreenMode = copy.FullScreenMode;
      VSync = copy.VSync;
      TargetFrameRate = copy.TargetFrameRate;
      AntiAliasing = copy.AntiAliasing;
      PostProcessing = copy.PostProcessing;
      Shadows = copy.Shadows;
      TextureQuality = copy.TextureQuality;
      EffectsQuality = copy.EffectsQuality;
    }

    public bool Equals(SettingsData b)
    {
      SettingsData a = this;

      if (ReferenceEquals(a, b))
      {
        return true;
      }

      if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
      {
        return false;
      }

      return
             a.LocaleIdentifier == b.LocaleIdentifier &&
             a.MasterVolume == b.MasterVolume &&
             a.MusicVolume == b.MusicVolume &&
              a.AmbientVolume == b.AmbientVolume &&
             a.SFXVolume == b.SFXVolume &&
             a.ResolutionWidth == b.ResolutionWidth &&
              a.ResolutionHeight == b.ResolutionHeight &&
              a.RefreshRateDenominator == b.RefreshRateDenominator &&
              a.RefreshRateNumerator == b.RefreshRateNumerator &&
             a.FullScreenMode == b.FullScreenMode &&
             a.VSync == b.VSync &&
             a.TargetFrameRate == b.TargetFrameRate &&
             a.AntiAliasing == b.AntiAliasing &&
             a.PostProcessing == b.PostProcessing &&
             a.Shadows == b.Shadows &&
             a.TextureQuality == b.TextureQuality &&
             a.EffectsQuality == b.EffectsQuality;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("LocaleIdentifier", LocaleIdentifier, typeof(LocaleIdentifier));
      info.AddValue("MasterVolume", MasterVolume);
      info.AddValue("MusicVolume", MusicVolume);
      info.AddValue("AmbientVolume", AmbientVolume);
      info.AddValue("SFXVolume", SFXVolume);
      info.AddValue("FullScreenMode", FullScreenMode);
      info.AddValue("VSync", VSync);
      info.AddValue("TargetFrameRate", TargetFrameRate);
      info.AddValue("AntiAliasing", AntiAliasing);
      info.AddValue("PostProcessing", PostProcessing);
      info.AddValue("Shadows", Shadows);
      info.AddValue("TextureQuality", TextureQuality);
      info.AddValue("EffectsQuality", EffectsQuality);

      info.AddValue("ResolutionWidth", ResolutionWidth);
      info.AddValue("ResolutionHeight", ResolutionHeight);
      info.AddValue("RefreshRateDenominator", RefreshRateDenominator);
      info.AddValue("RefreshRateNumerator", RefreshRateNumerator);
    }

    public SettingsData()
    {
    }

    public SettingsData(SerializationInfo info, StreamingContext context)
    {
      LocaleIdentifier = (LocaleIdentifier)info.GetValue("LocaleIdentifier", typeof(LocaleIdentifier));
      MasterVolume = info.GetSingle("MasterVolume");
      MusicVolume = info.GetSingle("MusicVolume");
      AmbientVolume = info.GetSingle("AmbientVolume");
      SFXVolume = info.GetSingle("SFXVolume");
      FullScreenMode = (FullScreenMode)info.GetValue("FullScreenMode", typeof(FullScreenMode));
      VSync = info.GetBoolean("VSync");
      TargetFrameRate = (TargetFrameRateEnum)info.GetValue("TargetFrameRate", typeof(TargetFrameRateEnum));
      AntiAliasing = (AntiAliasingEnum)info.GetValue("AntiAliasing", typeof(AntiAliasingEnum));
      PostProcessing = (PostProcessingEnum)info.GetValue("PostProcessing", typeof(PostProcessingEnum));
      Shadows = (ShadowsEnum)info.GetValue("Shadows", typeof(ShadowsEnum));
      TextureQuality = (TextureQualityEnum)info.GetValue("TextureQuality", typeof(TextureQualityEnum));
      EffectsQuality = (EffectsQualityEnum)info.GetValue("EffectsQuality", typeof(EffectsQualityEnum));

      ResolutionWidth = info.GetInt32("ResolutionWidth");
      ResolutionHeight = info.GetInt32("ResolutionHeight");
      RefreshRateDenominator = info.GetUInt32("RefreshRateDenominator");
      RefreshRateNumerator = info.GetUInt32("RefreshRateNumerator");
    }

    public void SaveToDisk()
    {
      Debug.Log("SettingsData Saving...");
      string json = JsonUtility.ToJson(this);
      PlayerPrefs.SetString("settings", json);
      PlayerPrefs.Save();
      Debug.Log("SettingsData Saved");
    }

    public bool LoadFromDisk()
    {
      if (PlayerPrefs.HasKey("settings"))
      {
        string json = PlayerPrefs.GetString("settings");
        SettingsData settingsData = JsonUtility.FromJson<SettingsData>(json);
        CopyValuesFrom(settingsData);

        Debug.Log("SettingsData Loaded");
        return true;
      }

      Debug.Log("SettingsData not found, using default settings");

      return false;
    }
  }
}