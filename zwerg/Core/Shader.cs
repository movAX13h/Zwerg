using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using OpenTK.Graphics.OpenGL;
using OpenTK;

using Utils;
using System.Drawing;

namespace Core
{
    class Shader
    {
        private const string defaultVertexShaderSource = "attribute vec2 pos; void main() { gl_Position = vec4(pos.x,pos.y,0.0,1.0); }";

        protected string vertexShaderSource;
        public string VertexShaderSource { get { return vertexShaderSource; } }
        protected int vertexShaderHandle;

        protected string fragmentShaderSource;
        public string FragmentShaderSource { get { return fragmentShaderSource; } set { fragmentShaderSource = value; } }
        protected int fragmentShaderHandle;

        protected int programHandle;
        public int ProgramHandle { get { return programHandle; } }

        private Dictionary<string, int> uniformLocations;

        protected string name;
        public string Name { get { return name; } }

        private bool ready = false;

        private string compileLog;
        public string CompileLog { get { return compileLog; } }

        public Shader(string name)
        {
            this.name = name;
            vertexShaderSource = defaultVertexShaderSource;
            fragmentShaderSource = "";
            uniformLocations = new Dictionary<string, int>();
            compileLog = "";
        }

        public bool LoadFromFile(string path)
        {
            if (!File.Exists(path)) return false;
            fragmentShaderSource = File.ReadAllText(path);
            return true;
        }

        public bool LoadFromSource(string fsSource)
        {
            fragmentShaderSource = fsSource;
            return true;
        }

        public bool Compile()
        {
            if (ready) return true;
            ready = true;

            if (vertexShaderSource.Length == 0 || fragmentShaderSource.Length == 0) return false;

            vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertexShaderHandle, vertexShaderSource);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);

            GL.CompileShader(vertexShaderHandle);
            GL.CompileShader(fragmentShaderHandle);

            compileLog = GL.GetShaderInfoLog(vertexShaderHandle).Trim();
            Log.WriteLine("vert shader '" + name + "' compile log '" + compileLog + "'");
            if (compileLog.Contains("error")) return false;

            compileLog = GL.GetShaderInfoLog(fragmentShaderHandle).Trim();
            Log.WriteLine("frag shader '" + name + "' compile log '" + compileLog + "'");
            if (compileLog.Contains("error")) return false;

            programHandle = GL.CreateProgram();

            GL.AttachShader(programHandle, vertexShaderHandle);
            GL.AttachShader(programHandle, fragmentShaderHandle);

            GL.LinkProgram(programHandle);

            Log.WriteLine("uploaded shader prog '" + name + "'; log: " + GL.GetProgramInfoLog(programHandle).Trim());

            return true;
        }
        
        public virtual void Bind()
        {
            GL.UseProgram(programHandle);
        }

        public virtual void UnBind()
        {
            GL.UseProgram(0);
        }

        private int uniformLocation(string name)
        {
            if (uniformLocations.ContainsKey(name)) return uniformLocations[name];
            int location = GL.GetUniformLocation(programHandle, name);
            uniformLocations.Add(name, location);
            return location;
        }

        // set uniform methods

        public bool SetUniform(string name, Asset asset)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform1(location, asset.Unit - TextureUnit.Texture0);
            return true;
        }

        /*
        public bool SetUniform(string name, TextureUnit unit)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform1(location, (int)unit);
            return true;
        }
        */
        
        public bool SetUniform(string name, Matrix4 matrix)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.UniformMatrix4(location, false, ref matrix);
            return true;
        }

        public bool SetUniform(string name, int i)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform1(location, 1, ref i);
            return true;
        }

        public bool SetUniform(string name, float f)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform1(location, 1, ref f);
            return true;
        }

        public bool SetUniform(string name, ref Vector2 v)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform2(location, ref v);
            return true;
        }

        public bool SetUniform(string name, Vector2 v)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform2(location, ref v);
            return true;
        }

        public bool SetUniform(string name, ref Vector3 v)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform3(location, ref v);
            return true;
        }

        public bool SetUniform(string name, Vector3 v)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform3(location, ref v);
            return true;
        }

        public bool SetUniform(string name, Color c)
        {
            Vector3 v = new Vector3((float)c.R / 256.0f, (float)c.G / 256.0f, (float)c.B / 256.0f);
            return SetUniform(name, v);
        }

        /*
        public bool SetUniform(string name, Vector3[] vs, int fixedLength)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;

            // serialize floats
            float[] fs = new float[fixedLength * 3];
            int i = 0;
            foreach (Vector3 v in vs)
            {
                fs[i++] = v.X;
                fs[i++] = v.Y;
                fs[i++] = v.Z;
            }

            while (i < fs.Length - 1)
            {
                i++;
                fs[i] = 0;
            }

            GL.Uniform3(location, fs.Length, fs);
            return true;
        }
        */

        public bool SetUniform(string name, float[] fs)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform1(location, fs.Length, fs);
            return true;
        }

        public bool SetUniform3(string name, float[] fs)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform3(location, (int)(fs.Length / 3.0f), fs);
            return true;
        }

        public bool SetUniform(string name, Vector4 v)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform4(location, ref v);
            return true;
        }

        public bool SetUniform(string name, ref Vector4 v)
        {
            int location = uniformLocation(name);
            if (location < 0) return false;
            GL.Uniform4(location, ref v);
            return true;
        }

    }
}
