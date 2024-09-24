using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

namespace CupkekGames.Core
{
  public class InputEscapeManager : MonoBehaviour
  {
    // Local References
    [SerializeField] private CoreEventDatabase _coreEventDatabase;
    // State
    private List<InputEscapeEntry> _escapeList = new();

    private void OnEnable()
    {
      _coreEventDatabase.InputEscapeEvent += OnEscape;
    }

    private void OnDisable()
    {
      _coreEventDatabase.InputEscapeEvent -= OnEscape;
    }

    public void OnEscape()
    {
      Pop();
    }

    public void Push(Action action, Guid? key = null)
    {
      InputEscapeEntry entry;
      if (key.HasValue)
      {
        entry = new InputEscapeEntry(key.Value, action);
      }
      else
      {
        entry = new InputEscapeEntry(Guid.NewGuid(), action);
      }

      _escapeList.Add(entry);
    }

    public void Pop() => PopInternal(null, true);

    public void Pop(Guid key) => PopInternal(key, true);

    public void PopWithoutExecute(Guid key) => PopInternal(key, false);

    private void PopInternal(Guid? key, bool execute)
    {
      if (_escapeList.Count == 0)
      {
        return;
      }

      InputEscapeEntry? entry = null;
      int index = key.HasValue
          ? _escapeList.FindIndex(e => e.Key == key.Value)
          : _escapeList.Count - 1;

      if (index != -1)
      {
        entry = _escapeList[index];
        _escapeList.RemoveAt(index);
      }

      if (execute && entry.HasValue)
      {
        entry.Value.Action.Invoke();
      }
    }
  }
}
