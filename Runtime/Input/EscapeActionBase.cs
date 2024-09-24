using UnityEngine;
using System;

namespace CupkekGames.Core
{
  public abstract class EscapeActionBase : MonoBehaviour
  {
    private InputEscapeManager _inputEscapeManager;
    public InputEscapeManager InputEscapeManager => _inputEscapeManager;
    private Guid _escapeKey;
    public Guid EscapeKey => _escapeKey;

    protected virtual void Awake()
    {
      _inputEscapeManager = ServiceLocator.Get<InputEscapeManager>();

      _escapeKey = Guid.NewGuid();
    }
    protected virtual void OnEnable()
    {
      Push();
    }
    protected virtual void OnDisable()
    {
      _inputEscapeManager.PopWithoutExecute(_escapeKey);
    }

    public void Push()
    {
      _inputEscapeManager.Push(OnEscape, _escapeKey);
    }

    protected abstract void OnEscape();

    public void Escape()
    {
      _inputEscapeManager.Pop(_escapeKey);
    }
  }
}