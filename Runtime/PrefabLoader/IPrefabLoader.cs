
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CupkekGames.Core
{
  public interface IPrefabLoader<TKey, TValue> : IKeyValueDatabase<TKey, TValue>, IPrefabLoaderBase
  {
    // Events
    public event EventHandler<TKey> OnInstanceDestroyed;
    // Methods
    public List<GameObject> GetInstances(TKey key);
    public GameObject Instantiate(TKey key);
    public void DestroyAllOf(TKey key);
    public IEnumerator DestroyAllOfWithDelay(TKey key, float duration);
    public void DestroyAll();
    // public void ReportDestroy(TKey key, GameObject instance);
    // public void AddReportDestroy(TKey key, GameObject instance);
  }
}