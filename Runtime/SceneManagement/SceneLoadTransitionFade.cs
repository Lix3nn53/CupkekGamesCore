namespace CupkekGames.Core
{
  // Do not use struct, because we need to pass it by reference
  public class SceneLoadTransitionFade : SceneLoadTransition
  {
    public float _fadeInDuration = 0.5f;
    public float _fadeOutDuration = 1f; // Longer because game lags when loading scene
    public override float GetStartDelay()
    {
      return _fadeInDuration + 0.2f; // add some duration to make sure fade is complete before scene loading starts
    }
    public override void FadeIn()
    {
      CoreEventDatabase.LoadingScreenToggleEvent?.Invoke(true, _fadeInDuration);
    }

    public override void FadeOut()
    {
      CoreEventDatabase.LoadingScreenToggleEvent?.Invoke(false, _fadeOutDuration);
    }
  }
}