using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CupkekGames.Core
{
  [Serializable]
  public class KeyValueDatabase<TKey, TValue> : IKeyValueDatabase<TKey, TValue>, ISerializationCallbackReceiver
  {
    [NonSerialized] private Dictionary<TKey, TValue> _dictionary;
    public Dictionary<TKey, TValue> Dictionary
    {
      get
      {
        return _dictionary;
      }
    }
    [SerializeField] private List<KeyValuePair<TKey, TValue>> _list = new();

    public KeyValueDatabase()
    {
      InitializeDictionary();
    }

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
      InitializeDictionary();
    }

    public void InitializeDictionary()
    {
      _dictionary = new Dictionary<TKey, TValue>();
      foreach (var pair in _list)
      {
        _dictionary.TryAdd(pair.Key, pair.Value);
      }
    }
    // Editor
    public bool EditorHasKey(TKey key)
    {
      foreach (var pair in _list)
      {
        if (pair.Key.Equals(key))
        {
          return true;
        }
      }

      return false;
    }

    public bool EditorAdd(TKey key, TValue value)
    {
      if (EditorHasKey(key))
      {
        return false;
      }

      _list.Add(new KeyValuePair<TKey, TValue>
      {
        Key = key,
        Value = value
      });

      return true;
    }

    public bool EditorHasDuplicateKeys()
    {
      HashSet<TKey> keys = new HashSet<TKey>();

      foreach (var pair in _list)
      {
        // If the key is already in the set, it's a duplicate
        if (!keys.Add(pair.Key))
        {
          return true; // Duplicate found
        }
      }

      return false; // No duplicates
    }

    public void EditorClear()
    {
      _list.Clear();
    }

    // Runtime

    public System.Collections.Generic.KeyValuePair<TKey, TValue> GetRandomPair()
    {
      return _dictionary.ElementAt(UnityEngine.Random.Range(0, _dictionary.Count));
    }
  }
}