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
        public static Matrix4 projMatrix;
        public static Shader shader;
        public static Dictionary<string, Model> modelCollection = new Dictionary<string,Model>();
        public static Model model;

        public static void SetModel(string name)
        {
            model = modelCollection[name];  //make copy of model
        }

        public static void Initialise()
        {
            openTKWindow = new GLControl();

            openTKWindow.Load += OpenTKWindow_Load;
            openTKWindow.Paint += OpenTKWindow_Paint;
            openTKWindow.Resize += openTKWindow_Resize;
        }

        public static void ModelCollectionInit()
        {
            modelCollection.Add("sphere", new Model("Models/Sphere.obj", false));
            modelCollection.Add("cube", new Model("Models/Cube.obj", false));
            modelCollection.Add("cone", new Model("Models/Cone.obj", false));
            modelCollection.Add("icosphere", new Model("Models/Icosphere.obj", false));
        }

        public static void SetCustomModel(string name, string fileName)
        {
            modelCollection.Add(name, new Model(fileName, false));
        }

        public static void openTKWindow_Resize(object sender, EventArgs e)
        {
            int w = openTKWindow.Width;
            int h = openTKWindow.Height;

            GL.Viewport(0, 0, openTKWindow.Size.Width, openTKWindow.Size.Height);
            //Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, openTKWindow.Size.Width / (float)openTKWindow.Size.Height, 1.0f, 64.0f);

            //GL.MatrixMode(MatrixMode.Projection);

            //GL.LoadMatrix(ref projection);
        }

        public static void OpenTKWindow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

            shader.Bind();
            shader.SetUniform("ProjectionMatrix", projMatrix);
            //add new shader code

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
