#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace CupkekGames.Core.CGEditor
{
  [CustomEditor(typeof(FadeableMono), true)]
  public class FadeableMonoEditor : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      // Draw the default inspector first
      DrawDefaultInspector();

      // Reference to the target script
      FadeableMono myScript = (FadeableMono)target;

      if (GUILayout.Button("Fade In"))
      {
        myScript.Fadeable.FadeIn();
      }

      if (GUILayout.Button("Fade Out"))
      {
        myScript.Fadeable.FadeOut();
      }
    }
  }
}
#endif