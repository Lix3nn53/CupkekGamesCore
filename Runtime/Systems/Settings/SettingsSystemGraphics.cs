using UnityEngine;

#if UNITY_URP
using UnityEngine.Rendering.Universal;
#endif

namespace CupkekGames.Core
{
  public class SettingsSystemGraphics : MonoBehaviour
  {
    [Header("Listening on channels")]
    [SerializeField] private CoreEventDatabase _coreEventDatabase = default;
#if UNITY_URP
    [Header("URP Settings Assets [0 = low, 1 = medium, 2 = high, 3 = ultra]")]
    [SerializeField] private UniversalRenderPipelineAsset[] _renderPipelineAssets; // 0 = low, 1 = medium, 2 = high, 3 = ultra
#endif
    public void ApplySettings(SettingsData settingsData)
    {
      ApplyResolution(settingsData);
      ApplyFullScreenMode(settingsData);
      ApplyTargetFrameRate(settingsData);
      ApplyVSync(settingsData);
      ApplyPostProcessing(settingsData);

#if UNITY_URP
      ApplyAntiAliasingURP(settingsData);
      ApplyShadowsURP(settingsData);
#endif

      ApplyTextureQuality(settingsData);
      ApplyEffectsQuality(settingsData);
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
  }
}