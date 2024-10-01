#if UNITY_ADDRESSABLES
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CupkekGames.Core
{
  public abstract class PrefabLoaderAddressable<TKey> : KeyValueDatabaseMono<TKey, AssetReference>, IPrefabLoader<TKey, AssetReference>
  {
    [SerializeField] private AddressableAssetManager _addressableAssetManager;

#pragma warning disable CS0414
    [SerializeField] string _searchLabel = "Prefab"; // Used by editor code
#pragma warning restore CS0414

    public event EventHandler<TKey> OnInstanceDestroyed;

    protected override void Awake()
    {
      base.Awake();

      if (_addressableAssetManager == null)
      {
        _addressableAssetManager = ServiceLocator.Get<AddressableAssetManager>();
      }
    }
    public bool IsLoaded(TKey key)
    {
      return _addressableAssetManager.IsLoaded(Dictionary[key]);
    }

    // public void LoadOnly(string key)
    // {
    //   _addressableAssetManager.LoadAsset<GameObject>(_prefabs[key]);
    // }

    // public void DestroyWithoutRelease(string key)
    // {
    //   if (_addressableAssetManager.DestroyAllWithoutRelease(_prefabs[key]))
    //   {
    //     OnUIUnloaded?.Invoke(key);
    //   }
    // }

    public GameObject Instantiate(TKey key)
    {
      if (!Dictionary.ContainsKey(key))
      {
        Debug.LogWarning("Key not found: " + key);

        return null;
      }

      GameObject instance = _addressableAssetManager.InstantiateSync(Dictionary[key]);

      AddReportDestroy(key, instance);

      return instance;
    }

    public void DestroyAllOf(TKey key)
    {
      bool unloaded = _addressableAssetManager.DestroyAllThenRelease(Dictionary[key]);

      if (unloaded)
      {
        OnInstanceDestroyed?.Invoke(this, key);
      }
    }

    public List<GameObject> GetInstances(TKey key)
    {
      return _addressableAssetManager.GetInstances(Dictionary[key]);
    }

    public IEnumerator DestroyAllOfWithDelay(TKey key, float duration = 0.5f)
    {
      yield return new WaitForSeconds(duration);

      DestroyAllOf(key);
    }

    public void AddReportDestroy(object key, GameObject instance)
    {
      if (!instance.TryGetComponent<PrefabLoaderReportDestroy>(out var report))
      {
        report = instance.AddComponent<PrefabLoaderReportDestroy>();
      }

      report.PrefabLoader = this;
      report.PrefabKey = key;
    }

    public void ReportDestroy(object key, GameObject instance)
    {
      OnInstanceDestroyed?.Invoke(this, (TKey)key);
    }

    public void DestroyAll()
    {
      foreach (var kvp in Dictionary)
      {
        DestroyAllOf(kvp.Key);
      }
    }
  }
}
#endif