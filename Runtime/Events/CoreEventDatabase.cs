using System;
using System.Collections.Generic;
using UnityEngine;

namespace CupkekGames.Core
{
  public class CoreEventDatabase : MonoBehaviour
  {
    // Settings Events
    public Action<int> ResolutionChangeEvent;
    public Action SaveSettingsEvent;
    public Action<float> MasterVolumeChangeEvent;
    public Action<float> MusicVolumeChangeEvent;
    public Action<float> AmbientVolumeChangeEvent;
    public Action<float> SFXVolumeChangeEvent;

    // Scene Management Events
    public Action<bool, float> LoadingScreenToggleEvent; // fadeIn, duration
#if UNITY_ADDRESSABLES

    public Action<List<SceneSO>> SceneReadyEvent;
    public Action<List<SceneSO>> SceneUnloadEvent;
#endif
    // Input Events
    public Action InputEscapeEvent;
  }
}