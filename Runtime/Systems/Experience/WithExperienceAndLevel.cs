namespace CupkekGames.Core
{
  public abstract class WithExperienceAndLevel : WithExperience
  {
    // Level may be lower than the experience can reach, this allows level up to be triggered by player.
    public int Level = 1;

    public override int GetLevel()
    {
      return Level;
    }

    public bool CanLevelUp()
    {
      return TotalExp >= GetTotalRequiredExperience(GetLevel() + 1);
    }

    public bool TryLevelUp()
    {
      if (CanLevelUp())
      {
        Level++;
        return true;
      }

      return false;
    }
  }
}
