using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using Utils;

namespace Core
{
    class Canvas
    {
        protected int vaoHandle;
        protected int positionVboHandle;
        protected int normalVboHandle;
        protected int eboHandle;

        protected Vector3[] positionVboData;
        protected int[] indicesVboData;

        private Shader shader;
        
        private int width;
        public int Width { get { return width; } }

        private int height;
        public int Height { get { return height; } }

        public Canvas(int w, int h)
        {
            width = w;
            height = h;

            positionVboData = new Vector3[] {
            new Vector3(-1f, -1f, 0.0f),
            new Vector3(1.0f, -1.0f, 0.0f),
            new Vector3(1.0f, 1.0f, 0.0f),
            new Vector3(-1.0f, 1.0f, 0.0f) };

            indicesVboData = new int[] {
                0, 1, 2, 0, 2, 3 };
        }

        public void Resize(int w, int h)
        {
            width = w;
            height = h;
        }
        /*
        public void SetShader(Shader s)
        {
            shader = s;
        }
        */
        public void Create(Shader s)
        {
            shader = s;
            createVBOs();
            createVAOs();
        }

        protected void createVBOs()
        {
            if (positionVboData == null || indicesVboData == null)
            {
                Log.WriteLine(Log.LOG_ERROR, "can not create VBOs without positionVboData and indicesVboData for model '" + shader.Name + "'");
                return;
            }

            GL.GenBuffers(1, out positionVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(positionVboData.Length * Vector3.SizeInBytes), positionVboData, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out normalVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(positionVboData.Length * Vector3.SizeInBytes), positionVboData, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out eboHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * indicesVboData.Length), indicesVboData, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        protected void createVAOs()
        {
            if (shader == null)
            {
                Log.WriteLine(Log.LOG_ERROR, "Can not create VAOs without shader set for canvas.");
                return;
            }

            GL.GenVertexArrays(1, out vaoHandle);
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            GL.BindAttribLocation(shader.ProgramHandle, 0, "in_position");

            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalVboHandle);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            GL.BindAttribLocation(shader.ProgramHandle, 1, "in_normal");

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);

            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(BeginMode.Triangles, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);
        }

    }
}
