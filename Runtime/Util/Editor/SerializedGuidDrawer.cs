#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace CupkekGames.Core.CGEditor
{
  [CustomPropertyDrawer(typeof(SerializedGuid))]
  public class SerializedGuidDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(position, label, property);

      // Calculate the rect for the property label
      position = EditorGUI.PrefixLabel(position, label);

      // Get the 'guid' field in the GUID struct
      var guidProperty = property.FindPropertyRelative("_value");

      // Buttons and text field
      var buttonWidth = 20f;  // Adjusted width to fit single letters
      var textFieldWidth = position.width - (buttonWidth * 3 + 6);

      var textFieldRect = new Rect(position.x, position.y, textFieldWidth, position.height);
      var generateButtonRect = new Rect(position.x + textFieldWidth + 2, position.y, buttonWidth, position.height);
      var copyButtonRect = new Rect(position.x + textFieldWidth + buttonWidth + 4, position.y, buttonWidth, position.height);
      var resetButtonRect = new Rect(position.x + textFieldWidth + buttonWidth * 2 + 6, position.y, buttonWidth, position.height);

      // Text Field
      guidProperty.stringValue = EditorGUI.TextField(textFieldRect, guidProperty.stringValue);

      // Single Letter Buttons
      if (GUI.Button(generateButtonRect, "G"))  // Generate
      {
        guidProperty.stringValue = System.Guid.NewGuid().ToString();
      }

      if (GUI.Button(copyButtonRect, "C"))  // Copy
      {
        EditorGUIUtility.systemCopyBuffer = guidProperty.stringValue;
      }

      if (GUI.Button(resetButtonRect, "R"))  // Reset
      {
        guidProperty.stringValue = Guid.Empty.ToString();
      }

      EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return EditorGUIUtility.singleLineHeight;
    }
  }
}
#endif
