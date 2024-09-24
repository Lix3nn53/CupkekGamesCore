using UnityEngine;

namespace CupkekGames.Core
{
  public class GameObjectPool : ObjectPoolBase<GameObject>
  {
    private GameObject _prefab;
    public GameObjectPool(GameObject prefab, int defaultCapacity, int maxSize, bool collectionCheck = true) : base(defaultCapacity, maxSize, false, collectionCheck)
    {
      _prefab = prefab;

      Prewarm();

      InternalDebug.Log("Created GameObjectPool pool for " + typeof(GameObject) + " with " + Pool.CountAll + " objects");
    }

    public override GameObject CreatePooledObject()
    {
      GameObject instance = GameObject.Instantiate(_prefab);
      instance.gameObject.SetActive(false);

      if (instance.GetComponent<GameObjectPoolPooled>() == null)
      {
        // Add the script to the targetGameObject
        instance.AddComponent(typeof(GameObjectPoolPooled));
      }

      return instance;
    }

    public override void OnTakeFromPool(GameObject instance)
    {
      // Instance.gameObject.SetActive(true);
      GameObjectPoolPooled pooledGameObject = instance.GetComponent<GameObjectPoolPooled>();
      pooledGameObject.GameObjectPool = this;
    }

    public override void OnReturnToPool(GameObject Instance)
    {
      Instance.gameObject.SetActive(false);
    }

    public override void OnDestroyObject(GameObject Instance)
    {
      GameObject.Destroy(Instance.gameObject);
    }
  }
}