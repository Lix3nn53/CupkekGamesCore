#if UNITY_ADDRESSABLES
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CupkekGames.Core
{
  public class AddressableAssetReportDestroy : MonoBehaviour
  {
    [HideInInspector] public AddressableAssetManager AddressableAssetManager;
    [HideInInspector] public AssetReference AssetReference;

    private void OnDestroy()
    {
      AddressableAssetManager.ReportDestroy(AssetReference, gameObject);
    }
  }
}
#endif
