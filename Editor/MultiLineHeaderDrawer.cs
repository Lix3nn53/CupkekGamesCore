#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CupkekGames.Core.CGEditor
{
    [CustomPropertyDrawer(typeof(MultiLineHeaderAttribute))]
    public class MultiLineHeaderDrawer : DecoratorDrawer
    {
        // Custom colors and border radius
        private Color backgroundColor = new Color(0f, 0f, 0f, 0.4f);  // Light gray background
        private Color borderColor = new Color(0f, 0f, 0f, 0.6f);
        public override void OnGUI(Rect position)
        {
            MultiLineHeaderAttribute header = (MultiLineHeaderAttribute)attribute;

            GUIStyle style = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true,
                padding = new RectOffset(10, 10, 5, 5)  // Padding inside the header box
            };

            DrawBackgroundWithBorderRadius(position);

            EditorGUI.LabelField(position, header.headerText, style);
        }

        public override float GetHeight()
        {
            MultiLineHeaderAttribute header = (MultiLineHeaderAttribute)attribute;
            GUIStyle style = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true,
                padding = new RectOffset(10, 10, 5, 5)
            };

            // Calculate height based on content
            return style.CalcHeight(new GUIContent(header.headerText), EditorGUIUtility.currentViewWidth) + style.padding.top + style.padding.bottom;
        }

        private void DrawBackgroundWithBorderRadius(Rect position)
        {
            // Adjust position to account for padding/border
            Rect adjustedRect = new Rect(position.x, position.y, position.width, position.height);

            // Draw background color
            EditorGUI.DrawRect(adjustedRect, backgroundColor);

            // Draw the border (manually drawing it with rounded corners)
            Handles.color = borderColor;
            Handles.DrawSolidRectangleWithOutline(adjustedRect, Color.clear, borderColor);
            // You can implement rounded corners using `Handles` or other custom drawing methods if needed.
        }
    }
}
#endif