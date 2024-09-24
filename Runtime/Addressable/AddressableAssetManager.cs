#if UNITY_ADDRESSABLES
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#if UNITASK_INSTALLED
using Cysharp.Threading.Tasks;
#endif

namespace CupkekGames.Core
{
  public class AddressableAssetManager : MonoBehaviour
  {
    private Dictionary<AssetReference, AsyncOperationHandle> _loadedAssetReference = new();
    private Dictionary<AsyncOperationHandle, List<GameObject>> _instantiatedGameObjects = new();

    public event Action<AssetReference, GameObject> OnInstanceDestroy;

    /// <summary>
    /// Event that is triggered when an asset is unloaded.
    /// </summary>
    public event Action<AssetReference> OnAssetUnloaded;

    /// <summary>
    /// Loads an asset of type T from the given asset reference.
    /// </summary>
    /// <typeparam name="T">The type of asset to load.</typeparam>
    /// <param name="assetReference">The asset reference to load the asset from.</param>
    /// <returns>The loaded asset.</returns>
    public object LoadAsset<T>(AssetReference assetReference)
    {
      if (IsLoaded(assetReference))
      {
        return _loadedAssetReference[assetReference].Result;
      }

      AsyncOperationHandle<T> handle = assetReference.LoadAssetAsync<T>();

      _loadedAssetReference.Add(assetReference, handle);

      handle.WaitForCompletion();

      if (!handle.IsValid() || handle.Status != AsyncOperationStatus.Succeeded)
      {
        return null;
      }

      return handle.Result;
    }

    public GameObject InstantiateSync(AssetReference assetReference, Vector3? position = null, Quaternion? rotation = null)
    {
      if (!IsLoaded(assetReference))
      {
        object loadResult = LoadAsset<GameObject>(assetReference);
        if (loadResult == null)
        {
          return null;
        }
      }

      return InstantiateGameObjectWithoutLoad(assetReference, position, rotation);
    }

    private GameObject InstantiateGameObjectWithoutLoad(AssetReference assetReference, Vector3? position = null, Quaternion? rotation = null)
    {
      AsyncOperationHandle handle = _loadedAssetReference[assetReference];

      if (handle.Status != AsyncOperationStatus.Succeeded)
      {
        Debug.LogError($"Failed to instantiate {assetReference}, handle status: {handle.Status}");
        return null;
      }

      if (position == null)
      {
        position = Vector3.zero;
      }
      if (rotation == null)
      {
        rotation = Quaternion.identity;
      }

      GameObject instance = Instantiate(handle.Result as GameObject, position.Value, rotation.Value);

      if (_instantiatedGameObjects.ContainsKey(handle))
      {
        _instantiatedGameObjects[handle].Add(instance);
      }
      else
      {
        _instantiatedGameObjects.Add(handle, new List<GameObject> { instance });
      }

      AddressableAssetReportDestroy addressableAssetReleaseOnDestroy = instance.GetComponent<AddressableAssetReportDestroy>();
      if (addressableAssetReleaseOnDestroy == null)
      {
        // Add the script to the targetGameObject
        addressableAssetReleaseOnDestroy = instance.AddComponent(typeof(AddressableAssetReportDestroy)) as AddressableAssetReportDestroy;
      }
      addressableAssetReleaseOnDestroy.AddressableAssetManager = this;
      addressableAssetReleaseOnDestroy.AssetReference = assetReference;

      return instance;
    }

    /// <summary>
    /// Releases the handle and destroys all game objects associated with it.
    /// </summary>
    /// <param name="assetReference">The asset reference to release and destroy.</param>
    /// <returns>True if the release and destruction was successful, false otherwise.</returns>
    public bool DestroyAllThenRelease(AssetReference assetReference)
    {
      if (assetReference == null)
      {
        return false;
      }

      if (!_loadedAssetReference.ContainsKey(assetReference))
      {
        return false;
      }

      // Get the handle
      AsyncOperationHandle handle = _loadedAssetReference[assetReference];
      // Remove the handle from the dictionary
      _loadedAssetReference.Remove(assetReference);

      if (!handle.IsValid())
      {
        return false;
      }

      // Destroy all game objects associated with the handle
      DestroyAllWithoutRelease(handle);

      // Release the handle
      Addressables.Release(handle);

      OnAssetUnloaded?.Invoke(assetReference);

      return true;
    }

    public bool ReportDestroy(AssetReference assetReference, GameObject instance)
    {
      if (!_loadedAssetReference.ContainsKey(assetReference))
      {
        return false;
      }

      AsyncOperationHandle handle = _loadedAssetReference[assetReference];

      if (!_instantiatedGameObjects.ContainsKey(handle))
      {
        return false;
      }

      if (_instantiatedGameObjects[handle].Remove(instance))
      {

        OnInstanceDestroy?.Invoke(assetReference, instance);

        if (_instantiatedGameObjects[handle].Count == 0)
        {
          DestroyAllThenRelease(assetReference);
        }

        return true;
      }

      return false;
    }

