using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualiser
{
    public class DataPoint
    {
        private Vector3 pos;
        float[] dataValues;

        Matrix4 modelMatrix;
        Vector3 color = new Vector3(1.0f, 1.0f, 1.0f);

        public DataPoint(float x, float y, float z, float[] dataValues)
        {
            pos.X = x;
            pos.Y = y;
            pos.Z = z;
            this.dataValues = dataValues;

            modelMatrix = Matrix4.CreateTranslation(pos);
        }
        public void Render()
        {
            OpenTKControl.shader.SetUniform("ModelMatrix", modelMatrix);
            GL.BindVertexArray(OpenTKControl.model.vertexArrayID);
            OpenTKControl.shader.SetUniform("InColor", color);
            GL.DrawArrays(BeginMode.Triangles, 0, OpenTKControl.model.vertexCount);
            GL.BindVertexArray(0);
        }

        public void Update()
        {
            //OpenTKControl.shader.SetUniform("ModelMatrix", modelMatrix);
        }
    }
}
