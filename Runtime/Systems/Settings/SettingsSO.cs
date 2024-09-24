using UnityEngine;

namespace CupkekGames.Core
{
  [CreateAssetMenu(fileName = "Settings", menuName = "CupkekGames/Settings")]
  public class SettingsSO : ScriptableObject
  {
    public SettingsData Data = new SettingsData();
  }
}