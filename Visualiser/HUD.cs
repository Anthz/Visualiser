using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Visualiser
{
    public class HUD
    {
        Matrix4 orthoMatrix;
        Shader textShader;
        Text testText;
        int fontAtlasID;
        int id, vertID;

        public HUD()
        {
            Initialise();
        }

        private void Initialise()
        {
            fontAtlasID = Texture.LoadTexture("Fonts/nulshock_atlas.bmp");
            textShader = new Shader("Shaders/textShader.vert", "Shaders/textShader.frag");
            testText = new Text(fontAtlasID, ref textShader);
            testText.ToText("Test", OpenTKControl.openTKWindow.Width / 2, OpenTKControl.openTKWindow.Height / 2, 5.0f);

            //id = GL.GenVertexArray();
            //GL.BindVertexArray(id);

            //vertID = GL.GenBuffer();
        }

        public void ResetOrthoMatrix()
        {
            textShader.Bind();
            //GL.Viewport(0, 0, OpenTKControl.openTKWindow.Size.Width, OpenTKControl.openTKWindow.Size.Height);
            orthoMatrix = Matrix4.CreateOrthographic(OpenTKControl.openTKWindow.Size.Width, OpenTKControl.openTKWindow.Size.Height, -100.0f, 100.0f);
            textShader.SetUniform("ProjectionMatrix", ref orthoMatrix);
            textShader.Unbind();
        }

        public void Render()
        {
            textShader.Bind();
            textShader.SetUniform("ProjectionMatrix", ref orthoMatrix);
            testText.Render();
            textShader.Unbind();
        }
    }
}
