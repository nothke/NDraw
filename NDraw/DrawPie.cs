using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{
    public static partial class Draw
    {
        public static partial class Screen
        {
            public static void Pie(int x, int y, float innerRadius, float outerRadius, float value)
            {
                if (!Drawer.Exists) return;

                Vector2 ci_in = new Vector2(x, y + innerRadius);
                Vector2 ci_out = new Vector2(x, y - outerRadius);

                Vector2 ci0_in = ci_in;
                Vector2 ci0_out = ci_out;

                Vector2 cil_in = ci_in;
                Vector2 cil_out = ci_out;

                float s = Mathf.Sign(value);
                float add = 0.3f * s;
                float limit = Mathf.Abs(2 * Mathf.PI * value);

                for (float theta = 0.0f; Mathf.Abs(theta) < limit; theta += add)
                {

                    ci_in = new Vector2(
                        x + (Mathf.Sin(theta) * innerRadius),
                        y - (Mathf.Cos(theta) * innerRadius));

                    ci_out = new Vector2(
                        x + (Mathf.Sin(theta) * outerRadius),
                        y - (Mathf.Cos(theta) * outerRadius));

                    screenTrisPoints.Add(cil_in);
                    screenTrisPoints.Add(cil_out);
                    screenTrisPoints.Add(ci_out);

                    screenTrisPoints.Add(ci_out);
                    screenTrisPoints.Add(ci_in);
                    screenTrisPoints.Add(cil_in);

                    // previous points
                    cil_in = ci_in;
                    cil_out = ci_out;
                }



                // last segment
                ci_in = new Vector2(
                    x + (Mathf.Sin(limit * s) * innerRadius),
                    y - (Mathf.Cos(limit * s) * innerRadius));

                ci_out = new Vector2(
                    x + (Mathf.Sin(limit * s) * outerRadius),
                    y - (Mathf.Cos(limit * s) * outerRadius));

                screenTrisPoints.Add(cil_in);
                screenTrisPoints.Add(cil_out);
                screenTrisPoints.Add(ci_out);

                screenTrisPoints.Add(ci_out);
                screenTrisPoints.Add(ci_in);
                screenTrisPoints.Add(cil_in);
            }
        }
    }
}