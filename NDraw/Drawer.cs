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

        private void Awake()
        {
            e = this;
        }

        private void Start()
        {
            CreateLineMaterial();

            camera = GetComponent<Camera>();
            //StartCoroutine(PostRender());
        }

        private void OnDestroy()
        {
            Draw.Clear();
        }

        WaitForEndOfFrame wof = new WaitForEndOfFrame();

        private void OnPostRender()
        {
            if (enabled)
                Render();

            Draw.Clear();
        }

        /*
        IEnumerator PostRender()
        {
            while (true)
            {
                yield return wof;

                if (enabled)
                    Render();

                Draw.Clear();
            }
        }*/

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

            // WORLD SPACE


            GL.PushMatrix();
            GL.LoadProjectionMatrix(camera.projectionMatrix);
            GL.modelview = camera.worldToCameraMatrix;

            GL.Begin(GL.LINES);
            GL.Color(Color.white);
            ProcessPoints(Draw.worldPoints, Draw.worldColorIndices);
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

            // SCREEN SPACE

            GL.PushMatrix();
            GL.LoadPixelMatrix();

            GL.Begin(GL.TRIANGLES);
            ProcessPoints(Draw.screenTrisPoints, Draw.screenTrisColorIndices);
            GL.End();

            GL.Begin(GL.LINES);
            ProcessPoints(Draw.screenPoints, Draw.screenColorIndices);
            GL.End();

            GL.PopMatrix();

        }

        static void ProcessPoints(List<Vector3> points, List<Draw.ColorIndex> colorIndices)
        {
            if (points.Count == 0) return;

            //GL.Color(Color.white);

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
                GL.Vertex(points[i]);
            }
        }
    }
}