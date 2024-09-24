using System;
using UnityEngine;
using System.Collections;

namespace CupkekGames.Core
{
  [Serializable]
  public class Fadeable
  {
    [Header("Fade Settings")]
    [SerializeField] public float _out = 0f;
    [SerializeField] public float _in = 1f;
    [SerializeField] public float _duration = 1f;
    [SerializeField] public float _fadeInDelay = 0f;
    [SerializeField] public float _fadeOutDelay = 0f;

    [Header("Fade Controller")]
    [SerializeField] private float _value = 0f;
    public float Value => _value;
    public event Action OnApply;
    public event Action OnFadeOutStart;
    public event Action OnFadeOutComplete;
    public event Action OnFadeInStart;
    public event Action OnFadeInComplete;
    // State
    private MonoBehaviour _parent;
    private bool _reversed = false;
    private Coroutine _fadeCoroutine;
    public Fadeable(MonoBehaviour parent)
    {
      Initialize(parent);
    }
    public void Initialize(MonoBehaviour parent)
    {
      _parent = parent;
    }

    public float GetValue()
    {
      return _value;
    }

    public void SetValue(float value)
    {
      _value = value;
    }

    private IEnumerator StartFade(float delay)
    {
      Kill();

      if (delay > 0)
      {
        yield return new WaitForSeconds(delay);
      }

      float startValue;
      float endValue;

      if (_reversed)
      {
        startValue = _in;
        endValue = _out;
      }
      else
      {
        startValue = _out;
        endValue = _in;
      }

      float time = 0f;

      while (time < _duration)
      {
        _value = Mathf.Lerp(startValue, endValue, time / _duration);

        OnApply?.Invoke();

        time += Time.deltaTime;

        yield return null;
      }

      OnCompleteInner();
    }

    private void OnCompleteInner()
    {
      if (_reversed)
      {
        OnFadedOut();
      }
      else
      {
        OnFadedIn();
      }
    }

    public void FadeIn()
    {
      OnFadeInStart?.Invoke();

      _reversed = false;

      _parent.StartCoroutine(StartFade(_fadeInDelay));
    }

    public void FadeOut()
    {
      OnFadeOutStart?.Invoke();

      _reversed = true;

      _parent.StartCoroutine(StartFade(_fadeInDelay));
    }

    public void SetFadedIn()
    {
      _reversed = false;
      OnFadedIn();
    }

    private void OnFadedIn()
    {
      Kill();

      _value = _in;
      OnApply?.Invoke();
      OnFadeInComplete?.Invoke();
    }

    public void SetFadedOut()
    {
      _reversed = false;
      OnFadedOut();
    }

    private void OnFadedOut()
    {
      Kill();

      _value = _out;
      OnApply?.Invoke();
      OnFadeOutComplete?.Invoke();
    }

    public void Kill()
    {
      if (_fadeCoroutine != null)
      {
        _parent.StopCoroutine(_fadeCoroutine);
      }
    }
  }
}