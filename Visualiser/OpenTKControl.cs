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
        public static Camera camera;

        private static long prevTime;

        public static void SetModel(string name)
        {
            model = modelCollection[name];  //make copy of model
        }

        public static void Initialise()
        {
            openTKWindow = new GLControl(OpenTK.Graphics.GraphicsMode.Default, 3, 3, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible);
            //openTKWindow = new GLControl();

            openTKWindow.Load += OpenTKWindow_Load;
            openTKWindow.Paint += OpenTKWindow_Paint;
            openTKWindow.Resize += openTKWindow_Resize;

            prevTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static void ModelCollectionInit()
        {
            modelCollection.Add("sphere", new Model("Models/Sphere.obj", false));
            modelCollection.Add("cube", new Model("Models/Cube.obj", false));
            modelCollection.Add("cone", new Model("Models/Cone.obj", false));
            modelCollection.Add("icosphere", new Model("Models/Icosphere.obj", false));
            Vector3 pos = new Vector3(0, 0, 10);
            camera = new Camera(pos, 0, 0, 80, 0.1f, 1000, openTKWindow.Width / openTKWindow.Height);
        }

        public static void SetCustomModel(string name, string fileName)
        {
            modelCollection.Add(name, new Model(fileName, false));
        }

        public static void openTKWindow_Resize(object sender, EventArgs e)
        {
            camera.ResetProjMatrix();
        }

        public static void OpenTKWindow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

            shader.Bind();

            long timeNow = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long deltaTime = timeNow - prevTime;
            prevTime = timeNow;

            camera.Update(deltaTime);

            foreach(DataPoint point in MainWindow.frames[MainWindow.currentFrame].dataPoints)
            {
                point.Render();
            }

            shader.Unbind();

            openTKWindow.SwapBuffers();
        }

        public static void OpenTKWindow_Load(object sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.CornflowerBlue);
        }
    }
}
