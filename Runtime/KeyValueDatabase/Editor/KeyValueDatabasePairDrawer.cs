#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CupkekGames.Core.CGEditor
{
  [CustomPropertyDrawer(typeof(KeyValuePair<,>))]
  public class KeyValueDatabasePairDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      // Begin property drawing
      EditorGUI.BeginProperty(position, label, property);

      // Calculate rects
      float quarterWidth = (position.width - 10) / 4;

      // Draw properties
      SerializedProperty keyProp = property.FindPropertyRelative("Key");
      SerializedProperty valueProp = property.FindPropertyRelative("Value");

      float keyWidth;
      float valueWidth;

      if (valueProp.hasVisibleChildren)
      {
        keyWidth = quarterWidth - 5;
        valueWidth = quarterWidth * 3 - 5;
      }
      else
      {
        keyWidth = quarterWidth * 2 - 5;
        valueWidth = quarterWidth * 2 - 5;
      }

      Rect keyRect = new Rect(position.x, position.y, keyWidth, position.height);
      Rect valueRect = new Rect(position.x + keyWidth + 10, position.y, valueWidth, position.height);

      // Ensure the fields can handle children correctly
      float keyHeight = EditorGUI.GetPropertyHeight(keyProp, GUIContent.none, true);
      float valueHeight = EditorGUI.GetPropertyHeight(valueProp, GUIContent.none, true);

      // Adjust position height based on the largest property
      float totalHeight = Mathf.Max(keyHeight, valueHeight);
      position.height = totalHeight;

      // Draw the properties again with the adjusted height
      keyRect.height = keyHeight;
      valueRect.height = valueHeight;

      DrawPropertyWithoutLabel(keyRect, keyProp);
      DrawPropertyWithoutLabel(valueRect, valueProp);

      // End property drawing
      EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      SerializedProperty keyProp = property.FindPropertyRelative("Key");
      SerializedProperty valueProp = property.FindPropertyRelative("Value");

      // Calculate heights for the properties
      float keyHeight = EditorGUI.GetPropertyHeight(keyProp, GUIContent.none, true);
      float valueHeight = EditorGUI.GetPropertyHeight(valueProp, GUIContent.none, true);

      // Return the total height needed, including spacing between properties
      return Mathf.Max(keyHeight, valueHeight) + 2f; // Adjust 2f to desired spacing
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
      // Create property container element.
      var container = new VisualElement();

      // Create property fields.
      var keyField = new PropertyField(property.FindPropertyRelative("Key"));
      var valueField = new PropertyField(property.FindPropertyRelative("Value"));

      // Add fields to the container.
      container.Add(keyField);
      container.Add(valueField);

      return container;
    }

    private void DrawPropertyWithoutLabel(Rect rect, SerializedProperty property)
    {
      switch (property.propertyType)
      {
        case SerializedPropertyType.Generic:
          EditorGUI.PropertyField(rect, property, new GUIContent(property.displayName), true);
          break;
        default:
          EditorGUI.PropertyField(rect, property, GUIContent.none);
          break;
      }
    }
  }
}
#endif
