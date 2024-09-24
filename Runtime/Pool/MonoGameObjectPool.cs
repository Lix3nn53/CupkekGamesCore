using UnityEngine;

namespace CupkekGames.Core
{
  public class MonoGameObjectPool : MonoObjectPoolBase<GameObject>
  {
    [SerializeField] private GameObject _prefab;

    public override GameObject CreatePooledObject()
    {
      GameObject instance = Instantiate(_prefab);
      instance.gameObject.SetActive(false);

      if (instance.GetComponent<MonoObjectPoolPooled>() == null)
      {
        // Add the script to the targetGameObject
        instance.AddComponent(typeof(MonoObjectPoolPooled));
      }

      return instance;
    }

    public override void OnTakeFromPool(GameObject instance)
    {
      // Instance.gameObject.SetActive(true);
      MonoObjectPoolPooled pooledGameObject = instance.GetComponent<MonoObjectPoolPooled>();
      pooledGameObject.GameObjectPool = this;
    }

    public override void OnReturnToPool(GameObject Instance)
    {
      Instance.gameObject.SetActive(false);
    }

    public override void OnDestroyObject(GameObject Instance)
    {
      Destroy(Instance.gameObject);
    }
  }
}