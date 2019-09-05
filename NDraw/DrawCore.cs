using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{
    public static partial class Draw
    {
#if NET46
        internal readonly struct ColorIndex
#else
        internal struct ColorIndex
#endif
        {
            public readonly int i;
            public readonly Color c;

            public ColorIndex(int i, Color c)
            {
                this.i = i;
                this.c = c;
            }
        }

        internal static List<Vector3> screenPoints = new List<Vector3>();
        internal static List<Vector3> worldPoints = new List<Vector3>();
        internal static List<Vector3> screenTrisPoints = new List<Vector3>();

        internal static List<ColorIndex> screenColorIndices = new List<ColorIndex>();
        internal static List<ColorIndex> worldColorIndices = new List<ColorIndex>();
        internal static List<ColorIndex> screenTrisColorIndices = new List<ColorIndex>();

        internal static void Clear()
        {
            screenPoints.Clear();
            screenColorIndices.Clear();

            worldPoints.Clear();
            worldColorIndices.Clear();

            screenTrisPoints.Clear();
            screenTrisColorIndices.Clear();
        }

        public static partial class Screen
        {
            public static void SetColor(Color color)
            {
                if (!Drawer.Exists) return;

                int pointIndex = screenPoints.Count;
                int lastci = screenColorIndices.Count - 1;

                ColorIndex ci = new ColorIndex(pointIndex, color);

                // Overwrite if last index is the same as this one
                if (screenColorIndices.Count > 0 &&
                    screenColorIndices[lastci].i == pointIndex)
                {
                    screenColorIndices[lastci] = ci;
                    return;
                }

                screenColorIndices.Add(ci);
            }

            public static void SetFillColor(Color color)
            {
                if (!Drawer.Exists) return;

                int pointIndex = screenTrisPoints.Count;
                int lastci = screenTrisColorIndices.Count - 1;

                ColorIndex ci = new ColorIndex(screenTrisPoints.Count, color);

                // Overwrite if last index is the same as this one
                if (screenTrisColorIndices.Count > 0 &&
                    screenTrisColorIndices[lastci].i == pointIndex)
                {
                    screenTrisColorIndices[lastci] = ci;
                    return;
                }

                screenTrisColorIndices.Add(ci);
            }
        }

        public static partial class World
        {
            public static void SetColor(Color color)
            {
                if (!Drawer.Exists) return;

                ColorIndex ci = new ColorIndex(worldPoints.Count, color);
                worldColorIndices.Add(ci);
            }
        }
    }
}