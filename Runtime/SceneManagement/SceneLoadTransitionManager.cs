using System.Collections.Generic;
using UnityEngine;

namespace CupkekGames.Core
{
  // Do not use struct, because we need to pass it by reference
  public class SceneLoadTransitionManager : MonoBehaviour
  {
    public KeyValueDatabase<string, SceneLoadTransition> Transitions = new(); // 0 - instant, 1 - fade, 2 - Circle
  }
}