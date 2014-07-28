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
            openTKWindow.Resize += openTKWindow_Resize;
        }

        public static void openTKWindow_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, openTKWindow.Size.Width, openTKWindow.Size.Height);
            //Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, openTKWindow.Size.Width / (float)openTKWindow.Size.Height, 1.0f, 64.0f);

            //GL.MatrixMode(MatrixMode.Projection);

            //GL.LoadMatrix(ref projection);
        }

        public static void OpenTKWindow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw a little yellow triangle
            GL.Color3(System.Drawing.Color.Yellow);
            GL.Begin(BeginMode.Triangles);
            GL.Vertex2(0.0, 0.0);
            GL.Vertex2(100.0, 0.0);
            GL.Vertex2(50.0, 100.0);
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
