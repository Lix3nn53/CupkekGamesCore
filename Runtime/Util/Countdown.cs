using System;
using System.Collections;
using UnityEngine;

namespace CupkekGames.Core
{
  public class Countdown
  {
    [Header("Settings")]
    private int _value = 1000;
    public int Value => _value;
    private int _target = 0;
    private int _interval;
    public event Action<int> OnStart;
    public event Action<int> OnTick;
    public event Action OnComplete;
    private Coroutine _countdownCoroutine;
    private MonoBehaviour _runner;

    public Countdown(MonoBehaviour runner, int startValue, int target = 0, int interval = 1000)
    {
      _runner = runner;
      _value = startValue * 1000;
      _target = target;
      _interval = interval;
    }

    public void SetValue(int value)
    {
      _value = value;
    }

    private IEnumerator StartCountdown()
    {
      if (_value <= _target)
      {
        yield break;
      }

      while (_value > _target)
      {
        yield return new WaitForSeconds(_interval / 1000f); // Convert milliseconds to seconds

        _value -= _interval;
        OnTick?.Invoke(_value);
      }

      OnComplete?.Invoke();
    }

    public void Resume()
    {
      OnStart?.Invoke(_value);

      if (_countdownCoroutine != null)
      {
        _runner.StopCoroutine(_countdownCoroutine);
      }

      _countdownCoroutine = _runner.StartCoroutine(StartCountdown());
    }

    public void Stop()
    {
      if (_countdownCoroutine != null)
      {
        _runner.StopCoroutine(_countdownCoroutine);
        _countdownCoroutine = null;
      }
    }

    public bool IsPlaying()
    {
      return _countdownCoroutine != null;
    }
  }
}
