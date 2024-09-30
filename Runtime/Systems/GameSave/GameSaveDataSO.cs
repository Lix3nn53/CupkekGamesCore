using System;
using UnityEngine;

namespace CupkekGames.Core
{
  public abstract class GameSaveDataSO<TSaveData> : ScriptableObject where TSaveData : GameSaveData
  {
    [SerializeField] public TSaveData Data;
  }
}