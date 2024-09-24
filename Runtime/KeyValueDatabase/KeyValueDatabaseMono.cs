using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CupkekGames.Core
{
  public abstract class KeyValueDatabaseMono<TKey, TValue> : MonoBehaviour, IKeyValueDatabase<TKey, TValue>
  {
    [SerializeField] private KeyValueDatabase<TKey, TValue> _pairs = new();

    public Dictionary<TKey, TValue> Dictionary => _pairs.Dictionary;

    protected virtual void Awake()
    {
      _pairs.InitializeDictionary();
    }

    public bool EditorHasKey(TKey key)
    {
      return _pairs.EditorHasKey(key);
    }

    public bool EditorAdd(TKey key, TValue value)
    {
      return _pairs.EditorAdd(key, value);
    }

    public System.Collections.Generic.KeyValuePair<TKey, TValue> GetRandomPair()
    {
      return _pairs.GetRandomPair();
    }
    public void EditorClear()
    {
      _pairs.EditorClear();
    }
    public bool EditorHasDuplicateKeys()
    {
      return _pairs.EditorHasDuplicateKeys();
    }
  }
}