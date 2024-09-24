using System;
using System.Collections.Generic;
using UnityEngine;

namespace CupkekGames.Core
{
  public class CoreEventDatabase : MonoBehaviour
  {
    [Header("Settings Events")]
    public Action<int> ResolutionChangeEvent;
    public Action SaveSettingsEvent;
    public Action<float> MasterVolumeChangeEvent;
    public Action<float> MusicVolumeChangeEvent;
    public Action<float> AmbientVolumeChangeEvent;
    public Action<float> SFXVolumeChangeEvent;

#if UNITY_ADDRESSABLES
    [Header("Scene Management Events")]

    public Action<List<SceneSO>> SceneReadyEvent;
    public Action<List<SceneSO>> SceneUnloadEvent;
    public Action<bool, float> LoadingScreenToggleEvent; // fadeIn, duration
#endif
    [Header("Input Events")]
    public Action InputEscapeEvent;
  }
}