#if UNITY_ADDRESSABLES
using System.Collections;
using UnityEngine;

namespace CupkekGames.Core
{
  // Do not use struct, because we need to pass it by reference
  public class SceneLoadTransitionCircle : SceneLoadTransition
  {
    [SerializeField] SceneLoadTransitionCircleUI _circle;
    private Coroutine _coroutine = null;
    public override float GetStartDelay()
    {
      return 1f; // Wait for transition to complete before starting scene unload/load
    }
    public override void FadeIn()
    {
      if (_coroutine != null)
      {
        StopCoroutine(_coroutine);
      }

      _circle.Fadeable.FadeIn();

      _coroutine = StartCoroutine(FadeInLoadingScreen());
    }

    public override void FadeOut()
    {
      if (_coroutine != null)
      {
        StopCoroutine(_coroutine);
      }

      _circle.Fadeable.SetFadedIn();
      _circle.Fadeable.FadeOut();

      CoreEventDatabase.LoadingScreenToggleEvent?.Invoke(false, 0.1f);
    }

    private IEnumerator FadeInLoadingScreen()
    {
      yield return new WaitForSeconds(GetStartDelay());

      CoreEventDatabase.LoadingScreenToggleEvent?.Invoke(true, 0.1f);

      yield return new WaitForSeconds(0.1f);

      _circle.Fadeable.SetFadedOut();
      _coroutine = null;
    }
  }
}
#endif