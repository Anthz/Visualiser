using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace Visualiser
{
    public class Text
    {
        int fontAtlasID;
        Shader shader;
        int rows, cols;
        int vertexArrayID, verticesCount;
        int[] vertexBufferIDs;
        bool initialised, highlighted;

        public Text(int fontAtlasID, ref Shader shader)
        {
            this.fontAtlasID = fontAtlasID;
	        this.shader = shader;

	        this.rows = 16;
	        this.cols = 16;
        }

        public Text(int fontAtlasID, int atlasRows, int atlasColumns, ref Shader shader)
        {
            this.fontAtlasID = fontAtlasID;
	        this.shader = shader;

            rows = atlasRows;
            cols = atlasColumns;
        }

        public void ToText(string text, float x, float y, float scale)
        {
            initialised = false;
	        int length = text.Length;

	        List<float> vertices_tmp = new List<float>();
	        List<float> texCoords_tmp = new List<float>();
	        for(int i = 0; i < length; i++)
	        {
		        //get ascii code as int
		        int ascii_code = text[i];

		        //work out row and column in atlas
		        int atlas_col = (ascii_code - ' ') % cols;
		        int atlas_row = (ascii_code - ' ') / cols;
    
		        //work out texture coordinates in atlas
		        float u = (float)(atlas_col * (1.0 / cols));
		        float v = (float)((atlas_row + 1) * (1.0 / rows));

		        float x_pos = x;
		        float y_pos = y - (OpenTKControl.openTKWindow.Height / scale);

		        //move next glyph along to the end of this one
		        if(i + 1 < length)
		        {
			        x += 1.0f * (OpenTKControl.openTKWindow.Width / scale);
		        }

		        /*vertices_tmp[i * 12] = x_pos;
		        vertices_tmp[i * 12 + 1] = y_pos;
		        vertices_tmp[i * 12 + 2] = x_pos;
		        vertices_tmp[i * 12 + 3] = y_pos - (OpenTKControl.openTKWindow.Height / scale);
		        vertices_tmp[i * 12 + 4] = x_pos + (OpenTKControl.openTKWindow.Width / scale);
		        vertices_tmp[i * 12 + 5] = y_pos - (OpenTKControl.openTKWindow.Height / scale);
		        vertices_tmp[i * 12 + 6] = x_pos + (OpenTKControl.openTKWindow.Width / scale);
		        vertices_tmp[i * 12 + 7] = y_pos - (OpenTKControl.openTKWindow.Height / scale);
		        vertices_tmp[i * 12 + 8] = x_pos + (OpenTKControl.openTKWindow.Width / scale);
		        vertices_tmp[i * 12 + 9] = y_pos;
		        vertices_tmp[i * 12 + 10] = x_pos;
		        vertices_tmp[i * 12 + 11] = y_pos;
    
		        texCoords_tmp[i * 12] = u;
		        texCoords_tmp[i * 12 + 1] = 1.0f - v + 1.0f / rows;
		        texCoords_tmp[i * 12 + 2] = u;
		        texCoords_tmp[i * 12 + 3] = 1.0f - v;
		        texCoords_tmp[i * 12 + 4] = u + 1.0f / cols;
		        texCoords_tmp[i * 12 + 5] = 1.0f - v;
		        texCoords_tmp[i * 12 + 6] = u + 1.0f / cols;
		        texCoords_tmp[i * 12 + 7] = 1.0f - v;
		        texCoords_tmp[i * 12 + 8] = u + 1.0f / cols;
		        texCoords_tmp[i * 12 + 9] = 1.0f - v + 1.0f / rows;
		        texCoords_tmp[i * 12 + 10] = u;
		        texCoords_tmp[i * 12 + 11] = 1.0f - v + 1.0f / rows;*/

                vertices_tmp.Add(x_pos);
                vertices_tmp.Add(y_pos);
                vertices_tmp.Add(x_pos);
                vertices_tmp.Add(y_pos - (OpenTKControl.openTKWindow.Height / scale));
                vertices_tmp.Add(x_pos + (OpenTKControl.openTKWindow.Width / scale));
                vertices_tmp.Add(y_pos - (OpenTKControl.openTKWindow.Height / scale));
                vertices_tmp.Add(x_pos + (OpenTKControl.openTKWindow.Width / scale));
                vertices_tmp.Add(y_pos - (OpenTKControl.openTKWindow.Height / scale));
                vertices_tmp.Add(x_pos + (OpenTKControl.openTKWindow.Width / scale));
                vertices_tmp.Add(y_pos);
                vertices_tmp.Add(x_pos);
                vertices_tmp.Add(y_pos);

                texCoords_tmp.Add(u);
                texCoords_tmp.Add(1.0f - v + 1.0f / rows);
                texCoords_tmp.Add(u);
                texCoords_tmp.Add(1.0f - v);
                texCoords_tmp.Add(u + 1.0f / cols);
                texCoords_tmp.Add(1.0f - v);
                texCoords_tmp.Add(u + 1.0f / cols);
                texCoords_tmp.Add(1.0f - v);
                texCoords_tmp.Add(u + 1.0f / cols);
                texCoords_tmp.Add(1.0f - v + 1.0f / rows);
                texCoords_tmp.Add(u);
                texCoords_tmp.Add(1.0f - v + 1.0f / rows);
	        }

	        int vertexAttributeLoc = GL.GetAttribLocation(OpenTKControl.shader.ID(), "InVertex");
	        int texCoordsAttributeLoc = GL.GetAttribLocation(OpenTKControl.shader.ID(), "InTexCoords");

	        GL.GenVertexArrays(1, out vertexArrayID);
            GL.BindVertexArray(vertexArrayID);

            vertexBufferIDs = new int[2];

	        GL.GenBuffers(2, vertexBufferIDs);
	        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferIDs[0]);
	        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(length * 12 * sizeof(float)), vertices_tmp.ToArray(), BufferUsageHint.StaticDraw);
	        GL.EnableVertexAttribArray(vertexAttributeLoc);
	        GL.VertexAttribPointer(vertexAttributeLoc, 2, VertexAttribPointerType.Float, false, 0, 0);

	        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferIDs[1]);
	        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(length * 12 * sizeof(float)), texCoords_tmp.ToArray(), BufferUsageHint.StaticDraw);
	        GL.EnableVertexAttribArray(texCoordsAttributeLoc);
	        GL.VertexAttribPointer(texCoordsAttributeLoc, 2, VertexAttribPointerType.Float, false, 0, 0);
  
	        verticesCount = length * 6;

            initialised = true;
        }

        public void Render()
        {
            if(initialised)
            {
	            GL.BindVertexArray(vertexArrayID);

	            if(highlighted)
	            {
		            shader.SetUniform("InColor", new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
	            }
	            else
	            {
                    shader.SetUniform("InColor", new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
	            }
	
	            GL.ActiveTexture(TextureUnit.Texture0);
	            GL.BindTexture(TextureTarget.Texture2D, fontAtlasID);
	            shader.SetUniform("Texture", fontAtlasID);

	            GL.DrawArrays(PrimitiveType.Triangles, 0, verticesCount);
	            GL.BindVertexArray(0);
            }
            else
                Console.WriteLine("Text not initialised.");
        }
    }
}