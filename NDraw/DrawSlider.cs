using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{
    public static partial class Draw
    {
        public static partial class Screen
        {
            /// <summary>
            /// Draws a rect that gets "filled" similar to a UI slider. Value is in 0-1
            /// </summary>
            /// <param name="value">In 0-1</param>
            /// <param name="x">Rect x position</param>
            /// <param name="y">Rect y position</param>
            /// <param name="color"></param>
            /// <param name="width">Width of the rect</param>
            public static void Slider(float value, int x, int y, Color color, int width)
            {
                if (!Drawer.Exists) return;

                value = Mathf.Clamp01(value);

                Color c = new Color(1, 1, 1, 0.2f);

                SetFillColor(c);
                FillRect(x, y, width, 10);

                SetFillColor(color);
                FillRect(x, y, (int)(value * width), 10);
            }

            /// <summary>
            /// Draws a rect that gets "filled" similar to a UI slider with -1 to 1 values where 0 is in the center.
            /// </summary>
            /// <param name="value">In -1 to 1</param>
            /// <param name="x">Rect x position</param>
            /// <param name="y">Rect y position</param>
            /// <param name="color"></param>
            /// <param name="width">Width of the rect</param>
            public static void MidSlider(float value, int x, int y, Color color, int width)
            {
                if (!Drawer.Exists) return;

                value = Mathf.Clamp(value, -1, 1);

                Color c = new Color(1, 1, 1, 0.2f);

                SetFillColor(c);
                FillRect(x, y, width, 10);

                SetFillColor(color);
                FillRect(x + width * 0.5f, y, (int)(value * 0.5f * width), 10);
            }
        }
    }
}