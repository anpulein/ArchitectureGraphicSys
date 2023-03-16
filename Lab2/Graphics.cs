using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GL = OpenTK.Graphics.OpenGL4.GL;

using ArchitectureGraphicSys;
using lib.Data;

namespace Lab2
{
    public class Graphics : GraphicsMain
    {
        private readonly float[] vertices = new float[]
        {
            -0.5f, 0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f,
            0.5f, 0.5f, 0.0f,
            0.5f, 0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f
        };

        private readonly Vector3 redColor = new Vector3(1.0f, 0.0f, 0.0f);
        private readonly Vector3 greenColor = new Vector3(0.0f, 1.0f, 0.0f);
        private readonly Vector3 blueColor = new Vector3(0.0f, 0.0f, 1.0f);
        private Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);
        private float stepOffsetX = 0.01f;
        private float stepOffsetY = 0.001f;

        /// <summary>
        /// Setting the window size and title window
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="height"></param>
        /// <param name="title"></param>
        public Graphics (int weight, int height, string title) : base(weight, height, title) {}
        /// <summary>
        /// Setting the title window
        /// </summary>
        /// <param name="title"></param>
        public Graphics (string title) : base(title) {}




        /// <summary>
        /// Update Frame
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            GetFPS((float)e.Time);
            
            base.OnUpdateFrame(e);
            
            // Esc - Exit current program
            KeyboardState input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape)) Close();
        }

        /// <summary>
        /// Starting at the very beginning of the program
        /// Init code
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();
            
            GL.ClearColor(1f, 1f, 1f, 1.0f);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            

            _shader = new Shader(Data.LOCAL_PATH_SHADER_VERTICAL, Data.LOCAL_PATH_SHADER_FRAGMENT);
            _shader.Use();
            
            _shader.SetVector3("red_color", redColor);
            _shader.SetVector3("green_color", greenColor);
            _shader.SetVector3("blue_color", blueColor);
            _shader.SetVector3("offset", offset);
            
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Code goes here.
            
            _shader.Use();

            SetPositionShader();
            
            GL.BindVertexArray(_vertexArrayObject);
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            SwapBuffers();
        }

        /// <summary>
        /// Editing size window app
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
            
            
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_vertexBufferObject);
            
            GL.DeleteProgram(_shader.Handle);
            
            base.OnUnload();
        }


        private void SetPositionShader()
        {
            offset = new Vector3(offset.X += stepOffsetX, offset.Y += stepOffsetY, offset.Z);

            if (offset.X > 0.5f) stepOffsetX *= -1;
            else if (offset.X < -0.5f) stepOffsetX *= -1;
            
            if (offset.Y > 0.5f) stepOffsetY *= -1;
            else if (offset.Y < -0.5f) stepOffsetY *= -1;
            
            
            _shader.SetVector3("offset", offset);
        }
    }
}