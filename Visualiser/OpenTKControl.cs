using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public static bool RiftEnabled
        {
            get { return riftEnabled; }
            set
            {
                riftEnabled = value;
                //camera.rift = new OculusRift();
            }
        }

        private static bool riftEnabled;
        private static long prevTime;

        public static void SetModel(string name)
        {
            model = modelCollection[name];  //make copy of model
            openTKWindow.Invalidate();
        }

        /// <summary>
        /// Change current shader
        /// </summary>
        /// <param name="shaderID">0 - basic shader, 1 - basic rift, 2 - lighting, 3 - lighting rift, 4 - toon, 5 - toon rift</param>
        public static void ChangeShader(int shaderID)
        {
            switch(shaderID)
            {
                case 0:
                    shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }

        public static void Initialise()
        {
            openTKWindow = new GLControl(OpenTK.Graphics.GraphicsMode.Default, 3, 3, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible);

            openTKWindow.Load += OpenTKWindow_Load;
            openTKWindow.Paint += OpenTKWindow_Paint;
            openTKWindow.Resize += openTKWindow_Resize;
            openTKWindow.KeyPress += openTKWindow_KeyPress;

            prevTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        static void openTKWindow_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                camera.pos.Z -= 5;
                openTKWindow.Invalidate();
            }
            if(e.KeyChar == 's')
            {
                camera.pos.Z += 5;
                openTKWindow.Invalidate();
            }
            if (e.KeyChar == 'a')
            {
                camera.XAngle += (float)Math.PI / 10;
                openTKWindow.Invalidate();
            }
            if (e.KeyChar == 'd')
            {
                camera.XAngle -= (float)Math.PI / 10;
                openTKWindow.Invalidate();
            }
            if (e.KeyChar == 'q')
            {
                camera.pos.Y += 5;
                openTKWindow.Invalidate();
            }
            if (e.KeyChar == 'e')
            {
                camera.pos.Y -= 5;
                openTKWindow.Invalidate();
            }
        }

        public static void ModelCollectionInit()
        {
            modelCollection.Add("sphere", new Model("Models/Sphere.obj", false));
            modelCollection.Add("cube", new Model("Models/Cube.obj", false));
            modelCollection.Add("cone", new Model("Models/Cone.obj", false));
            modelCollection.Add("icosphere", new Model("Models/Icosphere.obj", false));
            camera = new Camera(new Vector3(0, 0, 30), 0, 0, (float)Math.PI / 2, 0.1f, 100.0f, (float)openTKWindow.Width / (float)openTKWindow.Height);
        }

        public static void SetCustomModel(string name, string fileName)
        {
            modelCollection.Add(name, new Model(fileName, false));
        }

        public static void openTKWindow_Resize(object sender, EventArgs e)
        {
            shader.Bind();
            camera.ResetProjMatrix();
            shader.Unbind();
        }

        public static void OpenTKWindow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

            shader.Bind();
            shader.SetUniform("ProjectionMatrix", ref camera.projMatrix);

            long timeNow = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long deltaTime = timeNow - prevTime;
            prevTime = timeNow;

            camera.Update(deltaTime);

            if(MainWindow.frames.Count > 0 && MainWindow.correctFormat)
            {
                foreach(DataPoint point in MainWindow.frames[MainWindow.currentFrame - 1].dataPoints)
                {
                    point.Render();
                }
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
