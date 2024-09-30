#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CupkekGames.Core.CGEditor
{
    [CustomPropertyDrawer(typeof(MultiLineHeaderAttribute))]
    public class MultiLineHeaderDrawer : DecoratorDrawer
    {
        // Custom colors and border radius
        private Color backgroundColor = new Color(0f, 0f, 0f, 0.2f);  // Light gray background
        private Color borderColor = new Color(0f, 0f, 0f, 0.4f);
        private float margin = 5f;  // Margin around the header

        // Cached height to avoid calling GUI functions in GetHeight
        private float cachedHeight;
        public override void OnGUI(Rect position)
        {
            MultiLineHeaderAttribute header = (MultiLineHeaderAttribute)attribute;

            GUIStyle style = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true,
                padding = new RectOffset(10, 10, 5, 5)  // Padding inside the header box
            };

            // Adjust position to include margin
            Rect marginRect = new Rect(
                position.x + margin,
                position.y + margin,
                position.width - 2 * margin,
                position.height - 2 * margin
            );

            DrawBackgroundWithBorderRadius(marginRect);

            EditorGUI.LabelField(position, header.headerText, style);

            cachedHeight = style.CalcHeight(new GUIContent(header.headerText), position.width) + style.padding.top + style.padding.bottom;
        }

        public override float GetHeight()
        {
            return cachedHeight;
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