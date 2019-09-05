using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{
    public static partial class Draw
    {
        public static partial class Screen
        {
            public static void Rect(Rect rect)
            {
                if (!Drawer.Exists) return;

                Rect(rect.x, rect.y, rect.width, rect.height);
            }

            public static void Rect(float x, float y, float width, float height)
            {
                if (!Drawer.Exists) return;

                screenPoints.Add(new Vector2(x, y));
                screenPoints.Add(new Vector2(x + width, y));

                screenPoints.Add(new Vector2(x + width, y));
                screenPoints.Add(new Vector2(x + width, y + height));

                screenPoints.Add(new Vector2(x + width, y + height));
                screenPoints.Add(new Vector2(x, y + height));

                screenPoints.Add(new Vector2(x, y + height));
                screenPoints.Add(new Vector2(x, y));
            }

            public static void Circle(Vector2 center, float pixelRadius)
            {
                Circle(center.x, center.y, pixelRadius);
            }

            public static void Circle(float centerX, float centerY, float pixelRadius)
            {
                if (!Drawer.Exists) return;

                Vector2 size = new Vector2(pixelRadius, pixelRadius);
                Vector2 center = new Vector2(centerX, centerY);

                Ellipse(center, size);
            }

            public static void Ellipse(Vector2 center, Vector2 size)
            {
                if (!Drawer.Exists) return;

                float radX = size.x;
                float radY = size.y;

                Vector2 ci = new Vector2(
                    center.x + (1 * radX),
                    center.y);

                Vector2 ci0 = ci;

                for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.1f)
                {
                    screenPoints.Add(ci);

                    ci = new Vector2(center.x + (Mathf.Cos(theta) * radX), center.y + (Mathf.Sin(theta) * radY));

                    screenPoints.Add(ci);
                }

                // close
                screenPoints.Add(ci);
                screenPoints.Add(ci0);
            }

            public static void Grid(int xLineNum, int yLineNum, Rect rect)
            {
                if (!Drawer.Exists) return;

                float add = rect.height / yLineNum;
                for (int i = 0; i <= yLineNum; i++)
                {
                    float y = rect.yMax - i * add;
                    screenPoints.Add(new Vector2(rect.x, y));
                    screenPoints.Add(new Vector2(rect.xMax, y));
                }

                add = rect.width / yLineNum;
                for (int i = 0; i <= yLineNum; i++)
                {
                    float x = rect.x + i * add;
                    screenPoints.Add(new Vector2(x, rect.y));
                    screenPoints.Add(new Vector2(x, rect.yMax));
                }
            }

            // FILLED

            public static void FillRect(Rect rect)
            {
                if (!Drawer.Exists) return;

                FillRect(rect.x, rect.y, rect.width, rect.height);
            }

            public static void FillRect(float x, float y, float width, float height)
            {
                if (!Drawer.Exists) return;

                Vector3 p0 = new Vector3(x, y);
                Vector3 p1 = new Vector3(x + width, y);
                Vector3 p2 = new Vector3(x, y + height);
                Vector3 p3 = new Vector3(x + width, y + height);

                screenTrisPoints.Add(p0);
                screenTrisPoints.Add(p1);
                screenTrisPoints.Add(p2);

                screenTrisPoints.Add(p1);
                screenTrisPoints.Add(p3);
                screenTrisPoints.Add(p2);
            }

            public static void FillTriangle(Vector2 p1, Vector2 p2, Vector2 p3)
            {
                if (!Drawer.Exists) return;

                screenTrisPoints.Add(p1);
                screenTrisPoints.Add(p2);
                screenTrisPoints.Add(p3);
            }

            public static void FillFanPolygon(Vector2[] points)
            {
                if (!Drawer.Exists) return;
                if (points == null || points.Length < 2) return;

                Vector3 p0 = points[0];

                for (int i = 1; i < points.Length - 1; i++)
                {
                    screenTrisPoints.Add(p0);
                    screenTrisPoints.Add(points[i]);
                    screenTrisPoints.Add(points[i + 1]);
                }
            }

            public static void FillFanPolygon(List<Vector2> points)
            {
                if (!Drawer.Exists) return;
                if (points == null || points.Count < 2) return;

                Vector3 p0 = points[0];

                int pct = points.Count;
                for (int i = 1; i < pct - 1; i++)
                {
                    screenTrisPoints.Add(p0);
                    screenTrisPoints.Add(points[i]);
                    screenTrisPoints.Add(points[i + 1]);
                }
            }
        }

        public static partial class World
        {
            public static void Cube(Vector3 center, Vector3 size, Vector3 forward, Vector3 up)
            {
                if (!Drawer.Exists) return;

                forward = forward.normalized;
                up = Vector3.ProjectOnPlane(up, forward).normalized;
                Vector3 right = Vector3.Cross(forward, up);

                Vector3 frw = forward * size.z * 0.5f;
                Vector3 rgt = right * size.x * 0.5f;
                Vector3 upw = up * size.y * 0.5f;

                // vertical lines
                worldPoints.Add(center - frw - rgt - upw);
                worldPoints.Add(center - frw - rgt + upw);

                worldPoints.Add(center - frw + rgt - upw);
                worldPoints.Add(center - frw + rgt + upw);

                worldPoints.Add(center + frw - rgt - upw);
                worldPoints.Add(center + frw - rgt + upw);

                worldPoints.Add(center + frw + rgt - upw);
                worldPoints.Add(center + frw + rgt + upw);

                // horizontal lines
                worldPoints.Add(center - frw - rgt - upw);
                worldPoints.Add(center - frw + rgt - upw);

                worldPoints.Add(center - frw - rgt + upw);
                worldPoints.Add(center - frw + rgt + upw);

                worldPoints.Add(center + frw - rgt - upw);
                worldPoints.Add(center + frw + rgt - upw);

                worldPoints.Add(center + frw - rgt + upw);
                worldPoints.Add(center + frw + rgt + upw);

                // forward lines
                worldPoints.Add(center - frw - rgt - upw);
                worldPoints.Add(center + frw - rgt - upw);

                worldPoints.Add(center - frw + rgt - upw);
                worldPoints.Add(center + frw + rgt - upw);

                worldPoints.Add(center - frw - rgt + upw);
                worldPoints.Add(center + frw - rgt + upw);

                worldPoints.Add(center - frw + rgt + upw);
                worldPoints.Add(center + frw + rgt + upw);
            }

            public static void Circle(Vector3 center, float radius, Vector3 normal)
            {
                if (!Drawer.Exists) return;

                normal = normal.normalized;
                Vector3 forward = normal == Vector3.up ?
                    Vector3.ProjectOnPlane(Vector3.forward, normal).normalized :
                    Vector3.ProjectOnPlane(Vector3.up, normal);

                //Vector3 right = Vector3.Cross(normal, forward);

                Vector3 ci = center + forward * radius;
                Vector3 c0 = ci;

                for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.01f)
                {
                    //Vector3 ci = center + forward * Mathf.Cos(theta) * radius + right * Mathf.Sin(theta) * radius;

                    worldPoints.Add(ci);

                    Vector3 angleDir = Quaternion.AngleAxis(theta * Mathf.Rad2Deg, normal) * forward;
                    ci = center + angleDir.normalized * radius;

                    worldPoints.Add(ci);

                    //if (theta != 0)
                    //GL.Vertex(ci);
                }

                //worldPoints.Add(ci);
                //worldPoints.Add(c0);
            }

            public static void Helix(Vector3 p1, Vector3 p2, Vector3 forward, float radius, float angle)
            {
                if (!Drawer.Exists) return;

                Vector3 diff = p2 - p1;
                Vector3 normal = diff.normalized;

                forward = Vector3.ProjectOnPlane(forward, normal).normalized;

                //Vector3 right = Vector3.Cross(normal, forward);

                Vector3 ci = p1 + forward * radius;

                float lengthFactor = diff.magnitude;

                for (float f = 0.0f; f <= 1.0f; f += 0.02f)
                {
                    float theta = f * angle * Mathf.Deg2Rad;

                    //Vector3 ci = center + forward * Mathf.Cos(theta) * radius + right * Mathf.Sin(theta) * radius;

                    worldPoints.Add(ci);

                    Vector3 offset = normal * f * lengthFactor;

                    Vector3 angleDir = Quaternion.AngleAxis(theta * Mathf.Rad2Deg, normal) * forward;
                    ci = p1 + angleDir.normalized * radius + offset;

                    worldPoints.Add(ci);

                    //if (theta != 0)
                    //GL.Vertex(ci);
                }

                //worldPoints.Add(ci);
                //worldPoints.Add(c0);
            }

            public static void Spiral(Vector3 position, Vector3 normal, Vector3 forward, float radius)
            {
                const int count = 80;
                const float angle = -17.453f;
                float add = radius / count;

                Vector3 lastP = Vector3.zero;

                for (int i = 0; i < count; i++)
                {
                    Vector3 p = forward * add * i;
                    p = Quaternion.AngleAxis(angle * i, normal) * p;
                    Line(position + lastP, position + p);
                    lastP = p;
                }
            }
        }
    }
}