#if UNITY_ADDRESSABLES
using System.Collections.Generic;

namespace CupkekGames.Core
{
  // Do not use struct, because we need to pass it by reference
  public class SceneLoadRequest
  {
    //Parameters coming from scene loading requests
    public List<SceneSO> ScenesToLoad;
    public int ScenesLeftToLoad;
    public List<SceneSO> ScenesToUNNLoad;
    public int ScenesLeftToUNNLoad;
    public SceneLoadTransition SceneLoadTransition;

    public SceneLoadRequest(List<SceneSO> scenesToLoad, List<SceneSO> scenesToUNNLoad, SceneLoadTransition sceneLoadTransition)
    {
      ScenesToLoad = scenesToLoad != null ? scenesToLoad : new List<SceneSO>();
      ScenesLeftToLoad = scenesToLoad != null ? scenesToLoad.Count : 0;
      ScenesToUNNLoad = scenesToUNNLoad != null ? scenesToUNNLoad : new List<SceneSO>();
      ScenesLeftToUNNLoad = scenesToUNNLoad != null ? scenesToUNNLoad.Count : 0;
      SceneLoadTransition = sceneLoadTransition;
    }

    public int GetNextSceneToLoadIndex()
    {
      // Load scenes in order
      return ScenesToLoad.Count - ScenesLeftToLoad;
    }
  }
}
#endif