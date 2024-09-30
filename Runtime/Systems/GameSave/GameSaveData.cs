using System;

namespace CupkekGames.Core
{
  [Serializable]
  public abstract class GameSaveData
  {
    public int Number;
    public DateTime SaveDate;
    public GameSaveData(int number)
    {
      Number = number;
    }
  }
}