using UnityEngine;

namespace CupkekGames.Core
{
  public interface IPrefabLoaderBase
  {
    public void ReportDestroy(object key, GameObject instance);
    public void AddReportDestroy(object key, GameObject instance);
  }
}