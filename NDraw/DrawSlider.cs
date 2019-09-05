using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{
    public static partial class Draw
    {
        public static partial class Screen
        {
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