using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Timers;

namespace Visualiser
{
    public class Distortion
    {
        public Shader distortionShader;
        public int frameBufferID, renderBufferID, textureID;
        public int width, height;
        public bool initialised;
        public int testTex;

        public Distortion(int width, int height)
        {
            this.width = width;
            this.height = height;
            Init();
        }

        private void Init()
        {
            frameBufferID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferID);

            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            renderBufferID = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBufferID);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, width, height);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureID, 0);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, renderBufferID);

            Console.WriteLine(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer).ToString());

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            distortionShader = new Shader("Shaders/Oculus/empty.shdr", "Shaders/Oculus/barrel.frag", "Shaders/Oculus/barrel.geom");
            testTex = Texture.LoadTexture("Textures/brick.bmp");
            initialised = true;
        }

        public void Start()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferID);
            GL.Viewport(0, 0, width, height);   //half width, render, move, render
        }

        public void Stop()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }

    public static class OpenTKControl
    {
        public static GLControl openTKWindow;
        public static Matrix4 projMatrix;
        public static Shader shader;
        public static Dictionary<string, Model> modelCollection = new Dictionary<string,Model>();
        public static Model model;
        public static Camera camera;
        public static HUD hud;
        public static Distortion distortion;

        public static bool RiftEnabled
        {
            get { return riftEnabled; }
            set
            {
                riftEnabled = value;
                if(value == true)
                    camera.ResetRiftOrientation();
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
                    shader = new Shader("Shaders/toonShader.vert", "Shaders/toonShader.frag");
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
            if(e.KeyChar == 'w')
            {
                camera.pos.Z -= 2;
                openTKWindow.Invalidate();
            }
            if(e.KeyChar == 's')
            {
                camera.pos.Z += 2;
                openTKWindow.Invalidate();
            }
            if(e.KeyChar == 'a')
            {
                camera.XAngle += (float)Math.PI / 20;
                openTKWindow.Invalidate();
            }
            if(e.KeyChar == 'd')
            {
                camera.XAngle -= (float)Math.PI / 20;
                openTKWindow.Invalidate();
            }
            if(e.KeyChar == 'q')
            {
                camera.pos.Y += 2;
                openTKWindow.Invalidate();
            }
            if(e.KeyChar == 'e')
            {
                camera.pos.Y -= 2;
                openTKWindow.Invalidate();
            }
            if (e.KeyChar == 'r')
            {
                camera.ResetRiftOrientation();
            }
            if (e.KeyChar == 'l')
            {
                Console.WriteLine(GL.GetError());
            }
        }

        public static void ModelCollectionInit()
        {
            modelCollection.Add("sphere", new Model("Models/Sphere.obj", false));
            modelCollection.Add("cube", new Model("Models/Cube.obj", false));
            modelCollection.Add("cone", new Model("Models/Cone.obj", false));
            modelCollection.Add("icosphere", new Model("Models/Icosphere.obj", false));
            camera = new Camera(new Vector3(0, 40, 40), 1.57f, 0, (float)Math.PI / 2, 0.1f, 1000.0f, (float)openTKWindow.Width / (float)openTKWindow.Height);
            hud = new HUD();
            distortion = new Distortion(1280, 800);
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
            hud.ResetOrthoMatrix();
        }

        public static void OpenTKWindow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            distortion.Start();

            GL.ClearColor(System.Drawing.Color.Red);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);

            shader.Bind();
            shader.SetUniform("ProjectionMatrix", ref camera.projMatrix);
            shader.SetUniform("lightDir", Vector3.Normalize(camera.pos - new Vector3(0, 0, 0)));  //camera direction

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

            distortion.Stop();

            GL.ClearColor(System.Drawing.Color.Salmon);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);

            int temp = GL.GenVertexArray();
            GL.BindVertexArray(temp);
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, distortion.frameBufferID);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, distortion.textureID);    //test texture works, framebuffer doesn't
            distortion.distortionShader.Bind();
            distortion.distortionShader.SetUniform("Texture", 0);
            GL.DrawArrays(PrimitiveType.Points, 0, 1);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindVertexArray(0);

            //GL.Disable(EnableCap.DepthTest);*/

            //hud.Render();

            openTKWindow.SwapBuffers();
        }

        public static void OpenTKWindow_Load(object sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.CornflowerBlue);
        }
    }
}
