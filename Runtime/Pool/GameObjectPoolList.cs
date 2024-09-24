using System;
using System.Collections.Generic;
using UnityEngine;

namespace CupkekGames.Core
{
  public class GameObjectPoolList : MonoBehaviour
  {
    private Dictionary<Guid, GameObjectPool> _poolDictionary = new();

    public Guid CreateNewPool(GameObject prefab, int defaultCapacity, int maxSize)
    {
      GameObjectPool pool = new GameObjectPool(prefab, defaultCapacity, maxSize);

      Guid id = Guid.NewGuid();

      _poolDictionary.Add(id, pool);

      return id;
    }

    public GameObjectPool GetPool(Guid id)
    {
      return _poolDictionary[id];
    }

    private void OnDestroy()
    {
      Dispose();
    }

    public void Dispose()
    {
      foreach (System.Collections.Generic.KeyValuePair<Guid, GameObjectPool> kvp in _poolDictionary)
      {
        kvp.Value.Pool.Dispose();
      }

      _poolDictionary.Clear();
    }
  }
}