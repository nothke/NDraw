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
            /// Graphs a function on screen
            /// Warning: Produces GC allocs due to the lambda function.
            /// </summary>
            /// <param name="graphRect">Rect on screen</param>
            /// <param name="unitRect">A rect that defines a size of a unit of graph compared to pixels.
            /// E.g. a 0,0,1,1 unitRect will have it's lower left corner at 0,0 with unit size of 1 pixel. </param>
            /// <param name="func">A function that takes an 'x' and returns a 'y'</param>
            public static void Graph(Rect graphRect, Rect unitRect, System.Func<float, float> func)
            {
                if (!Drawer.Exists) return;

                float prev = 0;
                for (int i = 0; i < graphRect.width; i++)
                {
                    float input = -unitRect.x / unitRect.width + (i / unitRect.width); // z + 
                    float v0 = unitRect.y + func.Invoke(input) * unitRect.height;

                    v0 = Mathf.Clamp(v0,
                        0,
                        graphRect.height);

                    int screeny1 = (int)(graphRect.y + v0);
                    int screeny2 = (int)(graphRect.y + prev);

                    Line(
                        (int)graphRect.x + i, screeny1,
                        (int)graphRect.x + i + 1, screeny2);

                    prev = v0;
                }
            }
        }
    }
}
