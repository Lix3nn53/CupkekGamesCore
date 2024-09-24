using UnityEngine;

namespace CupkekGames.Core
{
  public abstract class FactoryMono<T> : MonoBehaviour, IFactory<T>
  {
    public abstract T Create();
  }
}