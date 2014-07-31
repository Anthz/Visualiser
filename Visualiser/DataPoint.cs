using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualiser
{
    class DataPoint
    {
        private Vector3 pos;
        float[] dataValues;

        Matrix4 modelMatrix;
        Model model;
        Vector3 color = new Vector3(1.0f, 1.0f, 1.0f);

        public DataPoint(float x, float y, float z, float[] dataValues, Model model)
        {
            pos.X = x;
            pos.Y = y;
            pos.Z = z;
            this.dataValues = dataValues;

            modelMatrix = Matrix4.CreateTranslation(pos);
            this.model = model;
        }
        public void Render()
        {
            OpenTKControl.shader.SetUniform("ModelMatrix", modelMatrix);
            GL.BindVertexArray(model.vertexArrayID);
            OpenTKControl.shader.SetUniform("InColor", color);
            GL.DrawArrays(BeginMode.Triangles, 0, model.vertexCount);
            GL.BindVertexArray(0);
        }

        public void Update()
        {

        }
    }
}
