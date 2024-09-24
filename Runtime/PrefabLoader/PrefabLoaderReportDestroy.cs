using UnityEngine;

namespace CupkekGames.Core
{
  public class PrefabLoaderReportDestroy : MonoBehaviour
  {
    public object PrefabKey;
    public IPrefabLoaderBase PrefabLoader;
    private void OnDestroy()
    {
      PrefabLoader.ReportDestroy(PrefabKey, gameObject);
    }
  }
}