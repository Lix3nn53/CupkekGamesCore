#if UNITY_ADDRESSABLES
namespace CupkekGames.Core
{
  // Do not use struct, because we need to pass it by reference
  public class SceneLoadTransitionInstant : SceneLoadTransition
  {
    public override float GetStartDelay()
    {
      return 0;
    }
    public override void FadeIn()
    {
      CoreEventDatabase.LoadingScreenToggleEvent?.Invoke(true, 0);
    }

    public override void FadeOut()
    {
      CoreEventDatabase.LoadingScreenToggleEvent?.Invoke(false, 0);
    }
  }
}
#endif