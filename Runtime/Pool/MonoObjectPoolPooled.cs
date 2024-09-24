using UnityEngine;

namespace CupkekGames.Core
{
  public class MonoObjectPoolPooled : MonoBehaviour
  {
    [HideInInspector] public MonoGameObjectPool GameObjectPool; // GameObjectPool is set in GameObjectPool.OnTakeFromPool

    private void OnDisable()
    {
      if (GameObjectPool != null)
      {
        GameObjectPool.Pool.Release(gameObject);
      }
    }
  }
}