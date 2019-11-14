using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{
    public static partial class Draw
    {
        public static partial class World
        {
            /// <summary>
            /// Draw a conic section, oriented with periapsis at forward.
            /// Draws a circle, ellipse, parabola or hyperbola depending on the eccentricity.
            /// It does not draw the negative part of hyperbola.
            /// </summary>
            /// <param name="center">The focus point</param>
            /// <param name="eccentricity">e=0 is circle, e less than 1 is ellipse, e=1 is parabola, e more than 1 is hyperbola </param>
            /// <param name="semiMajorAxis">Semi major axis of the ellipse, in case e is more than 1 it should be negative</param>
            /// <param name="normal">The normal vector of the section</param>
            /// <param name="periapsisDirection">The direction of the periapsis</param>
            public static void ConicSection(Vector3 center, float eccentricity, float semiMajorAxis, Vector3 normal, Vector3 periapsisDirection, int interpolations)
            {
                float semilatus = eccentricity == 1 ? semiMajorAxis :
                    semiMajorAxis * (1 - eccentricity * eccentricity);

                if (semilatus <= 0) return;
                if (interpolations <= 0) interpolations = 10;
                if (eccentricity < 0) eccentricity = 0;

                periapsisDirection = Vector3.ProjectOnPlane(periapsisDirection, normal).normalized;
                Vector3 right = Vector3.Cross(periapsisDirection, normal).normalized;

                Vector3 prevlp = new Vector3();
                Vector3 prevrp = new Vector3();

                int num = interpolations;
                float thetadiff = Mathf.PI / num;
                float theta = 0;

                bool breakn = false;

                for (int i = 0; i < num + 1; i++)
                {
                    float cosTheta = Mathf.Cos(theta);
                    float r = semilatus / (1 + eccentricity * cosTheta);

                    if (r < 0) { r *= -100; breakn = true; }

                    Vector3 rvec = right * Mathf.Sin(theta) * r;
                    Vector3 fvec = periapsisDirection * cosTheta * r;

                    // Left side
                    Vector3 lp = center - rvec + fvec;

                    if (theta != 0) worldPoints.Add(prevlp);
                    worldPoints.Add(lp);
                    prevlp = lp;

                    // Right side
                    Vector3 rp = center + rvec + fvec;

                    if (theta != 0) worldPoints.Add(prevrp);
                    worldPoints.Add(rp);
                    prevrp = rp;

                    theta += thetadiff;

                    if (breakn) break; // Prevents drawing the negative part of hyperbola
                }
            }

            /// <summary>
            /// Draws an elliptic orbit in world space using periapsis and apoapsis
            /// </summary>
            public static void ConicSectionUsingApses(Vector3 center, float periapsis, float apoapsis, Vector3 normal, Vector3 forward, int interpolations)
            {
                float a = (periapsis + apoapsis) / 2;
                float e = (apoapsis - periapsis) / (apoapsis + periapsis);

                // TODO: Add interpolations
                ConicSection(center, e, a, normal, forward, interpolations);
            }
        }
    }
}
