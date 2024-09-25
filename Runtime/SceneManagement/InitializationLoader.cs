#if UNITY_ADDRESSABLES
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CupkekGames.Core
{
  /// <summary>
  /// This class is responsible for starting the game by loading the persistent managers scene 
  /// and raising the event to load the Main Menu
  /// </summary>

  public class InitializationLoader : MonoBehaviour
  {
    [MultiLineHeader("Persistent Scenes\nScenes that will load at start and never unload again.\nThese scenes are NOT managed by SceneLoader.\nDo not create GameSceneSO for them.")]
    [SerializeField] private List<AssetReferenceT<SceneAsset>> persistentScenes;
    [MultiLineHeader("Start Scenes\nScenes to load after persistent scenes are loaded.\nThese scenes are managed by SceneLoader.")]
    [SerializeField] private List<SceneSO> startScenes;

    private int _persistentLoaded;

    private void Start()
    {
      //Load the persistent managers scene
      // _managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadMainMenu;

      if (persistentScenes.Count == 0)
      {
        OnPersistentScenesReady();
      }
      else
      {
        foreach (AssetReference persistent in persistentScenes)
        {
          persistent.LoadSceneAsync(LoadSceneMode.Single, true).Completed += OnPersistentSceneLoad;
        }
      }
    }

    private void OnPersistentSceneLoad(AsyncOperationHandle<SceneInstance> obj)
    {
      _persistentLoaded++;

      if (_persistentLoaded == persistentScenes.Count)
      {
        OnPersistentScenesReady();
      }
    }

    private void OnPersistentScenesReady()
    {
      // Since persistent scenes is loaded, we can get the services
      SceneLoader sceneLoader = ServiceLocator.Get<SceneLoader>();

      sceneLoader.SetActiveScene(startScenes[0]);

      sceneLoader.LoadScene(startScenes, sceneLoader.TransitionManager.Transitions.Dictionary["Fade"]);
    }
  }
}
#endif