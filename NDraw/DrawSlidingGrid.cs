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
            /// 
            /// </summary>
            /// <param name="rect">Rect that defines where to draw the grid on screen, in pixels</param>
            /// <param name="limits">The area that encompasses the current value</param>
            /// <param name="unit">The offset and separation of single cell</param>
            public static void SlidingGrid(Rect rect, Rect unit)
            {
                if (!Drawer.Exists) return;

                if (unit.height > 1)
                {
                    float off = unit.y % unit.height;
                    if (unit.y < 0) off += unit.height;

                    float start = rect.y + off;
                    float add = unit.height;

                    for (float y = start; y < rect.yMax; y += add)
                    {
                        screenPoints.Add(new Vector3(rect.x, y));
                        screenPoints.Add(new Vector3(rect.xMax, y));
                    }
                }

                if (unit.width > 1)
                {
                    float off = unit.x % unit.width;
                    if (unit.x < 0) off += unit.width;

                    float start = rect.x + off;
                    float add = unit.width;

                    for (float x = start; x < rect.xMax; x += add)
                    {
                        screenPoints.Add(new Vector3(x, rect.y));
                        screenPoints.Add(new Vector3(x, rect.yMax));
                    }
                }
            }
        }
    }
}
