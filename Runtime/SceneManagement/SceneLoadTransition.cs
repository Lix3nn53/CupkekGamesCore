using UnityEngine;

namespace CupkekGames.Core
{
  // Do not use struct, because we need to pass it by reference
  public abstract class SceneLoadTransition : MonoBehaviour
  {
    private CoreEventDatabase _coreEventDatabase;
    public CoreEventDatabase CoreEventDatabase => _coreEventDatabase;

    public void Initialize(CoreEventDatabase coreEventDatabase)
    {
      _coreEventDatabase = coreEventDatabase;
    }

    /// <summary>
    /// Delay before scene unloading/loading starts to make sure transition is done
    /// </summary>
    /// <returns>Delay</returns>
    public abstract float GetStartDelay();
    public abstract void FadeIn();
    public abstract void FadeOut();
  }
}