    /// <summary>
    /// Checks if the specified asset reference is loaded.
    /// </summary>
    /// <param name="assetReference">The asset reference to check.</param>
    /// <returns><c>true</c> if the asset reference is loaded; otherwise, <c>false</c>.</returns>
    public bool IsLoaded(AssetReference assetReference)
    {
      // Clear invalid handles before checking so that the dictionary is up to date
      ClearInvalidHandles(assetReference);

      // Check if the asset reference is loaded
      return _loadedAssetReference.ContainsKey(assetReference);
    }

    public GameObject GetFirstInstance(AssetReference assetReference)
    {
      List<GameObject> instances = GetInstances(assetReference);

      if (instances == null)
      {
        return null;
      }

      Debug.Log("3: " + instances.Count);

      return instances.Count > 0 ? instances[0] : null;
    }

    public List<GameObject> GetInstances(AssetReference assetReference)
    {
      if (!IsLoaded(assetReference))
      {
        return null;
      }

      AsyncOperationHandle handle = _loadedAssetReference[assetReference];

      if (!_instantiatedGameObjects.ContainsKey(handle))
      {
        return null;
      }

      return _instantiatedGameObjects[handle];
    }

    /// <summary>
    /// Clears invalid handles associated with the specified asset reference.
    /// If the handle is not valid, it is removed from the dictionary and all associated game objects are destroyed.
    /// </summary>
    /// <param name="assetReference">The asset reference to clear invalid handles for.</param>
    private void ClearInvalidHandles(AssetReference assetReference)
    {
      if (!_loadedAssetReference.ContainsKey(assetReference))
      {
        return;
      }

      AsyncOperationHandle handle = _loadedAssetReference[assetReference];

      if (!handle.IsValid())
      {
        // Remove the handle from the dictionary
        _loadedAssetReference.Remove(assetReference);
        // Destroy all game objects associated with the handle
        DestroyAllWithoutRelease(handle);
      }
    }

    public bool DestroyAllWithoutRelease(AssetReference assetReference)
    {
      if (!_loadedAssetReference.ContainsKey(assetReference))
      {
        return false;
      }

      AsyncOperationHandle handle = _loadedAssetReference[assetReference];

      DestroyAllWithoutRelease(handle);

      return true;
    }

    /// <summary>
    /// Destroys all instantiated game objects associated with the specified AsyncOperationHandle
    /// and removes the handle from the dictionary.
    /// </summary>
    /// <param name="handle">The AsyncOperationHandle to destroy game objects for.</param>
    public void DestroyAllWithoutRelease(AsyncOperationHandle handle)
    {
      if (_instantiatedGameObjects.ContainsKey(handle))
      {
        foreach (GameObject instance in _instantiatedGameObjects[handle])
        {
          if (instance != null)
          {
            Destroy(instance);
          }
        }

        _instantiatedGameObjects.Remove(handle);
      }
    }

#if UNITASK_INSTALLED
    /// <summary>
    /// Loads an asset asynchronously from an AssetReference.
    /// </summary>
    /// <typeparam name="T">The type of asset to load.</typeparam>
    /// <param name="assetReference">The AssetReference of the asset to load.</param>
    /// <returns>A UniTask representing the asynchronous loading operation.</returns>
    public async UniTask<object> LoadAssetAsync<T>(AssetReference assetReference)
    {
      if (IsLoaded(assetReference))
      {
        return _loadedAssetReference[assetReference].Result;
      }

      AsyncOperationHandle<T> handle = assetReference.LoadAssetAsync<T>();

      _loadedAssetReference.Add(assetReference, handle);

      await handle.Task;

      if (!handle.IsValid() || handle.Status != AsyncOperationStatus.Succeeded)
      {
        return null;
      }

      return handle.Result;
    }

    /// <summary>
    /// Instantiates a GameObject asynchronously from an AssetReference.
    /// </summary>
    /// <param name="assetReference">The AssetReference of the GameObject to instantiate.</param>
    /// <param name="position">The position of the instantiated GameObject. If null, the default position is Vector3.zero.</param>
    /// <param name="rotation">The rotation of the instantiated GameObject. If null, the default rotation is Quaternion.identity.</param>
    /// <returns>The instantiated GameObject.</returns>
    public async UniTask<GameObject> InstantiateAsync(AssetReference assetReference, Vector3? position = null, Quaternion? rotation = null)
    {
      if (!IsLoaded(assetReference))
      {
        object loadResult = await LoadAssetAsync<GameObject>(assetReference);
        if (loadResult == null)
        {
          return null;
        }
      }

      return InstantiateGameObjectWithoutLoad(assetReference, position, rotation);
    }
#endif
  }
}
#endif
