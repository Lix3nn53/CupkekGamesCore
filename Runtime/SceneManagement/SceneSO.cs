#if UNITY_ADDRESSABLES
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CupkekGames.Core
{
  /// <summary>
  /// This class is a base class which contains what is common to all game scenes (Locations, Menus, Managers)
  /// </summary>
  [CreateAssetMenu(fileName = "SceneSO", menuName = "CupkekGames/SceneManagement/SceneSO")]
  public class SceneSO : ScriptableObject
  {
    public AssetReferenceT<SceneAsset> sceneReference; //Used at runtime to load the scene from the right AssetBundle
  }
}
#endif