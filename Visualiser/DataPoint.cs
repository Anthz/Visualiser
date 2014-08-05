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
        private float scale;
        float[] dataValues;

        Matrix4 modelMatrix;
        Vector4 color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        public DataPoint(Vector3 pos, float scale, float[] dataValues)
        {
            this.pos = pos;
            this.scale = scale;
            this.dataValues = dataValues;

            modelMatrix = Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(pos);
        }
        public void Render()
        {
            OpenTKControl.shader.SetUniform("ModelMatrix", ref modelMatrix);
            GL.BindVertexArray(OpenTKControl.model.vertexArrayID);
            OpenTKControl.shader.SetUniform("InColor", color);
            GL.DrawArrays(PrimitiveType.Triangles, 0, OpenTKControl.model.vertexCount);
            GL.BindVertexArray(0);
        }

        public void Update()
        {
            //OpenTKControl.shader.SetUniform("ModelMatrix", ref modelMatrix);
        }
    }
}
