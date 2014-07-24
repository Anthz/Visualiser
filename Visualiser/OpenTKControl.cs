using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Visualiser
{
    public static class OpenTKControl
    {
        public static GLControl openTKWindow;

        public static void Initialise()
        {
            openTKWindow = new GLControl();

            openTKWindow.Load += OpenTKWindow_Load;
            openTKWindow.Paint += OpenTKWindow_Paint;
        }

        public static void OpenTKWindow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw a little yellow triangle
            GL.Color3(System.Drawing.Color.Yellow);
            GL.Begin(BeginMode.Triangles);
            GL.Vertex2(200, 50);
            GL.Vertex2(200, 200);
            GL.Vertex2(100, 50);
            GL.End();

            openTKWindow.SwapBuffers();
        }

        public static void OpenTKWindow_Load(object sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.CornflowerBlue);

            int w = openTKWindow.Width;
            int h = openTKWindow.Height;

            // Set up initial modes
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(0, 0, w, h);
        }
    }
}
