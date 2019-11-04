///
/// You can use 2 defines, and add them in Settings > Player > Other Settings > Scripting Define Symbols:
/// 
/// NDRAW_ORHTO_MULTIPLICATION
///     ^ If screen space lines don't draw in some cases (for example in HDRP), try using this define. 
///     Reason: GL.LoadPixelMatrix() doesn't seem to work in some cases so GL.LoadOrtho() needs to be used instead,
///     but there is a tiny cost of multiplying each vertex with screen resolution.
///     
/// NDRAW_UPDATE_IN_COROUTINE
///     ^ In case you are using SRP (URP or HDRP) and you don't see any lines, try using this define.
///     Reason: NDraw typically uses OnPostRender() for drawing, but in SRP this callback is not functional.
///     Instead a coroutine that waits for end of frame is used. IT doesn't produce any GC allocs because
///     WaitForEndOfFrame is cached.
///     

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{

    public class Drawer : MonoBehaviour
    {
        static Drawer e;

        public Material material;
        new Camera camera;

        public static bool Exists { get { return e != null; } }

        static readonly Vector2 one = Vector2.one;

        private void Awake()
        {
            e = this;
        }

        private void Start()
        {
            CreateLineMaterial();

            camera = GetComponent<Camera>();

#if NDRAW_UPDATE_IN_COROUTINE
            StartCoroutine(PostRender());
#endif
        }

        private void OnDestroy()
        {
            Draw.Clear();
        }

        WaitForEndOfFrame wof = new WaitForEndOfFrame();

#if !NDRAW_UPDATE_IN_COROUTINE
        private void OnPostRender()
        {
            if (enabled)
                Render();

            Draw.Clear();
        }
#endif

#if NDRAW_UPDATE_IN_COROUTINE
        IEnumerator PostRender()
        {
            while (true)
            {
                yield return wof;

                if (enabled)
                    Render();

                Draw.Clear();
            }
        }
#endif

        void CreateLineMaterial()
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            material = new Material(shader);
            //material.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            material.SetInt("_ZWrite", 0);

            // makes the material draw on top of everything
            material.SetInt("_ZTest", 0);
        }

        void Render()
        {
            material.SetPass(0);

            //-------------
            // WORLD SPACE
            //-------------

            GL.PushMatrix();
            GL.LoadProjectionMatrix(camera.projectionMatrix);
            GL.modelview = camera.worldToCameraMatrix;

            GL.Begin(GL.LINES);
            GL.Color(Color.white);
            ProcessPoints(Draw.worldPoints, Draw.worldColorIndices, false);
            GL.End();

            /*
            GL.Begin(GL.TRIANGLES);
            GL.Color(new Color(1f, 1f, 1f, 0.3f));
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(3, 3, 0);
            GL.Vertex3(0, 3, 0);
            GL.End();
            */

            GL.PopMatrix();

            //--------------
            // SCREEN SPACE
            //--------------

            GL.PushMatrix();

#if NDRAW_ORHTO_MULTIPLICATION
            GL.LoadOrtho();
#else
            GL.LoadPixelMatrix();
#endif

            GL.Begin(GL.TRIANGLES);
            ProcessPoints(Draw.screenTrisPoints, Draw.screenTrisColorIndices, true);
            GL.End();

            GL.Begin(GL.LINES);
            ProcessPoints(Draw.screenPoints, Draw.screenColorIndices, true);
            GL.End();

            GL.PopMatrix();

        }

        static void ProcessPoints(List<Vector3> points, List<Draw.ColorIndex> colorIndices, bool screen)
        {
            if (points.Count == 0) return;

            //GL.Color(Color.white);
#if NDRAW_ORHTO_MULTIPLICATION
            Vector2 s = screen ? new Vector2(1.0f / Screen.width, 1.0f / Screen.height) : one;
#endif

            bool hasColors = colorIndices.Count > 0;

            int ci = 0;
            int ct = points.Count;
            for (int i = 0; i < ct; i++)
            {
                // handle color
                if (hasColors && colorIndices[ci].i == i)
                {
                    GL.Color(colorIndices[ci].c);

                    ci++;
                    if (ci >= colorIndices.Count) ci = 0;
                }

                // push vertex
#if NDRAW_ORHTO_MULTIPLICATION
                if (screen)
                    GL.Vertex(points[i] * s);
                else
                    GL.Vertex(points[i]);
#else
                GL.Vertex(points[i]);
#endif
            }
        }
    }
}