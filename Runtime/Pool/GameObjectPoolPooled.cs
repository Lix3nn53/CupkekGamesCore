using UnityEngine;

namespace CupkekGames.Core
{
  public class GameObjectPoolPooled : MonoBehaviour
  {
    [HideInInspector] public GameObjectPool GameObjectPool; // GameObjectPool is set in GameObjectPool.OnTakeFromPool

    private void OnDisable()
    {
      if (GameObjectPool != null)
      {
        GameObjectPool.Pool.Release(gameObject);
      }
    }
  }
}