using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Visualiser
{
    public class Shader
    {
        private bool initialised;
        private int shader_vert = 0;
        private int shader_frag = 0;
        public int program_id;

        /// <summary>
        /// textFileRead loads in a standard text file from a given fileName and
	    /// then returns it as a string.
        /// </summary>
        /// <param name="fileName">Shader file name</param>
        /// <returns>File as a single string</returns>
        private string ReadFile(string fileName) 
        {
            StreamReader reader = new StreamReader(fileName);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Given a shader and the fileName associated with it, validateShader will
	    /// then get information from OpenGl on whether or not the shader was compiled successfully
	    /// and if it wasn't, it will output the file with the problem, as well as the problem.
        /// </summary>
        /// <param name="shader">Shader ID</param>
        private void ValidateShader(int shader) 
        {
            string message;
            GL.GetShaderInfoLog(shader, out message);

            if (message.Length > 0) // If we have any information to display
                Console.WriteLine("Shader " + shader + " compile error: " + message);
        }

        public Shader()
        {
            initialised = false; // Declare we have not initialized the shader yet
        }
	        
        /// <summary>
        /// Constructor for a Shader object which creates a GLSL shader based on a given
	    /// vertex and fragment shader file.
        /// </summary>
        /// <param name="vsFile">Vertex shader</param>
        /// <param name="fsFile">Fragment shader</param>
        public Shader(string vsFile, string fsFile)
        {
            initialised = false; // Declare we have not initialized the shader yet
    
            Init(vsFile, fsFile); // Initialize the shader
        }
        
	        
        /// <summary>
        /// init will take a vertex shader file and fragment shader file, and then attempt to create a valid
	    /// shader program from these. It will also check for any shader compilation issues along the way.
        /// </summary>
        /// <param name="vsFile">Vertex shader</param>
        /// <param name="fsFile">Fragment shader</param>
        void Init(string vsFile, string fsFile)
        {
            if (initialised) // If we have already initialized the shader
                return;

            initialised = true; // Mark that we have initialized the shader
    
            shader_vert = GL.CreateShader(ShaderType.VertexShader); // Create a vertex shader
            shader_frag = GL.CreateShader(ShaderType.FragmentShader); // Create a fragment shader
    
            string vsText = ReadFile(vsFile); // Read in the vertex shader
            string fsText = ReadFile(fsFile); // Read in the fragment shader

            if (vsText == string.Empty || fsText == string.Empty)
            {
                Console.WriteLine("Either vertex shader or fragment shader file not found."); // Output the error
                return;
            }

            GL.ShaderSource(shader_vert, vsText);
            GL.CompileShader(shader_vert);
            ValidateShader(shader_vert);
    
            GL.ShaderSource(shader_frag, fsText);
            GL.CompileShader(shader_frag);
            ValidateShader(shader_frag);
    
            program_id = GL.CreateProgram(); // Create a GLSL program
	        GL.AttachShader(program_id, shader_vert); // Attach a vertex shader to the program
            GL.AttachShader(program_id, shader_frag); // Attach the fragment shader to the program

	        GL.BindAttribLocation(program_id, 0, "InVertex"); // Bind a constant attribute location for positions of vertices
            GL.BindAttribLocation(program_id, 1, "InNormal"); // Bind another constant attribute location, this time for color
	        GL.BindAttribLocation(program_id, 2, "InColor"); // Bind another constant attribute location, this time for color

            GL.LinkProgram(program_id); // Link the vertex and fragment shaders in the program
            GL.ValidateProgram(program_id); // Validate the shader program
        }

        
	        
        /// <summary>
        /// Shader program id
        /// </summary>
        /// <returns>Returns the integer value associated with the shader program</returns>
        public int ID()
        {
            return program_id; // Return the shaders identifier
        }

        /// <summary>
        /// bind attaches the shader program for use by OpenGL
        /// </summary>
        public void Bind()
        { 
            GL.UseProgram(program_id);
        }

        /// <summary>
        /// Unbind deattaches the shader program from OpenGL
        /// </summary>
        public void Unbind()
        {
            GL.UseProgram(0);
        }

        // Setting integers
        public void SetUniform(string name, int value)
        {
	        int loc = GL.GetUniformLocation(program_id, name);
            GL.Uniform1(loc, value);
        }

        // Setting floats
        public void SetUniform(string name, float value)
        {
	        int loc = GL.GetUniformLocation(program_id, name);
	        GL.Uniform1(loc, value);
        }

        // Setting vectors
        public void SetUniform(string name, Vector2 vector)
        {
	        int loc = GL.GetUniformLocation(program_id, name);
	        GL.Uniform2(loc, vector);
        }

        public void SetUniform(string name, Vector3 vector)
        {
	        int loc = GL.GetUniformLocation(program_id, name);
	        GL.Uniform3(loc, vector);
        }

        public void SetUniform(string name, Vector4 vector)
        {
	        int loc = GL.GetUniformLocation(program_id, name);
	        GL.Uniform4(loc, vector);
        }

        // Setting 3x3 matrices
        public void SetUniform(string name, Matrix3 matrix)
        {
	        int loc = GL.GetUniformLocation(program_id, name);
            GL.UniformMatrix3(loc, false, ref matrix);
        }

        // Setting 4x4 matrices
        public void SetUniform(string name, ref Matrix4 matrix)
        {
	        int loc = GL.GetUniformLocation(program_id, name);
            GL.UniformMatrix4(loc, false, ref matrix);
        }
    }
}
