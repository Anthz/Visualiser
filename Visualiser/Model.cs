using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Visualiser
{
    public class Model
    {
        List<Vector3> vertices;
        List<Vector3> normals;
        List<Vector2> textureVertices;

        public int vertexCount;

        public int vertexArrayID;
        int[] vertexBufferIDs;

        bool textured;

        public Model(string fileName, bool textured)
        {
            this.textured = textured;
            LoadObj(fileName);
            BuildVAO();
        }

        void BuildVAO()
        {
            int vertexAttributeLoc = GL.GetAttribLocation(OpenTKControl.shader.ID(), "InVertex");
	        int normalAttributeLoc = GL.GetAttribLocation(OpenTKControl.shader.ID(), "InNormal");

            GL.GenVertexArrays(1, out vertexArrayID);
            GL.BindVertexArray(vertexArrayID);

            if(textured)
            {
                vertexBufferIDs = new int[3];
                GL.GenBuffers(3, vertexBufferIDs);
            }
            else
            {
                vertexBufferIDs = new int[2];
                GL.GenBuffers(2, vertexBufferIDs);
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferIDs[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Count * Vector3.SizeInBytes), vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(vertexAttributeLoc);
            GL.VertexAttribPointer(vertexAttributeLoc, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferIDs[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(normals.Count * Vector3.SizeInBytes), normals.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(normalAttributeLoc);
            GL.VertexAttribPointer(normalAttributeLoc, 3, VertexAttribPointerType.Float, false, 0, 0);

            if (textured)
            {
                int textureAttributeLoc = GL.GetAttribLocation(OpenTKControl.shader.ID(), "InTexCoords");
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferIDs[2]);
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(textureVertices.Count * Vector2.SizeInBytes), textureVertices.ToArray(), BufferUsageHint.StaticDraw);
                GL.EnableVertexAttribArray(textureAttributeLoc);
                GL.VertexAttribPointer(textureAttributeLoc, 2, VertexAttribPointerType.Float, false, 0, 0);
            }

            GL.BindVertexArray(0);
        }

        void LoadObj(string fileName) 
        {
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            textureVertices = new List<Vector2>();
	        List<Vector3> tempVertices = new List<Vector3>();
            List<Vector3> tempNormals = new List<Vector3>();
	        List<Vector2> tempTextureVertices = new List<Vector2>();
            List<int> vertexIndices = new List<int>();
            List<int> normalIndices = new List<int>();
            List<int> textureIndices = new List<int>();

	        StreamReader reader = new StreamReader(fileName);
	        if(reader == StreamReader.Null) 
	        { 
		        Console.WriteLine("Failed to open file: " + fileName);
                return;
	        }

	        string line;
	        while((line = reader.ReadLine()) != null) 
	        {
			    if(line.StartsWith("vn"))
			    {
                    Vector3 normal;
                    List<string> values = line.Split(' ').ToList();
                    values.RemoveAll(s => s.Contains(" "));                    

                    normal.X = float.Parse(values[1]);
                    normal.Y = float.Parse(values[2]);
                    normal.Z = float.Parse(values[3]);
				    tempNormals.Add(normal);
			    }

			    else if(line.StartsWith("vt"))
			    {
                    Vector2 tex;
                    List<string> values = line.Split(' ').ToList();
                    values.RemoveAll(s => s.Contains(" "));                    

                    tex.X = float.Parse(values[1]);
                    tex.Y = float.Parse(values[2]);
				    tempTextureVertices.Add(tex);
			    }

                else if(line.StartsWith("v"))
			    {
				    Vector3 vertex;
                    List<string> values = line.Split(' ').ToList();
                    values.RemoveAll(s => s.Contains(" "));                    

                    vertex.X = float.Parse(values[1]);
                    vertex.Y = float.Parse(values[2]);
                    vertex.Z = float.Parse(values[3]);
				    tempVertices.Add(vertex);
			    }

			    else if(line.StartsWith("f"))
			    {
				    List<int> values = new List<int>();
                    List<string> tempValues = line.Split(' ').ToList();
                    tempValues.RemoveAt(0);
                    tempValues.RemoveAll(s => s.Contains(" "));
                    foreach(string s in tempValues)
	                {
		                string[] temp = s.Split('/');
                        for(int i = 0; i < temp.Length; i++)
	                    {
                            if(temp[i] == "")
                                temp[i] = "0";
			                values.Add(int.Parse(temp[i]));
	                    }
                        vertexIndices.Add(values[0]);
                        if(textured)
                            textureIndices.Add(values[1]);
                        normalIndices.Add(values[2]);
	                }
			    }
	        }

	        //Ordering vertices
	        for(int i = 0; i < vertexIndices.Count; i++)
	        {
		        int vertexIndex = vertexIndices[i];

		        Vector3 vertex = tempVertices[vertexIndex - 1];
		        vertices.Add(vertex);
	        }

	        for(int i = 0; i < normalIndices.Count; i++)
	        {
		        int normalIndex = normalIndices[i];

		        Vector3 normal = tempNormals[normalIndex - 1];
		        normals.Add(normal);
	        }

            if (textured)
            {
                for (int i = 0; i < textureIndices.Count; i++)
                {
                    int textureIndex = textureIndices[i];

                    Vector2 textureVertex = tempTextureVertices[textureIndex - 1];
                    textureVertices.Add(textureVertex);
                }
            }

            vertexCount = vertexIndices.Count;
        }
    }
}
