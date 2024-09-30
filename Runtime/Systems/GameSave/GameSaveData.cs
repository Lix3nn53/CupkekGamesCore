using System;

namespace CupkekGames.Core
{
  [Serializable]
  public abstract class GameSaveData
  {
    public int Number;
    public DateTime SaveDate;

    // Reflection is used to construct this object.
    // Make sure not to change the constructor parameters when deriving from this class.
    public GameSaveData(string fileName, int number)
    {
      Number = number;
    }
  }
}
